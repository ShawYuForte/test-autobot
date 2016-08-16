using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using AutoMapper;
using forte.devices.entities;
using forte.devices.models;
using forte.devices.services;
using forte.models;

namespace forte.devices.data
{
    public class DeviceRepository : IDeviceRepository
    {
        private readonly string _connectionString;

        public DeviceRepository(IRuntimeConfig runtimeConfig)
        {
            if (!Directory.Exists(runtimeConfig.DataPath))
                Directory.CreateDirectory(runtimeConfig.DataPath);

            var dbFilePath = Path.Combine(runtimeConfig.DataPath, "device-db.sdf");

            _connectionString = $"Data Source={dbFilePath}";
        }

        public List<DeviceSetting> GetSettings()
        {
            using (var dbContext = new DeviceDbContext(_connectionString))
            {
                return dbContext.Settings.ToList();
            }
        }

        public DeviceConfig GetDeviceConfig()
        {
            using (var dbContext = new DeviceDbContext(_connectionString))
            {
                return dbContext.DeviceConfig.FirstOrDefault() ?? new DeviceConfig
                {
                    DeviceId = Guid.Parse("602687AA-37BD-4E92-B0F8-05FEFFB4A1E0")
                };
            }
        }

        public List<DeviceSetting> SaveSettings(List<DeviceSetting> settings)
        {
            using (var dbContext = new DeviceDbContext(_connectionString))
            {
                var savedSettings = GetSettings();

                foreach (var setting in savedSettings)
                {
                    if (settings.All(s => s.Name != setting.Name))
                        dbContext.Settings.Remove(setting);
                }

                foreach (var setting in settings)
                {
                    var existing = savedSettings.FirstOrDefault(s => s.Name == setting.Name);
                    if (existing != null)
                    {
                        setting.Id = existing.Id;
                        setting.Version = existing.Version;
                        dbContext.Settings.Attach(setting);
                        dbContext.Entry(setting).State = EntityState.Modified;
                    }
                    else
                    {
                        setting.Id = Guid.NewGuid();
                        setting.Created = DateTime.UtcNow;
                        dbContext.Settings.Add(setting);
                    }
                }

                dbContext.SaveChanges();

                return dbContext.Settings.ToList();
            }
        }

        public VideoStream GetVideoStream(Guid videoStreamId)
        {
            return null;
        }

        public VideoStream SaveVideoStream(VideoStream videoStream)
        {
            return videoStream;
        }

        public StreamingDeviceState GetDeviceState()
        {
            using (var dbContext = new DeviceDbContext(_connectionString))
            {
                return dbContext.DeviceState.FirstOrDefault();
            }
        }

        public void Save(StreamingDeviceState deviceState)
        {
            using (var dbContext = new DeviceDbContext(_connectionString))
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

        public DeviceCommandEntity SaveCommand(DeviceCommandEntity deviceCommandEntity)
        {
            using (var dbContext = new DeviceDbContext(_connectionString))
            {
                if (deviceCommandEntity.Created == DateTime.MinValue) deviceCommandEntity.Created = DateTime.UtcNow;
                if (deviceCommandEntity.LastModified == DateTime.MinValue) deviceCommandEntity.LastModified = DateTime.UtcNow;

                if (dbContext.Commands.Count(command => command.Id == deviceCommandEntity.Id) > 0)
                {
                    dbContext.Commands.Attach(deviceCommandEntity);
                    dbContext.Entry(deviceCommandEntity).State = EntityState.Modified;
                }
                else
                {
                    dbContext.Commands.Add(deviceCommandEntity);
                }

                dbContext.SaveChanges();

                return deviceCommandEntity;
            };
        }

        public void SaveSetting<T>(string setting, T value)
        {
            DeviceSetting existing;

            using (var dbContext = new DeviceDbContext(_connectionString))
            {
                // ... want it deached, hence calling separately
                existing = dbContext.Settings.FirstOrDefault(s => s.Name == setting);
            }

            using (var dbContext = new DeviceDbContext(_connectionString))
            {
                var newSetting = Mapper.Map<DeviceSetting>(new DataValue(value));
                newSetting.Name = setting;
                newSetting.LastModified = DateTime.UtcNow;

                if (existing != null)
                {
                    newSetting.Id = existing.Id;
                    newSetting.Version = existing.Version;
                    newSetting.Created = existing.Created;
                    dbContext.Settings.Attach(newSetting);
                    dbContext.Entry(newSetting).State = EntityState.Modified;
                }
                else
                {
                    newSetting.Id = Guid.NewGuid();
                    newSetting.Created = DateTime.UtcNow;
                    dbContext.Settings.Add(newSetting);
                }

                dbContext.SaveChanges();
            }
        }
    }
}