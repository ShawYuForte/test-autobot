using AutoMapper;
using forte.devices.models;
using forte.devices.services;
using forte.devices.services.clients;
using Microsoft.Practices.Unity;

namespace forte.devices
{
    public static class VmixClientModule
    {
        public static class Registrar
        {
            private static readonly object MapperLock = new object();

            public static void RegisterDependencies(IUnityContainer container)
            {
                container.RegisterType<IStreamingClient, VmixStreamingClient>(new HierarchicalLifetimeManager());
            }

            public static void RegisterMappings()
            {
                Mapper.CreateMap<Audio, VmixAudio>();
                Mapper.CreateMap<VmixState, StreamingClientState>().ForMember(m => m.Software, expression => { expression.UseValue("vMix"); });
            }
        }
    }
}