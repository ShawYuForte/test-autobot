using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using device.logging;
using device.web.server;
using forte.devices;
using forte.devices.services;
using Microsoft.Practices.Unity;

namespace device.web.temp
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            var container = new UnityContainer();
            UnityResolver.CreateDefault(container);
            config.DependencyResolver = UnityResolver.Default;

            WebModule.Registrar.RegisterDependencies(container);
            WebModule.Registrar.RegisterMappings();

            ClientModule.Registrar.RegisterDependencies(container);
            ClientModule.Registrar.RegisterMappings();

            var runtimeConfig = container.Resolve<IRuntimeConfig>();
            runtimeConfig.DataPath = @"c:\dev\runtime\forte\data";
            runtimeConfig.LogPath = @"c:\dev\runtime\forte\logs";

            VmixClientModule.Registrar.RegisterDependencies(container);
            VmixClientModule.Registrar.RegisterMappings();

            LoggerModule.Registrar.RegisterDependencies(container);

            CoreModule.SetDefaultSerializerSettings();

            var formatters = config.Formatters;
            var jsonFormatter = formatters.JsonFormatter;
            //jsonFormatter.SerializerSettings = CoreModule.GetSerializerSettings(jsonFormatter.SerializerSettings);
            CoreModule.SetDefaultSerializerSettings(jsonFormatter.SerializerSettings);

            // remove XML support for Web API calls
            formatters.Remove(formatters.XmlFormatter);
        }
    }
}
