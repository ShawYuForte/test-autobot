using device.web.server;
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
        }
    }
}