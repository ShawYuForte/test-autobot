using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using forte.devices.services;
using Microsoft.Practices.Unity;

namespace forte.devices
{
    public static class ServiceModule
    {
        public static class Registrar
        {
            private static IMapper _mapper;
            private static readonly object MapperLock = new object();

            public static void RegisterDependencies(IUnityContainer container)
            {
                container.RegisterType<ILogger, SeriLogger>(new HierarchicalLifetimeManager());
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

                //cfg.CreateMap<VmixState, StreamingClientState>()
                //    .ForMember(m => m.Software, expression => { expression.UseValue("vMix"); });
            }
        }
    }
}
