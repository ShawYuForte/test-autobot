using System;
using System.Configuration;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using forte.devices.data;
using forte.devices.entities;
using forte.devices.models;
using Microsoft.AspNet.SignalR.Client;
using RestSharp;
using Settings = forte.devices.models.Settings;

namespace forte.devices.services
{
    public class StreamingDevice : IStreamingDevice
    {
        public delegate void MessageReceivedDelegate(string message);

        private readonly RestClient _client;
        private readonly IDeviceRepository _deviceRepository;

        private readonly IStreamingClient _streamingClient;
        private readonly Guid _deviceId;
        private readonly ILogger _logger;

        private HubConnection _hubConnection;

        private IHubProxy _deviceInteractionHubProxy;

        public StreamingDevice(IDeviceRepository deviceRepository, IStreamingClient streamingClient, ILogger logger)
        {
            _deviceRepository = deviceRepository;
            _streamingClient = streamingClient;
            _logger = logger;
            var settings = _deviceRepository.GetSettings();
            _client = new RestClient($"{settings.ApiPath}/devices/");
            _deviceId = GetDeviceConfig().DeviceId;
        }

        /// <summary>
        ///     Publish device state to the server
        /// </summary>
        public bool PublishState()
        {
            _logger.Debug("Publishing state");

            var deviceState = FetchDeviceAndClientState();
            var request = new RestRequest($"{deviceState.DeviceId}/state", Method.POST);
            request.AddHeader("Content-Type", "application/json; charset=utf-8");

            deviceState.Streaming = true;
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
        ///     Start streaming for the specified video stream identifier
        /// </summary>
        /// <param name="videoStreamId"></param>
        public void StartStreaming(Guid videoStreamId)
        {
            var state = _deviceRepository.GetDeviceState();
            state.Streaming = true;
            state.ActiveVideoStreamId = videoStreamId;
            _deviceRepository.Save(state);
            //PublishState();
            //var videoStream = DownloadStreamInformation(videoStreamId);
            // TODO
            // 1. Generate preset (using video stream + other things)
            // 2. Make sure streaming client is idle (report if not)
            // 3. Load client preset
            // 4. Start stream
            // 5. Report new state to server
        }

        /// <summary>
        ///     Stop streaming for the specified video stream identifier. The video stream identifier is there to ensure that the
        ///     request is coming for the right stream
        /// </summary>
        /// <param name="videoStreamId"></param>
        public void StopStreaming(Guid videoStreamId)
        {
            throw new NotImplementedException();
            // TODO
            // 1. Stop client streaming
            // 2. Unload client
            // 3. Report new state to server
        }

        public Settings GetSettings()
        {
            var settings = _deviceRepository.GetSettings();
            return ClientModule.Registrar.CreateMapper().Map<Settings>(settings);
        }

        public StreamingDeviceConfig GetConfig()
        {
            var deviceConfig = _deviceRepository.GetDeviceConfig();
            return ClientModule.Registrar.CreateMapper().Map<StreamingDeviceConfig>(deviceConfig);
        }

        public event services.MessageReceivedDelegate MessageReceived;

        public void Connect()
        {
            ConnectAsync().Wait();
        }

        private async Task ConnectAsync()
        {
            var serverUrl = ConfigurationManager.AppSettings["server:url"];
            _hubConnection = new HubConnection(serverUrl);
            _deviceInteractionHubProxy = _hubConnection.CreateHubProxy("DeviceInteractionHub");
            _deviceInteractionHubProxy.On("command-available", deviceId =>
            {
                // If not our event, ignore
                if (Guid.Parse(deviceId) != _deviceId) return;
                OnMessageReceived($"Server notified us of a command available for device {deviceId}");
                FetchCommand();
            });

            _hubConnection.Closed += () => _logger.Debug("Connection closed!");
            _hubConnection.ConnectionSlow += () => _logger.Warning("Connection slow... might close!");
            _hubConnection.Error += (exception) => _logger.Error($"Connection error: {exception.Message}");
            _hubConnection.Reconnected += () => _logger.Debug($"Connection re-established");
            _hubConnection.Reconnecting += () => _logger.Debug($"Re-connecting...");
            _hubConnection.StateChanged += (state) => _logger.Warning($"Connection state changed from {state.OldState} to {state.NewState}");
            _hubConnection.Received += (data) => _logger.Debug($"Received {data}");
            await _hubConnection.Start();
        }

        public void FetchCommand()
        {
            // TODO handle duplicates and queuing
            var request = new RestRequest($"{_deviceId}/commands/next", Method.GET);
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
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<DeviceCommandEntity, DeviceCommandModel>()
                    .ForMember(cmd => cmd.ExecutionSucceeded, map => map.MapFrom(other => other.Status == ExecutionStatus.Executed));
                cfg.CreateMap<DeviceCommandModel, DeviceCommandEntity>()
                    .ForMember(cmd => cmd.Status, map => map.UseValue(ExecutionStatus.Received));
            });
            var mapper = config.CreateMapper();
            var commandEntity = mapper.Map<DeviceCommandEntity>(response.Data);
            _deviceRepository.SaveCommand(commandEntity);
            ExecuteCommand(commandEntity);
        }

