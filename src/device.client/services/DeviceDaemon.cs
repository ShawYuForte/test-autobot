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

namespace forte.devices.services
{
    /// <summary>
    /// Streaming device manager, responds to WebSocket messages when there are new commands, and then for each command:
    ///  - Fetches command
    ///  - Saves locally
    ///  - Executes the command
    ///  - Saves state locally
    ///  - Saves command state on the server
    ///  - Publishes new client state
    /// </summary>
    public class DeviceDaemon : IDeviceDaemon
    {
        public delegate void MessageReceivedDelegate(string message);

        private readonly RestClient _client;
        private readonly RestClient _streamClient;
        private readonly Guid _deviceId;
        private readonly IDeviceRepository _deviceRepository;
        private readonly ILogger _logger;
        private readonly IConfigurationManager _configurationManager;
        private readonly IServerListener _serverListener;

        private readonly IStreamingClient _streamingClient;
        private static readonly object Key = new object();
        private static readonly object StopKey = new object();

        public DeviceDaemon(IDeviceRepository deviceRepository, IStreamingClient streamingClient, ILogger logger, 
            IConfigurationManager configurationManager, IServerListener serverListener)
        {
            _deviceRepository = deviceRepository;
            _streamingClient = streamingClient;
            _logger = logger;
            _configurationManager = configurationManager;
            SetDefaultSettings();
            var config = _configurationManager.GetDeviceConfig();
            _client = new RestClient($"{config.Get<string>(SettingParams.ServerApiPath)}/devices/");
            _streamClient = new RestClient($"{config.Get<string>(SettingParams.ServerApiPath)}/streams/");
            _deviceId = config.DeviceId;
            _serverListener = serverListener;
            serverListener.MessageReceived += OnServerMessageReceived;
        }

        private void OnServerMessageReceived(string message)
        {
            if (message != "CommandAvailable") return;
            FetchRequested = true;
        }

        private void SetDefaultSettings()
        {
            var config = _configurationManager.GetDeviceConfig();
            if (string.IsNullOrWhiteSpace(config.Get<string>(SettingParams.ServerRootPath)))
                config = _configurationManager.UpdateSetting(SettingParams.ServerRootPath, "http://forte-devapi.azurewebsites.net");
            if (string.IsNullOrWhiteSpace(config.Get<string>(SettingParams.ServerApiPath)))
                config = _configurationManager.UpdateSetting(SettingParams.ServerApiPath, "http://forte-devapi.azurewebsites.net/api");
            if (config.DeviceId == Guid.Empty)
                _configurationManager.UpdateSetting(nameof(config.DeviceId), Guid.Parse("602687AA-37BD-4E92-B0F8-05FEFFB4A1E0"));
        }

