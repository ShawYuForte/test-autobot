using System;
using System.Configuration;
using System.Threading.Tasks;
using AutoMapper;
using forte.devices.data;
using forte.devices.entities;
using forte.devices.models;
using Microsoft.AspNet.SignalR.Client;
using RestSharp;
using DeviceConfig = forte.devices.models.DeviceConfig;
using Settings = forte.devices.models.Settings;

namespace forte.devices.services
{
    public class DeviceManager : IDeviceManager
    {
        public delegate void MessageReceivedDelegate(string message);

        private readonly RestClient _client;
        private readonly IDeviceRepository _deviceRepository;

        private readonly IDeviceManager _deviceManager;
        private readonly IStreamingClient _streamingClient;

        private HubConnection _hubConnection;

        private IHubProxy _stockTickerHubProxy;

        public DeviceManager(IDeviceRepository deviceRepository, IStreamingClient streamingClient)
        {
            _deviceRepository = deviceRepository;
            _streamingClient = streamingClient;
            var settings = _deviceRepository.GetSettings();
            _client = new RestClient(settings.ApiPath);
        }

        /// <summary>
        ///     Publish device state to the server
        /// </summary>
        public void PublishState()
        {
            var clientState = _streamingClient.GetState();
            var request = new RestRequest("", Method.POST);
            request.AddBody(clientState);
            var response = _client.Execute(request);

            // TODO
            // 1. Get latest from streaming client
            // 2. Post to client
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

        public DeviceConfig GetConfig()
        {
            var deviceConfig = _deviceRepository.GetDeviceConfig();
            return ClientModule.Registrar.CreateMapper().Map<DeviceConfig>(deviceConfig);
        }

        public event services.MessageReceivedDelegate MessageReceived;

        public async Task Connect()
        {
            _hubConnection = new HubConnection(ConfigurationManager.AppSettings["server:url"]);
            _stockTickerHubProxy = _hubConnection.CreateHubProxy("DeviceInteractionHub");
            _stockTickerHubProxy.On("Hello", message => { OnMessageReceived($"Server said {message}"); });
            _stockTickerHubProxy.On("RequestState", deviceId =>
            {
                OnMessageReceived($"Server requested state for device id {deviceId}");
                _deviceManager.PublishState();
            });
            await _hubConnection.Start();
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