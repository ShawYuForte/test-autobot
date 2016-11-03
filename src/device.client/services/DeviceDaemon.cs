﻿#region

using System;
using System.Net;
using System.Threading;
using AutoMapper;
using forte.devices.data;
using forte.devices.entities;
using forte.devices.extensions;
using forte.devices.models;
using forte.models.devices;
using forte.services;
using RestSharp;
using StreamingDeviceState = forte.devices.models.StreamingDeviceState;
using StreamingDeviceStatuses = forte.devices.models.StreamingDeviceStatuses;

#endregion

namespace forte.devices.services
{
    /// <summary>
    ///     Streaming device manager, responds to WebSocket messages when there are new commands, and then for each command:
    ///     - Fetches command
    ///     - Saves locally
    ///     - Executes the command
    ///     - Saves state locally
    ///     - Saves command state on the server
    ///     - Publishes new client state
    /// </summary>
    public class DeviceDaemon : IDeviceDaemon
    {
        public delegate void MessageReceivedDelegate(string message);

        private static readonly object Key = new object();

        private RestClient _client;
        private readonly IConfigurationManager _configurationManager;
        private Guid _deviceId;
        private readonly IDeviceRepository _deviceRepository;
        private readonly ILogger _logger;
        private readonly IServerListener _serverListener;
        private RestClient _streamClient;
        private DateTime _lastPublishTime = DateTime.MinValue;

        private readonly IStreamingClient _streamingClient;

        public DeviceDaemon(IDeviceRepository deviceRepository, IStreamingClient streamingClient, ILogger logger,
            IConfigurationManager configurationManager, IServerListener serverListener)
        {
            _deviceRepository = deviceRepository;
            _streamingClient = streamingClient;
            _logger = logger;
            _configurationManager = configurationManager;
            _serverListener = serverListener;
            serverListener.MessageReceived += OnServerMessageReceived;
        }

        public void Init()
        {
            Init(Guid.Empty);
        }
        public void Init(Guid deviceId)
        {
            SetDefaultSettings();
            var config = _configurationManager.GetDeviceConfig();
            if (deviceId != Guid.Empty)
            {
                if (config.DeviceId != Guid.Empty && config.DeviceId != deviceId)
                {
                    _logger.Warning("New device identifier specified, changing from {@oldId} to {@newId}",
                        config.DeviceId, deviceId);
                    _configurationManager.UpdateSetting(nameof(models.StreamingDeviceConfig.DeviceId), deviceId);
                    var storedState = GetState();
                    if (storedState.DeviceId != deviceId)
                    {
                        storedState.DeviceId = deviceId;
                        _deviceRepository.Save(storedState);
                    }
                }
            }
            _client = new RestClient($"{config.Get<string>(SettingParams.ServerApiPath)}/devices/");
            _streamClient = new RestClient($"{config.Get<string>(SettingParams.ServerApiPath)}/streams/");
            _deviceId = config.DeviceId;
            _logger.Information("Devide unique identifier {@deviceId}", _deviceId);
        }

        private bool FetchRequested { get; set; }

        private bool Stopped { get; set; }

        public void ForceResetToIdle()
        {
            _logger.Warning("Force resetting device to idle");

            var state = GetState();
            _streamingClient.ShutDown();
            state.Status = StreamingDeviceStatuses.Idle;
            _deviceRepository.Save(state);

            _logger.Debug("Device reset to idle!");
        }

        public StreamingDeviceState GetState()
        {
            var state = _deviceRepository.GetDeviceState();
            if (state != null) return state;

            var config = _configurationManager.GetDeviceConfig();

            state = new StreamingDeviceState
            {
                DeviceId = config.DeviceId,
                StateCapturedOn = DateTime.UtcNow,
                Status = StreamingDeviceStatuses.Idle
            };
            _deviceRepository.Save(state);
            return state;
        }

        private void TryPublishState()
        {
            try
            {
                PublishState();
            }
            catch (Exception exception)
            {
                _logger.Error(exception, "Could not publish state");
            }
        }

        /// <summary>
        ///     Publish device state to the server
        /// </summary>
        public bool PublishState()
        {
            _logger.Debug("Publishing state");

            var deviceState = GetState();
            var request = new RestRequest($"{deviceState.DeviceId}/state", Method.POST)
            {
                JsonSerializer = NewtonsoftJsonSerializer.Default
            };
            request.AddHeader("Content-Type", "application/json; charset=utf-8");
            request.AddJsonBody(deviceState);
            var response = _client.Execute(request);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                // Log error
                throw new Exception(response.ErrorMessage ?? $"Publishing state, response was {response.StatusCode}");
            }

            _lastPublishTime = DateTime.Now;
            _logger.Debug("State published");

            return true;
        }

        public void QueryServer()
        {
            FetchRequested = true;
        }

