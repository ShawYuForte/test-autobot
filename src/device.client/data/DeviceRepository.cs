using System;
using System.Data.Entity;
using System.Linq;
using forte.devices.entities;
using forte.devices.models;
using Settings = forte.devices.entities.Settings;

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

        public StreamingDeviceState GetDeviceState()
        {
            using (var dbContext = new DeviceDbContext())
            {
                return dbContext.DeviceState.FirstOrDefault() ?? new StreamingDeviceState();
            }
        }

        public void Save(StreamingDeviceState deviceState)
        {
            using (var dbContext = new DeviceDbContext())
            {
                if (deviceState.DeviceId == Guid.Empty)
                    deviceState.DeviceId = Guid.Parse("602687AA-37BD-4E92-B0F8-05FEFFB4A1E0");
                deviceState.StateCapturedOn = DateTime.UtcNow;

                var existing = dbContext.DeviceState.Count();
                if (existing > 0)
                {
                    dbContext.DeviceState.Attach(deviceState);
                    dbContext.Entry(deviceState).State = EntityState.Modified;
                }
                else
                {
                    dbContext.DeviceState.Add(deviceState);
                }

                dbContext.SaveChanges();
            }
        }
    }
}