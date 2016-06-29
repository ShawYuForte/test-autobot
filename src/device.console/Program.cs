using System;
using device.client;

namespace device.console
{
    public class Program
    {
        private static void Main(string[] args)
        {
            Console.Write("Starting client connection... ");
            var client = new Client();
            client.MessageReceived += Client_MessageReceived;
            client.Connect().Wait();
            Console.WriteLine("Done!");

            Console.WriteLine("Waiting...");
            Console.ReadLine();
        }

        private static void Client_MessageReceived(string message)
        {
            Console.WriteLine($"From server: {message}");
        }
    }
}