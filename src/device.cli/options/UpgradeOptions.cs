#region

using CommandLine;
using CommandLine.Text;

#endregion

namespace forte.devices.options
{
    public class UpgradeOptions
    {
        public const string VerbName = "upgrade";

        [Option("source", Required = false, HelpText = "Updated package source.")]
        public string Source { get; set; }

        [Option("repo", DefaultValue = @"c:\forte\repo", HelpText = "Local NuGet package repo.")]
        public string Repo { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            return HelpText.AutoBuild(this, (HelpText current) => HelpText.DefaultParsingErrorsHandler(this, current));
        }
    }
}