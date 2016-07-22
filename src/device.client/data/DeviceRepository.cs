using System;
using System.Linq;
using forte.devices.entities;

namespace forte.devices.data
{
    public class DeviceRepository : IDeviceRepository
    {
        public Settings GetSettings()
        {
            return new Settings { ApiPath = "http://dev-api.forte.fit/api"};
        }

        public DeviceConfig GetDeviceConfig()
        {
            using (var dbContext = new DeviceDbContext())
            {
                return dbContext.DeviceConfig.FirstOrDefault();
            }
        }

        public Settings SaveSettings(Settings settings)
        {
            throw new NotImplementedException();
        }

        public VideoStream GetVideoStream(Guid videoStreamId)
        {
            throw new NotImplementedException();
        }

        public VideoStream SaveVideoStream(VideoStream videoStream)
        {
            throw new NotImplementedException();
        }
    }
}