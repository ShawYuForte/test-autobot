using System;
using Microsoft.Owin.Hosting;

namespace device.web.server
{
    public class ApiServer
    {
        public IDisposable Run(int port)
        {
            var baseAddress = $"http://localhost:{port}/";

            return WebApp.Start<Startup>(baseAddress);
        }
    }
}