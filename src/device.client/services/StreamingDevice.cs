using System;
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
            var config = _configurationManager.GetDeviceConfig();
            _client = new RestClient($"{config.Get<string>("ApiPath")}/devices/");
            _deviceId = config.DeviceId;
        }

        /// <summary>
        ///     Start streaming for the specified video stream identifier
        /// </summary>
        /// <param name="command"></param>
        public bool StartStreaming(DeviceCommandModel command)
        {
            _logger.Debug("Starting streaming for command {@command}", command);

            if (!command.Data.ContainsKey(CommonEntityParams.VideoStreamId))
                throw new Exception("Video stream id not provided, cannot start streaming");
            var videoStreamId = command.Data[CommonEntityParams.VideoStreamId].Get<Guid>();

            var videoStream = DownloadStreamInformation(videoStreamId);
            if (videoStream == null) return false;



            var state = _deviceRepository.GetDeviceState();
            state.Streaming = true;
            state.ActiveVideoStreamId = videoStreamId;
            _deviceRepository.Save(state);

            _logger.Debug("Streaming started.");

            return true;
            //PublishState();
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
        /// <param name="command"></param>
        public bool StopStreaming(DeviceCommandModel command)
        {
            _logger.Debug("Stopping streaming for command {@command}", command);

            var state = _deviceRepository.GetDeviceState();
            state.Streaming = false;
            state.ActiveVideoStreamId = null;
            _deviceRepository.Save(state);

            _logger.Debug("Streaming stopped.");

            return true;
            // TODO
            // 1. Stop client streaming
            // 2. Unload client
            // 3. Report new state to server
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

            try
            {
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
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            catch (Exception exception)
            {
                resultMessage = exception.Message;
                result = false;
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
            var request = new RestRequest($"streams/{videoStreamId}?extended=true", Method.GET)
            {
                Timeout = 1
            };
            var response = _client.Execute<VideoStreamModel>(request);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                var message = response.ErrorMessage ?? response.StatusDescription;
                // TODO handle logging / exceptions
                _logger.Fatal("Could not retrieve video stream details for {@videoStreamId} due to {@message} from response {@response}",
                    videoStreamId, message, response);
                throw new Exception($"Could not retrieve video stream details for {videoStreamId} due to {message}");
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