namespace forte.devices.services
{
	public interface IRuntimeConfig
	{
		string DataPath { get; set; }
		string LogPath { get; set; }
	}
}