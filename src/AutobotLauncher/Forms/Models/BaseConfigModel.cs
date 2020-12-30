namespace AutobotLauncher.Forms.Models
{
	public class BaseConfigModel : BaseViewModel
	{
		private string _customDeviceId;
		private string _deviceId;
		private string _apiPath;
		private string _apiPathExt;
		private string _agoraApp;

		public string CustomDeviceId
		{
			get { return _customDeviceId; }
			set { _customDeviceId = value; OnPropertyChanged("CustomDeviceId"); }
		}

		public string DeviceId
		{
			get { return _deviceId; }
			set { _deviceId = value; OnPropertyChanged("DeviceId"); }
		}

		public string ApiPath
		{
			get { return _apiPath; }
			set
			{
				if (!value.EndsWith("/"))
				{
					value += "/";
				}

				_apiPath = value;
				_apiPathExt = $"{value}api";
				OnPropertyChanged("ApiPath");
			}
		}

		public string ApiPathExt { get { return _apiPathExt; } }

		public string AgoraApp
		{
			get { return _agoraApp; }
			set { _agoraApp = value; OnPropertyChanged("AgoraApp"); }
		}
	}
}
