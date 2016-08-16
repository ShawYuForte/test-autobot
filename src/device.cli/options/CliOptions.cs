using CommandLine;
using CommandLine.Text;

namespace forte.devices.options
{
    public class CliOptions
    {
        [Option("server", Required = true, HelpText = "API server root url.")]
        public string ServerUrl { get; set; }

        [Option("port", Required = false, HelpText = "Local server UI port.", DefaultValue = 9000)]
        public int Port { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            return HelpText.AutoBuild(this, (HelpText current) => HelpText.DefaultParsingErrorsHandler(this, current));
        }
    }
}