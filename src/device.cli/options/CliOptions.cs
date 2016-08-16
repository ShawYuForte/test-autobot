using CommandLine;
using CommandLine.Text;

namespace forte.devices.options
{
    public class CliOptions
    {
        [Option('s', "server", Required = true, HelpText = "API server root url.")]
        public string ServerUrl { get; set; }

        [Option('p', "port", Required = false, HelpText = "Local server UI port.", DefaultValue = 9000)]
        public int Port { get; set; }

        [Option('d', "datapath", Required = true, HelpText = "Path where local data will be stored.")]
        public string DataPath { get; set; }

        [Option('l', "logpath", Required = true, HelpText = "Path where local logs will be stored.")]
        public string LogPath { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            return HelpText.AutoBuild(this, (HelpText current) => HelpText.DefaultParsingErrorsHandler(this, current));
        }
    }
}