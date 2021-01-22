using System.IO;
using System.Net;
using forte.devices.config;
using forte.services;
using Newtonsoft.Json;

namespace forte.devices.services
{
	public class AgoraService
	{
        private readonly ILogger _logger;
        private readonly string _agoraApiUrl;

        public AgoraService(ILogger logger, IConfigurationManager configManager)
		{
			var config = configManager.GetDeviceConfig();
            _logger = logger;
            _agoraApiUrl = config.Get<string>(SettingParams.ServerApiPath);
        }

		public string GetChannelKey(string channelName, string deviceId, uint uid)
		{
            var url = $"{_agoraApiUrl}/streams/getVideoMeetingToken";
			var reaponseData = CallRestMethod(url,
			new
			{
				SessionId = channelName,
				AgoraUserId = uid,
				StreamingDeviceId = deviceId
			});
			_logger.Debug(reaponseData);
			var keyData = JsonConvert.DeserializeObject<dynamic>(reaponseData);
			return keyData.token;
		}

		private string CallRestMethod(string url, object data)
		{
			HttpWebRequest httpWebRequest = (HttpWebRequest) WebRequest.Create(url);
			httpWebRequest.Method = "POST";
			httpWebRequest.ContentType = "application/json";

			var requestData = JsonConvert.SerializeObject(data);
			using(var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
			{
				streamWriter.Write(requestData);
			}

			var httpResponse = (HttpWebResponse) httpWebRequest.GetResponse();
			string response;
			using(var streamReader = new StreamReader(httpResponse.GetResponseStream()))
			{
				response = streamReader.ReadToEnd();
			}
			return response;
		}
    }
}
