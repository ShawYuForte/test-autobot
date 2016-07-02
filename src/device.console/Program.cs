using System;
using device.client;
using Newtonsoft.Json;

namespace device.console
{
    public class Program
    {
        private static void Main(string[] args)
        {
            var anon = new
            {
                Name = "Anonymous"
            };
            var str = JsonConvert.SerializeObject(anon);
            Console.WriteLine(str);
            var back = JsonConvert.DeserializeAnonymousType(str, new { Name = "" });
            var dyn = JsonConvert.DeserializeObject<dynamic>(str);
            Console.WriteLine(back.Name);
            Console.WriteLine(dyn.Name);
            Console.ReadLine();

            //Console.Write("Starting client connection... ");
            //var client = new Client();
            //client.MessageReceived += Client_MessageReceived;
            //client.Connect().Wait();
            //Console.WriteLine("Done!");

            //Console.WriteLine("Waiting...");
            //Console.ReadLine();
        }

        private static void Client_MessageReceived(string message)
        {
            Console.WriteLine($"From server: {message}");
        }
    }
}