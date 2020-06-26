using System;
using forte.devices.services;
using Microsoft.Owin.Hosting;

namespace device.web.server
{
    public class ApiServer: IApiServer
	{
        public IDisposable Run(int port)
        {
            var baseAddress = $"http://localhost:{port}/";

            return WebApp.Start<Startup>(baseAddress);
        }
    }
}