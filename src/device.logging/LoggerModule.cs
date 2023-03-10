#region

using device.logging.services;
using forte;
using forte.devices.services;
using forte.services;
using Microsoft.Practices.Unity;

#endregion

namespace device.logging
{
    public class LoggerModule
    {
        public static class Registrar
        {
            public static void RegisterDependencies(IUnityContainer unityContainer)
            {
                unityContainer.RegisterType<SeriLoggerEx, SeriLoggerEx>(new ContainerControlledLifetimeManager());
				var seriLogger = unityContainer.Resolve<SeriLoggerEx>();
				seriLogger.Configure();
				unityContainer.RegisterInstance<ILogger>(seriLogger);
			}
        }
    }
}