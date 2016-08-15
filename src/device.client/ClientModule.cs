using System.Collections.Generic;
using AutoMapper;
using forte.devices.data;
using forte.devices.entities;
using forte.devices.models;
using forte.devices.services;
using forte.models;
using forte.models.devices;
using Microsoft.Practices.Unity;
using Newtonsoft.Json;
using StreamingDeviceConfig = forte.devices.models.StreamingDeviceConfig;

namespace forte.devices
{
    public static class ClientModule
    {
        public static class Registrar
        {
            private static readonly object MapperLock = new object();

            public static void RegisterDependencies(IUnityContainer container)
            {
                container.RegisterType<IDeviceRepository, DeviceRepository>(new HierarchicalLifetimeManager());
                container.RegisterType<IStreamingDevice, StreamingDevice>(new HierarchicalLifetimeManager());
                container.RegisterType<IConfigurationManager, ConfigurationManager>(new HierarchicalLifetimeManager());
            }

            public static void RegisterMappings()
            {
                //cfg.CreateMap<Audio, VmixAudio>();

                Mapper.CreateMap<DeviceConfig, StreamingDeviceConfig>();
                Mapper.CreateMap<StreamingDeviceConfig, DeviceConfig>();

                Mapper.CreateMap<DeviceCommandEntity, DeviceCommandModel>()
                    .ForMember(entity => entity.ExecutionSucceeded,
                        map => map.MapFrom(model => model.Status == ExecutionStatus.Executed))
                    .ForMember(entity => entity.Data,
                        map => map.MapFrom(
                                model => JsonConvert.DeserializeObject<Dictionary<string, DataValue>>(model.Data)));
                Mapper.CreateMap<DeviceCommandModel, DeviceCommandEntity>()
                    .ForMember(model => model.Status, map => map.UseValue(ExecutionStatus.Received))
                    .ForMember(model => model.Data,
                        map => map.MapFrom(entity => JsonConvert.SerializeObject(entity.Data)));

                Mapper.CreateMap<DataValue, DeviceSetting>();
                Mapper.CreateMap<DeviceSetting, DataValue>();

                Mapper.CreateMap<VideoStreamModel, VideoStream>();
                Mapper.CreateMap<VideoStream, VideoStreamModel>();
            }
        }
    }
}