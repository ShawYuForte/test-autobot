#region

using System.Web.Http;
using forte.devices;
using Microsoft.Owin;
using Microsoft.Owin.FileSystems;
using Microsoft.Owin.StaticFiles;
using Microsoft.Practices.Unity;
using Owin;

#endregion

namespace device.web.server
{
    public class Startup
    {
        // This code configures Web API. The Startup class is specified as a type
        // parameter in the WebApp.Start method.
        public void Configuration(IAppBuilder appBuilder)
        {
            // Configure Web API for self-host. 
            var config = new HttpConfiguration();

            config.MapHttpAttributeRoutes();
            config.Routes.MapHttpRoute(
                "DefaultApi",
                "api/{controller}/{id}",
                new {id = RouteParameter.Optional }
            );

            var formatters = config.Formatters;
            var jsonFormatter = formatters.JsonFormatter;
            CoreModule.SetDefaultSerializerSettings(jsonFormatter.SerializerSettings);

            // remove XML support for Web API calls
            formatters.Remove(formatters.XmlFormatter);

            appBuilder.UseWebApi(config);
            appBuilder.MapSignalR();

            appBuilder.UseFileServer(new FileServerOptions()
            {
                RequestPath = PathString.Empty,
                FileSystem = new PhysicalFileSystem(@".\client"),
                EnableDirectoryBrowsing = true
            });
            appBuilder.UseStaticFiles("/client");

            config.DependencyResolver = UnityResolver.Default;
        }
    }
}