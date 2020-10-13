using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using AgoraSDK;
using forte.devices.config;
using forte.services;
using Newtonsoft.Json;

namespace forte.devices.services
{
	public class AgoraService
	{
		private bool _joined;
		private readonly bool _enabled;
		private readonly ILogger _logger;
		private readonly string _appId;
		private readonly string _agoraApiUrl;
		private RtcEngine _rtcEngine;
		private readonly MailService _ms;
		private readonly Dictionary<string, uint> _uids = new Dictionary<string, uint>();

		public AgoraService(ILogger logger, IConfigurationManager configManager, MailService ms)
		{
			var config = configManager.GetDeviceConfig();

			_logger = logger;
			_ms = ms;
			_appId = config.Get<string>(SettingParams.AgoraAppId);
			_agoraApiUrl = config.Get<string>(SettingParams.ServerApiPath);
			_enabled = !string.IsNullOrEmpty(_appId) && !string.IsNullOrEmpty(_agoraApiUrl) && _appId != "false" && _agoraApiUrl != "false";
		}

		public async Task Connect(string channelName, string deviceId)
		{
			if(!_enabled) return;
			await Disconnect();
			await Task.Run(async () =>
			{
				try
				{
					// init engine
					_rtcEngine = RtcEngine.GetEngine(_appId);

					// set callbacks (optional)
					_rtcEngine.OnJoinChannelSuccess = onJoinChannelSuccess;
					_rtcEngine.OnUserJoined = onUserJoined;
					_rtcEngine.OnUserOffline = onUserOffline;
					_rtcEngine.OnError = (e, m) => { _logger.Error($"Sdk error: {e} {m}"); };
					_rtcEngine.OnWarning = (e, m) => { _logger.Warning($"Sdk warn: {e} {m}"); };

					if (!_uids.ContainsKey(channelName))
					{
						_uids[channelName] = (uint) channelName.GetHashCode();
						//_uids[channelName] = GetChannelUid();
					}
					var uid = _uids[channelName];

					var channelKey = GetChannelKey(channelName, deviceId, uid);
					_logger.Debug($"Channel: {channelName}, Uid: {uid}, Channel key: {channelKey}");

					var isSetVideoEncoderConfigurationSuccessful = _rtcEngine.SetVideoEncoderConfiguration(new VideoEncoderConfiguration
					{
						dimensions = new VideoDimensions
						{
							width = 1280,
							height = 720
						},
						frameRate = FRAME_RATE.FRAME_RATE_FPS_30,
						bitrate = 3420
					});

					// enable video
					_rtcEngine.EnableVideo();

					// join channel
					var joinResult = _rtcEngine.JoinChannelByKey(channelKey, channelName.ToUpper(), null, uid);
					var waitTime = 0;
					_logger.Debug($"Joined Agora result: {joinResult}");
					_joined = false;
					while(true)
					{
						await Task.Delay(100);
						waitTime += 100;
						if(_joined) break;
						if(waitTime > 10000) throw new Exception("Wasn't able to join to Agora channel. Streaming to Agora won't be possible.");
					}

					// Expected to be returned count of connected video cameras by always returns -10000000
					//var vdCount = _rtcEngine.GetVideoDeviceCount();
					//Console.WriteLine(string.Format("Video Device Count : {0}", vdCount));
				}
				catch(Exception ex)
				{
					await Disconnect();
					_logger.Error(ex, $"Error connecting to Agora.");
					//catch error in the main workflow instead
					throw;
				}
			});
		}

		public async Task Disconnect()
		{
			if(!_enabled || _rtcEngine == null) return;
			_logger.Debug($"Attempt disconnect");

			await Task.Run(() =>
			{
				try
				{
					_rtcEngine.LeaveChannel();
					//_rtcEngine.ReleaseQueue();
					//_rtcEngine = null;
				}
				catch(Exception ex)
				{
					_logger.Error(ex, "");
				}
			});
		}

		#region callabcks

		private void onJoinChannelSuccess(string channelName, uint uid, int elapsed)
		{
			_logger.Debug(string.Format("SDK Version : {0}", _rtcEngine.GetSdkVersion()));
			_logger.Debug(string.Format("onJoinChannelSuccess: channelName = {0}, uid = {1}, elapsed = {2}", channelName, uid, elapsed));
			_joined = true;
		}

		private void onUserJoined(uint uid, int elapsed)
		{
			_logger.Debug(string.Format("onUserJoined: uid = {0}, elapsed = {1}", uid, elapsed));
		}

		private void onUserOffline(uint uid, USER_OFFLINE_REASON reason)
		{
			_logger.Debug(string.Format("onUserOffline: uid = {0}, reason = {1}", uid, reason));
		}

		#endregion

		#region implementation

		private uint GetChannelUid()
		{
			Random random = new Random();
			var uid = random.Next(0, int.MaxValue);
			return (uint) uid + (uint) random.Next(0, int.MaxValue);
		}

		private string GetChannelKey(string channelName, string deviceId, uint uid)
		{
			//var url = "https://agora-token-demo.azurewebsites.net" + "/api/agorademo";
			//var reaponseData = CallRestMethod(url,
			//new
			//{
			//	ChannelName = channelName,
			//	Uid = uid
			//});
			//var keyData = JsonConvert.DeserializeObject<dynamic>(reaponseData);
			//return keyData.Token;

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
			var response = string.Empty;
			using(var streamReader = new StreamReader(httpResponse.GetResponseStream()))
			{
				response = streamReader.ReadToEnd();
			}
			return response;
		}

		#endregion
	}
}
