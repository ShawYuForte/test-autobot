#region

using CommandLine;
using CommandLine.Text;

#endregion

namespace forte.devices.options
{
    public class UpgradeOptions
    {
        public const string VerbName = "upgrade";

        [Option("source", Required = true, HelpText = "Updated package source.")]
        public string Source { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            return HelpText.AutoBuild(this, (HelpText current) => HelpText.DefaultParsingErrorsHandler(this, current));
        }
    }
}