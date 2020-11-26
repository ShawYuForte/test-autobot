using System.Text;
using CommandLine;
using CommandLine.Text;

namespace forte.devices.options
{
    public class RunOptions
    {
        public const string VerbName = "run";

        [Option('s', "server", Required = false, HelpText = "API server root url.")]
        public string ServerUrl { get; set; }

        [Option('p', "port", Required = false, HelpText = "Local server UI port.", DefaultValue = 9000)]
        public int Port { get; set; }

        [Option('d', "datapath", DefaultValue = @"c:\forte\data", HelpText = "Path where local data will be stored.")]
        public string DataPath { get; set; }

        [Option("deviceid", Required = false, HelpText = "Device identifier, if you are setting up a new device.")]
        public string DeviceId { get; set; }

        [Option('l', "logpath", DefaultValue = @"c:\forte\logs", HelpText = "Path where local logs will be stored.")]
        public string LogPath { get; set; }

        [Option('b', "background", HelpText = "Runs the daemon in the background (no console).")]
        public bool Background { get; set; }

		[Option("pr", HelpText = "Path where vmix preset is stored.")]
		public string PresetPath { get; set; }

		[Option("test-preset", DefaultValue = false, HelpText = "Run vmix preset flow, without streaming")]
		public bool TestPreset { get; set; }

		[Option("test-api", DefaultValue = false, HelpText = "Run api test request")]
		public bool TestApi { get; set; }

		[HelpOption]
        public string GetUsage()
        {
            return HelpText.AutoBuild(this, (HelpText current) => HelpText.DefaultParsingErrorsHandler(this, current));
        }

        public string ToArgs()
        {
            var buffer = new StringBuilder();

            buffer.Append("run");

			if (!string.IsNullOrWhiteSpace(ServerUrl))
			{
				buffer.Append($" -s {ServerUrl}");
			}

            buffer.Append($" -p {Port}");
            buffer.Append($" -d \"{DataPath}\"");
            buffer.Append($" -l \"{LogPath}\"");

			if (!string.IsNullOrEmpty(PresetPath))
			{
				buffer.Append($" -pr \"{PresetPath}\"");
			}

			if (Background)
			{
				buffer.Append(" -b");
			}

            return buffer.ToString();
        }
    }
}