        public bool StartStreaming(DeviceCommandModel command)
        {
            _logger.Debug("Starting streaming for command {@command}", command);

            if (!command.Data.ContainsKey(CommonEntityParams.VideoStreamId))
                throw new Exception("Video stream id not provided, cannot start streaming");
            var videoStreamId = command.Data[CommonEntityParams.VideoStreamId].Get<Guid>();

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

        private bool Stopped { get; set; }
        private bool FetchRequested { get; set; }

        public bool StartProgram(DeviceCommandModel command)
        {
            _logger.Debug("Starting program for command {@command}", command);

            if (!command.Data.ContainsKey(CommonEntityParams.VideoStreamId))
                throw new Exception("Video stream id not provided, cannot start program");
            var videoStreamId = command.Data[CommonEntityParams.VideoStreamId].Get<Guid>();

            var state = GetState();
            if (state.ActiveVideoStreamId != videoStreamId)
            {
                _logger.Fatal("Cannot stream for program {@newVideoStream}, prepared to stream for video stream {@oldVideoStream}", videoStreamId, state.ActiveVideoStreamId);
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

        /// <summary>
        ///     Stop streaming for the specified video stream identifier. The video stream identifier is there to ensure that the
        ///     request is coming for the right stream
        /// </summary>
        /// <param name="command"></param>
        public bool StopStreaming(DeviceCommandModel command)
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
                    _streamingClient.StopStreaming(shutdownClient: true);
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

        /// <summary>
        ///     Stop streaming for the specified video stream identifier. The video stream identifier is there to ensure that the
        ///     request is coming for the right stream
        /// </summary>
        /// <param name="command"></param>
        public bool StopProgram(DeviceCommandModel command)
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


        public void FetchCommand()
        {
            lock (Key)
            {
                ThreadSafeFetchCommand();
            }
        }

        public void ThreadSafeFetchCommand()
        {
            _logger.Debug("Fetching command...");

            // TODO handle duplicates and queuing
            var request = new RestRequest($"{_deviceId}/commands/next", Method.GET)
            {
                JsonSerializer = NewtonsoftJsonSerializer.Default
            };
            var response = _client.Execute<DeviceCommandModelEx>(request);
            // Not found if no command
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                _logger.Debug("No command available, exiting");
                // ... so just exit
                return;
            }
            if (response.StatusCode != HttpStatusCode.OK)
            {
                // TODO handle logging / exceptions
                throw new Exception(response.ErrorMessage ?? response.StatusDescription);
            }

            _logger.Debug("Command retrieved {@command}", response.Data);
            var command = response.Data;

            SaveCommandLocally(command);

            try
            {
                ExecuteCommand(command);
            }
            catch (Exception exception)
            {
                command.ExecutionSucceeded = false;
                command.Status = ExecutionStatus.Failed;
                command.ExecutionMessages = exception.Message;
                command.RetryCount++;
                _logger.Error(exception, "Could not execute command {@command}", command);
            }

            SaveCommandLocally(command);

            try
            {
                SaveCommandOnServer(command);
                command.PublishedOn = DateTime.UtcNow;
            }
            catch (Exception exception)
            {
                command.ExecutionMessages = exception.Message;
            }

            SaveCommandLocally(command);

            if (command.Status == ExecutionStatus.Failed && command.RetryCount >= 3)
            {
                var deviceState = GetState();
                deviceState.Status = StreamingDeviceStatuses.Error;
                _deviceRepository.Save(deviceState);
            }

            PublishState();
        }

        private void SaveCommandLocally(DeviceCommandModel command)
        {
            var commandEntity = Mapper.Map<DeviceCommandEntity>(command);
            _deviceRepository.SaveCommand(commandEntity);
        }


        private void ExecuteCommand(DeviceCommandModelEx command)
        {
            var result = true;
            var resultMessage = string.Empty;

            switch (command.Command)
            {
                case DeviceCommands.UpdateState:
                    result = PublishState();
                    break;
                case DeviceCommands.StartStreaming:
                    result = StartStreaming(command);
                    break;
                case DeviceCommands.StopStreaming:
                    result = StopStreaming(command);
                    break;
                case DeviceCommands.StartProgram:
                    result = StartProgram(command);
                    break;
                case DeviceCommands.StopProgram:
                    result = StopProgram(command);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            command.ExecutionMessages = resultMessage;
            command.Status = result ? ExecutionStatus.Executed : ExecutionStatus.Failed;
            command.ExecutedOn = DateTime.UtcNow;
        }

        public void QueryServer()
        {
            FetchRequested = true;
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

            _logger.Debug("State published");

            return true;
        }

        public void Run()
        {
            var seconds = 10;
            _serverListener.Connect();
            while (!Stopped)
            {
                seconds--;
                if (seconds <= 0 || FetchRequested)
                {
                    FetchRequested = false;
                    seconds = 10;
                    FetchCommand();
                }
                Thread.Sleep(1000);
            }
        }

        public void Stop()
        {
            _serverListener.Disconnect();
            Stopped = true;
        }

        /// <summary>
        /// Saves command state on the server
        /// </summary>
        /// <param name="commandModel"></param>
        /// <exception cref="Exception">If server update fails</exception>
        private void SaveCommandOnServer(DeviceCommandModelEx commandModel)
        {
            _deviceRepository.SaveCommand(Mapper.Map<DeviceCommandEntity>(commandModel));

            // TODO handle duplicates and queuing
            var request = new RestRequest($"{_deviceId}/commands/{commandModel.Id}", Method.PUT)
            {
                JsonSerializer = NewtonsoftJsonSerializer.Default
            };
            request.AddHeader("Content-Type", "application/json; charset=utf-8");
            var commandPatch = new
            {
                ExecutionSucceeded = commandModel.Status == ExecutionStatus.Executed, commandModel.ExecutedOn, commandModel.ExecutionMessages
            };
            request.AddJsonBody(commandPatch);
            var response = _client.Execute<DeviceCommandModel>(request);

            if (response.StatusCode == HttpStatusCode.OK) return;

            _logger.Error("Could not update command because of {@status}, response {@resposne}", response.ErrorMessage ?? response.StatusDescription, response);
            throw new Exception(response.ErrorMessage ?? response.StatusDescription);
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

        private VideoStreamModel DownloadStreamInformation(Guid videoStreamId)
        {
            var request = new RestRequest($"{videoStreamId}?extended=true", Method.GET);
            var response = _streamClient.Execute<VideoStreamModel>(request);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                var message = response.ErrorMessage ?? response.StatusDescription;
                // TODO handle logging / exceptions
                _logger.Fatal("Could not retrieve video stream details for {@videoStreamId} due to {@message} from response {@response}", videoStreamId, message, response);
                throw new Exception($"Could not retrieve video stream details for {videoStreamId} due to {message}");
            }
            _deviceRepository.SaveVideoStream(Mapper.Map<VideoStream>(response.Data));
            return response.Data;
        }
    }
}