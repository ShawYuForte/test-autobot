using forte.services;
using Microsoft.Practices.Unity;

namespace forte
{
    public static class SerilogModule
    {
        public static class Registrar
        {
            public static void RegisterDependencies(IUnityContainer container)
            {
                var seriLogger = new SeriLogger();
                seriLogger.Configure();

                // Register as singleton
                container.RegisterInstance<ILogger>(seriLogger);
            }

            public static void RegisterMappings()
            {
            }
        }
    }
}
