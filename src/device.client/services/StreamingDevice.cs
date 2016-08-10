﻿using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using forte.devices.data;
using forte.devices.entities;
using forte.devices.extensions;
using forte.devices.models;
using Microsoft.AspNet.SignalR.Client;
using RestSharp;

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
    public class StreamingDevice : IStreamingDevice
    {
        public delegate void MessageReceivedDelegate(string message);

        private readonly RestClient _client;
        private readonly RestClient _streamClient;
        private readonly Guid _deviceId;
        private readonly IDeviceRepository _deviceRepository;
        private readonly ILogger _logger;
        private readonly IConfigurationManager _configurationManager;

        private readonly IStreamingClient _streamingClient;

        private IHubProxy _deviceInteractionHubProxy;

        private HubConnection _hubConnection;
        private Timer _timer;
        private static readonly object Key = new object();

        public StreamingDevice(IDeviceRepository deviceRepository, IStreamingClient streamingClient, ILogger logger, IConfigurationManager configurationManager)
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
        }

        private void SetDefaultSettings()
        {
            var config = _configurationManager.GetDeviceConfig();
            if (string.IsNullOrWhiteSpace(config.Get<string>(SettingParams.ServerApiPath)))
                config = _configurationManager.UpdateSetting(SettingParams.ServerApiPath, "http://dev-api.forte.fit/api");
            if (config.DeviceId == Guid.Empty)
                config = _configurationManager.UpdateSetting(nameof(config.DeviceId), Guid.Parse("602687AA-37BD-4E92-B0F8-05FEFFB4A1E0"));
        }

        public bool StartStreaming(DeviceCommandModel command)
        {
            _logger.Debug("Starting streaming for command {@command}", command);

            if (!command.Data.ContainsKey(CommonEntityParams.VideoStreamId))
                throw new Exception("Video stream id not provided, cannot start streaming");
            var videoStreamId = command.Data[CommonEntityParams.VideoStreamId].Get<Guid>();

            var state = _deviceRepository.GetDeviceState();
            if (state.ActiveVideoStreamId != videoStreamId)
            {
                _logger.Fatal(
                    "Cannot stream for video stream {@newVideoStream}, prepared to stream for video stream {@oldVideoStream}",
                    videoStreamId, state.ActiveVideoStreamId);
                throw new Exception("Cannot stream, device prepared to stream for a different stream");
            }
            _deviceRepository.Save(state);

            _streamingClient.StartBroadcast();
            FetchDeviceAndClientState();

            _logger.Debug("Streaming started.");

            return true;
        }

        /// <summary>
        ///     Start streaming for the specified video stream identifier
        /// </summary>
        /// <param name="command"></param>
        public bool PrepareForStream(DeviceCommandModel command)
        {
            _logger.Debug("Preparing to stream based on command {@command}", command);

            if (!command.Data.ContainsKey(CommonEntityParams.VideoStreamId))
                throw new Exception("Video stream id not provided, cannot prepare for streaming");
            var videoStreamId = command.Data[CommonEntityParams.VideoStreamId].Get<Guid>();

            var videoStream = DownloadStreamInformation(videoStreamId);
            if (videoStream == null)
                throw new Exception("Video stream could not be downloaded, cannot prepare for streaming");

            _streamingClient.LoadVideoStreamPreset(videoStream);
            FetchDeviceAndClientState();
            var state = _deviceRepository.GetDeviceState();
            state.ActiveVideoStreamId = videoStream.Id;
            state.Status = StreamingDeviceStatuses.ReadyToStream;
            _deviceRepository.Save(state);

            _logger.Debug("Device is ready to stream.");

            return true;

            // TODO
            // - Make sure streaming client is idle (report if not)
        }

        /// <summary>
        ///     Stop streaming for the specified video stream identifier. The video stream identifier is there to ensure that the
        ///     request is coming for the right stream
        /// </summary>
        /// <param name="command"></param>
        public bool StopStreaming(DeviceCommandModel command)
        {
            _logger.Debug("Stopping streaming for command {@command}", command);

            var state = _deviceRepository.GetDeviceState();
            state.ActiveVideoStreamId = null;
            _deviceRepository.Save(state);

            _streamingClient.EndBroadcast(true);
            FetchDeviceAndClientState();

            _logger.Debug("Streaming stopped.");

            return true;
        }

         public StreamingDeviceConfig GetConfig()
        {
            var deviceConfig = _deviceRepository.GetDeviceConfig();
            return ClientModule.Registrar.CreateMapper().Map<StreamingDeviceConfig>(deviceConfig);
        }

        public event services.MessageReceivedDelegate MessageReceived;

        public void Connect()
        {
            try
            {
                ConnectAsync().Wait();
            }
            catch (Exception exception)
            {
                _logger.Error(exception, "Could not connect to hub");
                OnHubConnectionOnClosed();
            }
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
            var response = _client.Execute<DeviceCommandModel>(request);
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
                var deviceState = _deviceRepository.GetDeviceState();
                deviceState.Status = StreamingDeviceStatuses.Error;
                _deviceRepository.Save(deviceState);
            }

            PublishState();
        }

        private void SaveCommandLocally(DeviceCommandModel command)
        {
            var commandEntity = ClientModule.Registrar.CreateMapper().Map<DeviceCommandEntity>(command);
            _deviceRepository.SaveCommand(commandEntity);
        }

        public void Disconnect()
        {
            //_cancellationTokenSource.Cancel();
            _hubConnection.Dispose();
        }

        public async Task Send(string message)
        {
            await _deviceInteractionHubProxy.Invoke("SendHello", message);
            await _deviceInteractionHubProxy.Invoke("RequestState", Guid.Parse("602687aa-37bd-4e92-b0f8-05feffb4a1e0"));
        }

        private async Task ConnectAsync()
        {
            if (_hubConnection != null)
            {
                await _hubConnection.Start();
                return;
            }

            var serverUrl = System.Configuration.ConfigurationManager.AppSettings["server:url"];
            _hubConnection = new HubConnection(serverUrl);
            _deviceInteractionHubProxy = _hubConnection.CreateHubProxy("DeviceInteractionHub");
            _deviceInteractionHubProxy.On("CommandAvailable", deviceId =>
            {
                // If not our event, ignore
                if (Guid.Parse(deviceId) != _deviceId) return;
                _logger.Debug($"Server notified us of a command available for device {deviceId}");
                FetchCommand();
            });

            _hubConnection.Closed += OnHubConnectionOnClosed;
            _hubConnection.ConnectionSlow += () => _logger.Warning("Connection slow... might close!");
            _hubConnection.Error += exception => _logger.Error($"Connection error: {exception.Message}");
            _hubConnection.Reconnected += () => _logger.Debug($"Connection re-established");
            _hubConnection.Reconnecting += () => _logger.Debug($"Re-connecting...");
            _hubConnection.StateChanged +=
                state => _logger.Warning($"Connection state changed from {state.OldState} to {state.NewState}");
            _hubConnection.Received += data => _logger.Debug($"Received {data}");
            await _hubConnection.Start();
        }

        private void OnHubConnectionOnClosed()
        {
            _logger.Debug("Connection closed, will retry in 10 seconds!");
            _timer = new Timer(state =>
            {
                _logger.Debug("Attempting to re-connect");
                Connect();
                _timer.Dispose();
            }, null, TimeSpan.FromSeconds(10), TimeSpan.FromSeconds(1));
        }

        private void ExecuteCommand(DeviceCommandModel command)
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
                case DeviceCommands.PrepareForStream:
                    result = PrepareForStream(command);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            command.ExecutionMessages = resultMessage;
            command.Status = result ? ExecutionStatus.Executed : ExecutionStatus.Failed;
            command.ExecutedOn = DateTime.UtcNow;
        }

        /// <summary>
        ///     Publish device state to the server
        /// </summary>
        public bool PublishState()
        {
            _logger.Debug("Publishing state");

            var deviceState = FetchDeviceAndClientState();
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

        /// <summary>
        /// Saves command state on the server
        /// </summary>
        /// <param name="commandEntity"></param>
        /// <exception cref="Exception">If server update fails</exception>
        private void SaveCommandOnServer(DeviceCommandModel commandEntity)
        {
            var mapper = ClientModule.Registrar.CreateMapper();
            _deviceRepository.SaveCommand(mapper.Map<DeviceCommandEntity>(commandEntity));

            // TODO handle duplicates and queuing
            var request = new RestRequest($"{_deviceId}/commands/{commandEntity.Id}", Method.PUT)
            {
                JsonSerializer = NewtonsoftJsonSerializer.Default
            };
            request.AddHeader("Content-Type", "application/json; charset=utf-8");
            var commandPatch = new 
            {
                ExecutionSucceeded = commandEntity.Status == ExecutionStatus.Executed,
                commandEntity.ExecutedOn,
                commandEntity.ExecutionMessages
            };
            request.AddJsonBody(commandPatch);
            var response = _client.Execute<DeviceCommandModel>(request);

            if (response.StatusCode == HttpStatusCode.OK) return;

            _logger.Error("Could not update command because of {@status}, response {@resposne}",
                response.ErrorMessage ?? response.StatusDescription, response);
            throw new Exception(response.ErrorMessage ?? response.StatusDescription);
        }

        public StreamingDeviceState FetchDeviceAndClientState()
        {
            var clientState = _streamingClient.GetState();
            var deviceState = _deviceRepository.GetDeviceState();
            deviceState.Status = StreamingDeviceStatuses.Idle;

            if (clientState != null)
            {
                if (clientState.Recording && clientState.Streaming) 
                    deviceState.Status = StreamingDeviceStatuses.StreamingAndRecording;
                else if (clientState.Recording)
                    deviceState.Status = StreamingDeviceStatuses.Recording;
                else if (clientState.Streaming)
                    deviceState.Status = StreamingDeviceStatuses.Streaming;
            }
            deviceState.StateCapturedOn = DateTime.UtcNow;
            _deviceRepository.Save(deviceState);
            return GetState();
        }

        private StreamingDeviceState GetState()
        {
            return _deviceRepository.GetDeviceState();
        }

        private VideoStreamModel DownloadStreamInformation(Guid videoStreamId)
        {
            var request = new RestRequest($"{videoStreamId}?extended=true", Method.GET);
            var response = _streamClient.Execute<VideoStreamModel>(request);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                var message = response.ErrorMessage ?? response.StatusDescription;
                // TODO handle logging / exceptions
                _logger.Fatal("Could not retrieve video stream details for {@videoStreamId} due to {@message} from response {@response}",
                    videoStreamId, message, response);
                throw new Exception($"Could not retrieve video stream details for {videoStreamId} due to {message}");
            }
            var mapper = ClientModule.Registrar.CreateMapper();
            _deviceRepository.SaveVideoStream(mapper.Map<VideoStream>(response.Data));
            return response.Data;
        }

        private void OnMessageReceived(string message)
        {
            MessageReceived?.Invoke(message);
        }
    }
}