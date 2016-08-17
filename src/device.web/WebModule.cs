using System.Collections.Generic;
using AutoMapper;
using device.web.models;
using device.web.server;
using forte.devices.models;
using forte.models;
using Microsoft.Practices.Unity;

namespace device.web
{
    public static class WebModule
    {
        public static class Registrar
        {
            public static void RegisterDependencies(IUnityContainer container)
            {
                UnityResolver.CreateDefault(container);
            }

            public static void RegisterMappings()
            {
                //Mapper.CreateMap<DataValue, object>()
                //    .ConvertUsing(source => source.Get());

                Mapper.CreateMap<StreamingDeviceConfig, SettingsModel>()
                    .ForMember(destination => destination.Settings,
                        config => config.MapFrom(source => source.ToDictionary(false)));
            }
        }
    }
}