        public void Run()
        {
            var config = _configurationManager.GetDeviceConfig();
            if (config.DeviceId == Guid.Empty)
                throw new NotSupportedException("No device identifier configured");

            var seconds = 30;
            _serverListener.Connect();
            while (!Stopped)
            {
                seconds--;
                if ((seconds <= 0) || FetchRequested)
                {
                    FetchRequested = false;
                    seconds = 30;

                    try
                    {
                        FetchCommand();
                    }
                    catch (Exception exception)
                    {
                        _logger.Error(exception, exception.Message);
                    }
                }

                // Keep the server informed that we are alive
                if ((DateTime.Now - _lastPublishTime).Minutes > 30)
                    TryPublishState();

                Thread.Sleep(1000);
            }
        }

        public void Stop()
        {
            _serverListener.Disconnect();
            Stopped = true;
        }


        public void FetchCommand()
        {
            lock (Key)
            {
                ThreadSafeFetchCommand();
            }
        }

        public bool StartProgram(StreamingDeviceCommandModel command)
        {
            _logger.Debug("Starting program for command {@command}", command);

            if (command.VideoStreamId == null)
                throw new Exception("Video stream id not provided, cannot start program");
            var videoStreamId = command.VideoStreamId.Value;

            var state = GetState();
            if (state.ActiveVideoStreamId != videoStreamId)
            {
                _logger.Fatal(
                    "Cannot stream for program {@newVideoStream}, prepared to stream for video stream {@oldVideoStream}",
                    videoStreamId, state.ActiveVideoStreamId);
                throw new Exception("Cannot stream, device prepared to stream for a different stream");
            }

            switch (state.Status)
            {
                case StreamingDeviceStatuses.Idle:
                    _logger.Error("Request to start program against idle device (expected streaming)");
                    return false;
                case StreamingDeviceStatuses.Streaming:
                case StreamingDeviceStatuses.Recording:
                case StreamingDeviceStatuses.StreamingAndRecording:
                    _streamingClient.StartProgram();
                    break;
                case StreamingDeviceStatuses.StreamingProgram:
                case StreamingDeviceStatuses.StreamingAndRecordingProgram:
                case StreamingDeviceStatuses.RecordingProgram:
                    _logger.Warning("Request to start program against device with started program");
                    return false;
                case StreamingDeviceStatuses.Offline:
                case StreamingDeviceStatuses.Error:
                    throw new Exception($"Device with status {state.Status} cannot accept commands");
                default:
                    throw new ArgumentOutOfRangeException();
            }

            state.Status = StreamingDeviceStatuses.StreamingProgram;
            _deviceRepository.Save(state);

            _logger.Debug("Program started.");

            return true;
        }

        public bool StartStreaming(StreamingDeviceCommandModel command)
        {
            _logger.Debug("Starting streaming for command {@command}", command);

            if (command.VideoStreamId == null)
                throw new Exception("Video stream id not provided, cannot start streaming");
            var videoStreamId = command.VideoStreamId.Value;

            var videoStream = DownloadStreamInformation(videoStreamId);
            if (videoStream == null)
                throw new Exception("Video stream could not be downloaded, cannot prepare for streaming");

            var state = GetState();

            switch (state.Status)
            {
                case StreamingDeviceStatuses.Idle:
                    state.StreamingPresetLoadHash = _streamingClient.LoadVideoStreamPreset(videoStream);
                    _streamingClient.StartStreaming();
                    break;
                case StreamingDeviceStatuses.Streaming:
                case StreamingDeviceStatuses.StreamingProgram:
                case StreamingDeviceStatuses.StreamingAndRecording:
                case StreamingDeviceStatuses.StreamingAndRecordingProgram:
                case StreamingDeviceStatuses.Recording:
                case StreamingDeviceStatuses.RecordingProgram:
                    _logger.Warning("Request to start streaming against device with state {@state}", state);
                    return false;
                case StreamingDeviceStatuses.Offline:
                case StreamingDeviceStatuses.Error:
                    throw new Exception($"Device with status {state.Status} cannot accept commands");
                default:
                    throw new ArgumentOutOfRangeException();
            }

            state.ActiveVideoStreamId = videoStream.Id;
            state.Status = StreamingDeviceStatuses.Streaming;
            _deviceRepository.Save(state);

            _logger.Debug("Streaming started.");

            return true;
        }

