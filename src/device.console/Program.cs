using System;
using forte.devices.services;
using Microsoft.Practices.Unity;

namespace forte.devices
{
    public class Program
    {
        const string WaitingForUserInput = "Waiting for user input (type '?' for available commands)";

        private static void Main(string[] args)
        {
            Console.Write("Starting client connection... ");
            var container = new UnityContainer();

            ServiceModule.Registrar.RegisterDependencies(container);
            ClientModule.Registrar.RegisterDependencies(container);
            VmixClientModule.Registrar.RegisterDependencies(container);

            var logger = container.Resolve<ILogger>();

            var deviceManager = container.Resolve<IStreamingDevice>();
            deviceManager.MessageReceived += Client_MessageReceived;
            deviceManager.Connect();
            Console.WriteLine("Done!");

            logger.Debug(WaitingForUserInput);
            string input;

            while (!string.IsNullOrWhiteSpace(input = Console.ReadLine()))
            {
                if (input == "update")
                {
                    logger.Debug("User requested to publish state");
                    deviceManager.PublishState();
                }
                if (input == "fetch")
                {
                    logger.Debug("User requested one device cycle");
                    deviceManager.FetchCommand();
                }
                if (input.StartsWith("stream"))
                {
                    var videoStreamId = Guid.Parse(input.Split(':')[1]);
                    logger.Debug("User requested to stream for video stream id {@videoStreamId}", videoStreamId);
                    //Console.WriteLine($"Starting stream for video stream id {videoStreamId}");
                    deviceManager.StartStreaming(videoStreamId);
                }
                if (input == "?")
                {
                    logger.Debug("update: publishes state");
                    logger.Debug("fetc: fetches and processes next command");
                    logger.Debug("stream {video-stream-id}: starts streaming for specified video stream");
                }
                //  Console.WriteLine($"Sending '{input}'");
                //deviceManager.Send(input).Wait();
                logger.Debug(WaitingForUserInput);
            }
        }

        private static void Client_MessageReceived(string message)
        {
            Console.WriteLine($"From server: {message}");
        }
    }
}