        private void ExecuteCommand(DeviceCommandEntity commandEntity)
        {
            var result = true;
            var resultMessage = string.Empty;

            try
            {
                switch (commandEntity.Command)
                {
                    case DeviceCommands.UpdateState:
                        result = PublishState();
                        break;
                    case DeviceCommands.StartStreaming:
                        break;
                    case DeviceCommands.StopStreaming:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            catch (Exception exception)
            {
                resultMessage = exception.Message;
                result = false;
            }

            commandEntity.ExecutionMessages = resultMessage;
            commandEntity.Status = result ? ExecutionStatus.Executed : ExecutionStatus.Failed;
            commandEntity.ExecutedOn = DateTime.UtcNow;

            UpdateCommand(commandEntity);
        }

        private void UpdateCommand(DeviceCommandEntity commandEntity)
        {
            _deviceRepository.SaveCommand(commandEntity);

            // TODO handle duplicates and queuing
            var request = new RestRequest($"{_deviceId}/commands/{commandEntity.Id}", Method.PUT);
            var response = _client.Execute<DeviceCommandModel>(request);
            request.AddHeader("Content-Type", "application/json; charset=utf-8");
            var commandPatch = new 
            {
                ExecutionSucceeded = commandEntity.Status == ExecutionStatus.Executed,
                commandEntity.ExecutedOn,
                commandEntity.ExecutionMessages
            };
            request.AddJsonBody(commandPatch);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                // TODO handle logging / exceptions
                _logger.Error("Could not update command because of {@status}", response.ErrorMessage ?? response.StatusDescription);
                return;
            }

            if (commandEntity.Status != ExecutionStatus.Executed) return;

            commandEntity.PublishedOn = DateTime.UtcNow;
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

        private StreamingDeviceConfig GetDeviceConfig()
        {
            return ClientModule.Registrar.CreateMapper().Map<StreamingDeviceConfig>(_deviceRepository.GetDeviceConfig()) ?? new StreamingDeviceConfig();
        }

        public StreamingDeviceState FetchDeviceAndClientState()
        {
            var clientState = _streamingClient.GetState();
            var deviceState = _deviceRepository.GetDeviceState();
            deviceState.Recording = clientState?.Recording ?? false;
            deviceState.StateCapturedOn = DateTime.UtcNow;
            deviceState.Streaming = clientState?.Streaming ?? false;
            _deviceRepository.Save(deviceState);
            return GetState();
        }

        private StreamingDeviceState GetState()
        {
            return _deviceRepository.GetDeviceState();
        }

        private VideoStreamModel DownloadStreamInformation(Guid videoStreamId)
        {
            var request = new RestRequest($"streams/{videoStreamId}", Method.GET)
            {
                Timeout = 1
            };
            var response = _client.Execute<VideoStreamModel>(request);
            if (response.ResponseStatus != ResponseStatus.Completed)
            {
                // TODO handle logging / exceptions
                return null;
            }
            var config = new MapperConfiguration(cfg => { cfg.CreateMap<VideoStream, VideoStreamModel>(); });
            var mapper = config.CreateMapper();
            _deviceRepository.SaveVideoStream(mapper.Map<VideoStream>(response.Data));
            return response.Data;
        }

        private void OnMessageReceived(string message)
        {
            MessageReceived?.Invoke(message);
        }
    }
}