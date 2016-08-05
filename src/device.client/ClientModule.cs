using System.Collections.Generic;
using AutoMapper;
using forte.devices.data;
using forte.devices.entities;
using forte.devices.models;
using forte.devices.services;
using Microsoft.Practices.Unity;
using Newtonsoft.Json;

namespace forte.devices
{
    public static class ClientModule
    {
        public static class Registrar
        {
            private static IMapper _mapper;
            private static readonly object MapperLock = new object();

            public static void RegisterDependencies(IUnityContainer container)
            {
                container.RegisterType<IDeviceRepository, DeviceRepository>(new HierarchicalLifetimeManager());
                container.RegisterType<IStreamingDevice, StreamingDevice>(new HierarchicalLifetimeManager());
            }

            public static IMapper CreateMapper()
            {
                lock (MapperLock)
                {
                    if (_mapper != null) return _mapper;
                    var config = new MapperConfiguration(ConfigureMapper);
                    _mapper = config.CreateMapper();
                    return _mapper;
                }
            }

            private static void ConfigureMapper(IMapperConfigurationExpression cfg)
            {
                //cfg.CreateMap<Audio, VmixAudio>();

                cfg.CreateMap<DeviceConfig, StreamingDeviceConfig>();
                cfg.CreateMap<StreamingDeviceConfig, DeviceConfig>();

                cfg.CreateMap<DeviceCommandEntity, DeviceCommandModel>()
                    .ForMember(entity => entity.ExecutionSucceeded,
                        map => map.MapFrom(model => model.Status == ExecutionStatus.Executed))
                    .ForMember(entity => entity.Data,
                        map => map.MapFrom(
                                model => JsonConvert.DeserializeObject<Dictionary<string, DataValue>>(model.Data)));
                cfg.CreateMap<DeviceCommandModel, DeviceCommandEntity>()
                    .ForMember(model => model.Status, map => map.UseValue(ExecutionStatus.Received))
                    .ForMember(model => model.Data,
                        map => map.MapFrom(entity => JsonConvert.SerializeObject(entity.Data)));

                //cfg.CreateMap<VmixState, StreamingClientState>()
                //    .ForMember(m => m.Software, expression => { expression.UseValue("vMix"); });
            }
        }
    }
}