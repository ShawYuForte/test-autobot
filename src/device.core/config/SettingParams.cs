namespace forte.devices.config
{
	public static class SettingParams
	{
		public static string CustomDeviceId => "CustomDeviceId";
		public static string CustomDeviceIdPresent => "CustomDeviceIdPresent";
		public static string DeviceId => "DeviceId";
		public static string ServerApiPath => "server-api-path";
		public static string ServerRootPath => "server-root-path";
		public static string VerboseDebug => "use-extended-logging";
		public static string DeviceName => "device-name";
		public static string AgoraRtmpEnabled => "agora-rtmp-enabled";
		public static string AgoraApiUrl => "agora-api-url";
        public static string AgoraRtmpUrl => "agora-rtmp-url";
		//HTTP client time out
		public static string ClientTimeOut => "client-time-out";

		public static string LinkTime => "link-time";
		public static string MailTo => "mail-to";
	}
}
