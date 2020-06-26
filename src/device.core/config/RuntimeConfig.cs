using forte.devices.services;

namespace forte.devices.config
{
	public class RuntimeConfig: IRuntimeConfig
	{
		public string DataPath { get; set; }
		public string LogPath { get; set; }
	}
}
