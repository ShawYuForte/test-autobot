using CommandLine;
using CommandLine.Text;

namespace forte.devices.options
{
    public class CliOptions 
    {
		public CliOptions()
        {
        }

        [VerbOption(RunOptions.VerbName, HelpText = "Run daemon, if not already running.")]
        public RunOptions RunVerb { get; set; }

        //[VerbOption(SimulatorOptions.VerbName, HelpText = "Run simulator daemon.")]
        //public SimulatorOptions SimulatorVerb { get; set; }

        [VerbOption(UpgradeOptions.VerbName, HelpText = "Upgrade application version.")]
        public UpgradeOptions UpgradeVerb { get; set; }

        [HelpVerbOption]
        public string DoHelpForVerb(string verbName)
        {
            return HelpText.AutoBuild(this, verbName);
        }
    }
}