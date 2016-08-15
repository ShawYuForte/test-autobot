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
            private static readonly object MapperLock = new object();

            public static void RegisterDependencies(IUnityContainer container)
            {
                //container.RegisterType<ILogger, SeriLogger>(new HierarchicalLifetimeManager());
            }

            public static void RegisterMappings()
            {
                //cfg.CreateMap<Audio, VmixAudio>();

                //cfg.CreateMap<VmixState, StreamingClientState>()
                //    .ForMember(m => m.Software, expression => { expression.UseValue("vMix"); });
            }
        }
    }
}
