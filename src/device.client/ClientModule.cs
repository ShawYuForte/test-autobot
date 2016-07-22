using AutoMapper;
using forte.devices.data;
using forte.devices.services;
using Microsoft.Practices.Unity;

namespace forte.devices
{
    public static class ClientModule
    {
        public static class Registrar
        {
            private static IMapper _mapper;

            public static void RegisterDependencies(IUnityContainer container)
            {
                container.RegisterType<IDeviceRepository, DeviceRepository>(new HierarchicalLifetimeManager());
                container.RegisterType<IDeviceManager, DeviceManager>(new HierarchicalLifetimeManager());
            }

            public static IMapper CreateMapper()
            {
                lock (_mapper)
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

                //cfg.CreateMap<VmixState, StreamingClientState>()
                //    .ForMember(m => m.Software, expression => { expression.UseValue("vMix"); });
            }
        }
    }
}