#region

using System;
using device.client.web.server;
using Microsoft.Owin.Hosting;

#endregion

namespace device.client.web
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var baseAddress = "http://localhost:9000/";

            // Start OWIN host 
            using (WebApp.Start<Startup>(baseAddress))
            {
                Console.WriteLine("Running...");
                Console.ReadLine();
            }
        }
    }
}