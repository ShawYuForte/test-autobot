using System;
using AutoMapper;
using forte.devices.data;
using forte.devices.entities;
using forte.devices.models;
using RestSharp;

namespace forte.devices.services
{
    public class DeviceManager : IDeviceManager
    {
        private readonly RestClient _client;
        private readonly IDeviceRepository _deviceRepository;

        public DeviceManager(IDeviceRepository deviceRepository)
        {
            _deviceRepository = deviceRepository;
            var settings = _deviceRepository.GetSettings();
            _client = new RestClient(settings.ApiPath);
        }

        /// <summary>
        ///     Publish device state to the server
        /// </summary>
        public void PublishState()
        {
            throw new NotImplementedException();
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
    }
}