#region

using System.Web.Http;
using device.logging;
using forte.devices;
using Microsoft.Owin;
using Microsoft.Owin.FileSystems;
using Microsoft.Owin.StaticFiles;
using Microsoft.Practices.Unity;
using Owin;

#endregion

namespace device.client.web.server
{
    public class Startup
    {
        // This code configures Web API. The Startup class is specified as a type
        // parameter in the WebApp.Start method.
        public void Configuration(IAppBuilder appBuilder)
        {
            // Configure Web API for self-host. 
            var config = new HttpConfiguration();
            config.Routes.MapHttpRoute(
                "DefaultApi",
                "api/{controller}/{id}",
                new {id = RouteParameter.Optional}
            );

            appBuilder.UseWebApi(config);

            appBuilder.UseFileServer(new FileServerOptions()
            {
                RequestPath = PathString.Empty,
                FileSystem = new PhysicalFileSystem(@".\client"),
                EnableDirectoryBrowsing = true
            });
            appBuilder.UseStaticFiles("/client");

            ConfigureUnity(config);
        }

        private void ConfigureUnity(HttpConfiguration config)
        {
            var container = new UnityContainer();
            config.DependencyResolver = new UnityDependencyResolver(container);

            LoggerModule.RegisterDependencies(container);

            ClientModule.Registrar.RegisterDependencies(container);
            ClientModule.Registrar.RegisterMappings();

            VmixClientModule.Registrar.RegisterDependencies(container);
            VmixClientModule.Registrar.RegisterMappings();

            CoreModule.SetDefaultSerializerSettings();
        }
    }
}