        /// <summary>
        ///     Stop streaming for the specified video stream identifier. The video stream identifier is there to ensure that the
        ///     request is coming for the right stream
        /// </summary>
        /// <param name="command"></param>
        public bool StopProgram(StreamingDeviceCommandModel command)
        {
            _logger.Debug("Stopping program for command {@command}", command);

            var state = GetState();

            switch (state.Status)
            {
                case StreamingDeviceStatuses.Idle:
                    _logger.Error("Request to stop program against idle device (expected streaming program)");
                    return false;
                case StreamingDeviceStatuses.Streaming:
                case StreamingDeviceStatuses.StreamingAndRecording:
                case StreamingDeviceStatuses.Recording:
                    _logger.Warning("Request to stop program against device without a running program. {@state}", state);
                    return false;
                case StreamingDeviceStatuses.StreamingProgram:
                case StreamingDeviceStatuses.StreamingAndRecordingProgram:
                case StreamingDeviceStatuses.RecordingProgram:
                    _streamingClient.StopProgram();
                    state.Status = StreamingDeviceStatuses.Streaming;
                    break;
                case StreamingDeviceStatuses.Offline:
                case StreamingDeviceStatuses.Error:
                    throw new Exception($"Device with status {state.Status} cannot accept commands");
                default:
                    throw new ArgumentOutOfRangeException();
            }

            _deviceRepository.Save(state);

            _logger.Debug("Program stopped.");

            return true;
        }

        /// <summary>
        ///     Stop streaming for the specified video stream identifier. The video stream identifier is there to ensure that the
        ///     request is coming for the right stream
        /// </summary>
        /// <param name="command"></param>
        public bool StopStreaming(StreamingDeviceCommandModel command)
        {
            _logger.Debug("Stopping streaming for command {@command}", command);

            var state = GetState();

            switch (state.Status)
            {
                case StreamingDeviceStatuses.Idle:
                    _logger.Error("Request to stop streaming against idle device (expected streaming)");
                    return false;
                case StreamingDeviceStatuses.Streaming:
                case StreamingDeviceStatuses.StreamingAndRecording:
                case StreamingDeviceStatuses.Recording:
                    _streamingClient.StopStreaming(true);
                    state.Status = StreamingDeviceStatuses.Idle;
                    break;
                case StreamingDeviceStatuses.StreamingProgram:
                case StreamingDeviceStatuses.StreamingAndRecordingProgram:
                case StreamingDeviceStatuses.RecordingProgram:
                    _logger.Error("Request to stop stream against device running program");
                    return false;
                case StreamingDeviceStatuses.Offline:
                case StreamingDeviceStatuses.Error:
                    throw new Exception($"Device with status {state.Status} cannot accept commands");
                default:
                    throw new ArgumentOutOfRangeException();
            }

            state.ActiveVideoStreamId = null;
            _deviceRepository.Save(state);

            _logger.Debug("Streaming stopped.");

            return true;
        }

        public void ThreadSafeFetchCommand()
        {
            _logger.Debug("Fetching command...");

            var request = new RestRequest($"{_deviceId}/commands/next", Method.GET)
            {
                JsonSerializer = NewtonsoftJsonSerializer.Default
            };
            var response = _client.Execute<StreamingDeviceCommandModel>(request);
            // Not found if no command
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                _logger.Debug("No command available, exiting");
                // ... so just exit
                return;
            }
            if (response.StatusCode != HttpStatusCode.OK)
            {
                _logger.Error("Failed fetching command, response {@response}", response);
                throw new Exception(response.ErrorMessage ?? response.StatusDescription);
            }

            _logger.Debug("Command retrieved {@command}", response.Data);
            var command = response.Data;

            SaveCommandLocally(command);

            try
            {
                command.ExecutionAttempts++;
                ExecuteCommand(command);
                command.ExecutedOn = DateTime.UtcNow;
                command.ExecutionSucceeded = true;
                PublishState();
            }
            catch (Exception exception)
            {
                if (string.IsNullOrWhiteSpace(command.ExecutionMessages))
                {
                    command.ExecutionMessages = exception.Message;
                }
                else
                {
                    command.ExecutionMessages += $"{exception.Message};\n\r";
                }

                if (command.ExecutionAttempts >= command.MaxAttemptsAllowed)
                {
                    command.ExecutedOn = DateTime.UtcNow;
                    command.ExecutionSucceeded = false;
                    PublishTemporaryFailedState();
                    _logger.Fatal(exception, "Could not execute command {@command}", command);
                }
                else
                {
                    _logger.Error(exception, "Could not execute command {@command}", command);
                }
            }

            SaveCommandLocally(command);

