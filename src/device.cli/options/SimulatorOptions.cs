using System;
using CommandLine;
using CommandLine.Text;

namespace forte.devices.options
{
    public class SimulatorOptions
    {
        public const string VerbName = "simulate";

        [Option('s', "server", Required = true, HelpText = "API server root url.")]
        public string ServerUrl { get; set; }

        [Option('d', "device", Required = false, HelpText = "Default device identifier.")]
        public Guid DeviceId { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            return HelpText.AutoBuild(this, current => HelpText.DefaultParsingErrorsHandler(this, current));
        }
    }
}