#region

using device.logging.services;
using forte;
using forte.services;
using Microsoft.Practices.Unity;

#endregion

namespace device.logging
{
    public class LoggerModule
    {
        public static void RegisterDependencies(IUnityContainer unityContainer)
        {
            unityContainer.RegisterType<ILogger, SeriLoggerEx>();
            unityContainer.RegisterType<IDeviceLogger, SeriLoggerEx>();
        }
    }
}