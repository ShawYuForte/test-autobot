using System;
using forte.devices.services;
using Microsoft.Practices.Unity;

namespace forte.devices
{
    public class Program
    {
        private static void Main(string[] args)
        {
            Console.Write("Starting client connection... ");
            var container = new UnityContainer();

            ClientModule.Registrar.RegisterDependencies(container);
            VmixClientModule.Registrar.RegisterDependencies(container);

            var deviceManager = container.Resolve<IDeviceManager>();
            deviceManager.MessageReceived += Client_MessageReceived;
            deviceManager.Connect().Wait();
            Console.WriteLine("Done!");

            Console.WriteLine("Waiting...");
            string input;

            while (!string.IsNullOrWhiteSpace(input = Console.ReadLine()))
            {
                Console.WriteLine($"Sending '{input}'");
                deviceManager.Send(input).Wait();
            }
            //Console.ReadLine();
        }

        private static void Client_MessageReceived(string message)
        {
            Console.WriteLine($"From server: {message}");
        }
    }
}