using System;
using System.Configuration;
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
    public class DeviceManager : IDeviceManager
    {
        public delegate void MessageReceivedDelegate(string message);

        private readonly RestClient _client;
        private readonly IDeviceRepository _deviceRepository;

        private readonly IStreamingClient _streamingClient;
        private readonly Guid _deviceId;

        private HubConnection _hubConnection;

        private IHubProxy _stockTickerHubProxy;

        public DeviceManager(IDeviceRepository deviceRepository, IStreamingClient streamingClient)
        {
            _deviceRepository = deviceRepository;
            _streamingClient = streamingClient;
            var settings = _deviceRepository.GetSettings();
            _client = new RestClient(settings.ApiPath);
            _deviceId = GetDeviceConfig().DeviceId;
        }

        /// <summary>
        ///     Publish device state to the server
        /// </summary>
        public void PublishState()
        {
            var deviceState = FetchDeviceAndClientState();
            var request = new RestRequest($"{_deviceId}/state", Method.POST);
            request.AddBody(deviceState);
            var response = _client.Execute(request);

            if (response.ResponseStatus != ResponseStatus.Completed)
            {
                // Log error
            }
        }

        /// <summary>
        ///     Start streaming for the specified video stream identifier
        /// </summary>
        /// <param name="videoStreamId"></param>
        public void StartStreaming(Guid videoStreamId)
        {
            var videoStream = DownloadStreamInformation(videoStreamId);
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
            var serverUrl = ConfigurationManager.AppSettings["server:url"];
            OnMessageReceived($"Connecting to {serverUrl}");
            _hubConnection = new HubConnection(serverUrl);
            _stockTickerHubProxy = _hubConnection.CreateHubProxy("DeviceInteractionHub");
            _stockTickerHubProxy.On("Hello", message =>
            {
                OnMessageReceived($"Server said {message}");
                PublishState();
            });
            _stockTickerHubProxy.On("RequestState", deviceId =>
            {
                OnMessageReceived($"Server requested state for device id {deviceId}");
                PublishState();
            });
            _hubConnection.Start().Wait();
        }

        public void Disconnect()
        {
            //_cancellationTokenSource.Cancel();
            _hubConnection.Dispose();
        }

        public async Task Send(string message)
        {
            await _stockTickerHubProxy.Invoke("SendHello", message);
            await _stockTickerHubProxy.Invoke("RequestState", Guid.Parse("602687aa-37bd-4e92-b0f8-05feffb4a1e0"));
        }

        private StreamingDeviceConfig GetDeviceConfig()
        {
            return
                ClientModule.Registrar.CreateMapper().Map<StreamingDeviceConfig>(_deviceRepository.GetDeviceConfig()) ??
                new StreamingDeviceConfig();
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