            try
            {
                SaveCommandOnServer(command);
            }
            catch (Exception exception)
            {
                _logger.Fatal(exception, "Could not save command on the server {@command}", command);
            }
        }

        private void PublishTemporaryFailedState()
        {
            var state = GetState();
            var status = state.Status;
            if (status != StreamingDeviceStatuses.Error)
            {
                state.Status = StreamingDeviceStatuses.Error;
                _deviceRepository.Save(state);
            }
            PublishState();
            if (status != state.Status)
            {
                state.Status = status;
                _deviceRepository.Save(state);
            }
        }

        private VideoStreamModel DownloadStreamInformation(Guid videoStreamId)
        {
            var request = new RestRequest($"{videoStreamId}?extended=true", Method.GET);
            var response = _streamClient.Execute<VideoStreamModel>(request);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                var message = response.ErrorMessage ?? response.StatusDescription;
                // TODO handle logging / exceptions
                _logger.Fatal(
                    "Could not retrieve video stream details for {@videoStreamId} due to {@message} from response {@response}",
                    videoStreamId, message, response);
                throw new Exception($"Could not retrieve video stream details for {videoStreamId} due to {message}");
            }
            _deviceRepository.SaveVideoStream(Mapper.Map<VideoStream>(response.Data));
            return response.Data;
        }


        private void ExecuteCommand(StreamingDeviceCommandModel command)
        {
            bool result;
            var resultMessage = string.Empty;

            switch (command.Command)
            {
                case StreamingDeviceCommands.UpdateState:
                    result = PublishState();
                    break;
                case StreamingDeviceCommands.StartStreaming:
                    result = StartStreaming(command);
                    break;
                case StreamingDeviceCommands.StopStreaming:
                    result = StopStreaming(command);
                    break;
                case StreamingDeviceCommands.StartProgram:
                    result = StartProgram(command);
                    break;
                case StreamingDeviceCommands.StopProgram:
                    result = StopProgram(command);
                    break;
                case StreamingDeviceCommands.ResetToIdle:
                    result = ResetToIdle(command);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            command.ExecutionMessages = resultMessage;
            command.ExecutionSucceeded = result;
            command.ExecutedOn = DateTime.UtcNow;
        }

        private void OnServerMessageReceived(string message)
        {
            if (message != "CommandAvailable") return;
            FetchRequested = true;
        }

        private bool ResetToIdle(StreamingDeviceCommandModel command)
        {
            _logger.Debug("Resetting device to idle for command {@command}", command);

            var state = GetState();

            switch (state.Status)
            {
                case StreamingDeviceStatuses.Idle:
                    // already idle, asssume it worked
                    return true;
                case StreamingDeviceStatuses.Streaming:
                case StreamingDeviceStatuses.StreamingAndRecording:
                case StreamingDeviceStatuses.Recording:
                case StreamingDeviceStatuses.StreamingProgram:
                case StreamingDeviceStatuses.StreamingAndRecordingProgram:
                case StreamingDeviceStatuses.RecordingProgram:
                    if (state.ActiveVideoStreamId != command.VideoStreamId)
                        return false;
                    _streamingClient.ShutDown();
                    state.Status = StreamingDeviceStatuses.Idle;
                    break;
                case StreamingDeviceStatuses.Offline:
                case StreamingDeviceStatuses.Error:
                    throw new Exception($"Device with status {state.Status} cannot accept commands");
                default:
                    throw new ArgumentOutOfRangeException();
            }

            _deviceRepository.Save(state);

            _logger.Debug("Device reset to idle.");

            return true;
        }

        private void SaveCommandLocally(StreamingDeviceCommandModel command)
        {
            var commandEntity = Mapper.Map<StreamingDeviceCommandEntity>(command);
            _deviceRepository.SaveCommand(commandEntity);
        }

        /// <summary>
        ///     Saves command state on the server
        /// </summary>
        /// <param name="commandModel"></param>
        /// <exception cref="Exception">If server update fails</exception>
        private void SaveCommandOnServer(StreamingDeviceCommandModel commandModel)
        {
            _deviceRepository.SaveCommand(Mapper.Map<StreamingDeviceCommandEntity>(commandModel));

            // TODO handle duplicates and queuing
            var request = new RestRequest($"{_deviceId}/commands/{commandModel.Id}", Method.PUT)
            {
                JsonSerializer = NewtonsoftJsonSerializer.Default
            };
            request.AddHeader("Content-Type", "application/json; charset=utf-8");
            request.AddJsonBody(commandModel);
            var response = _client.Execute<StreamingDeviceCommandModel>(request);

            if (response.StatusCode == HttpStatusCode.OK) return;

            _logger.Error("Could not update command because of {@status}, response {@resposne}",
                response.ErrorMessage ?? response.StatusDescription, response);
            throw new Exception(response.ErrorMessage ?? response.StatusDescription);
        }

        private void SetDefaultSettings()
        {
            var config = _configurationManager.GetDeviceConfig();
            if (string.IsNullOrWhiteSpace(config.Get<string>(SettingParams.ServerRootPath)))
                config = _configurationManager.UpdateSetting(SettingParams.ServerRootPath,
                    "http://forte-devapi.azurewebsites.net");
            if (string.IsNullOrWhiteSpace(config.Get<string>(SettingParams.ServerApiPath)))
                _configurationManager.UpdateSetting(SettingParams.ServerApiPath,
                    "http://forte-devapi.azurewebsites.net/api");
        }
    }
}