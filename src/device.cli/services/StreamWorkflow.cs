using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using device.web.server;
using forte.devices.config;
using forte.devices.data;
using forte.devices.data.enums;
using forte.devices.entities;
using forte.devices.extensions;
using forte.devices.models;
using forte.devices.services;
using forte.models;
using forte.models.devices;
using forte.services;
using Newtonsoft.Json;
using RestSharp;

namespace forte.devices.workflow
{
	public class StreamWorkflow: IDeviceDaemon
	{
        private readonly IApiServer _server;
        private readonly ILogger _logger;
		private readonly IConfigurationManager _configurationManager;
		private readonly DbRepository _dbRepository;
        private readonly IStreamingClient _streamingClient;
		private RestClient _client;
		private RestClient _clientDevice;

		private bool _running = true;
		private string _deviceId;
		private ConcurrentQueue<StreamingDeviceCommandModel> _commands = new ConcurrentQueue<StreamingDeviceCommandModel>();
		private Dictionary<Guid, SessionState> _lockedSessions = new Dictionary<Guid, SessionState>();

		public StreamWorkflow(
			IApiServer server,
			IStreamingClient streamingClient, 
			DbRepository dbRepository, 
			IConfigurationManager configurationManager, 
			ILogger logger)
		{
			_server = server;
			_streamingClient = streamingClient;
			_dbRepository = dbRepository;
			_configurationManager = configurationManager;
			_logger = logger;
		}

		public void Await(int port)
		{
			//_serverListener.Connect();
			using(var server = _server.Run(port))
			{
				_logger.Information("Running device local UI web server.");
				//synchronous loop for keeping the app alive
				while(_running)
				{
					Thread.Sleep(1);
				}
			}
		}

		public async Task Start()
		{
			var config = _configurationManager.GetDeviceConfig();
			var apiPath = config.Get<string>(SettingParams.ServerApiPath);
			var seconds = 0;
			_deviceId = config.Get<string>(SettingParams.DeviceId);
			_client = _client ?? new RestClient($"{apiPath}/streams/");
			_clientDevice = _clientDevice ?? new RestClient($"{apiPath}/devices/");

			//async loop for processing commands
			Run();
			//async loop for checking new commands
			while(true)
			{
				if(seconds != 0)
				{
					await Task.Delay(1000);
					seconds--;
					continue;
				}

				seconds = 10;

				try
				{
					ThreadSafeFetchCommand();
				}
				catch(Exception exception)
				{
					_logger.Error(exception, exception.Message);
				}
			}

			//$"0-{DateTime.Now}", "405E2FBA-C7C1-4C8A-B10B-9A63D2375D60"
		}

		private async void Run()
		{
			var fakeRun = true;
			var now = DateTime.UtcNow;
			fakeRun = false;
			//now = new DateTime(2020, 6, 19, 17, 30, 00).ToUniversalTime();
			//var sessions = new List<SessionState>
			//{
			//	new SessionState
			//	{
			//		//StartTime = DateTime.UtcNow,
			//		StartTime = now,
			//		EndTime = now.AddMinutes(4),
			//		SessioId = new Guid(sessionId),
			//		Status = WorkflowState.Idle,
			//		Permalink = sessionName,
			//		VmixPreset = "Test"
			//	}
			//};

			var config = _configurationManager.GetDeviceConfig();
			var linkRetrySeconds = 15;
			var streamRetrySeconds = 15;
			var streamStopRetrySeconds = 15;
			var linkTimeSeconds = 10 * 60;
			var staticImageSeconds = config.Get(VmixSettingParams.StaticImageTime, 30);
			var serverStreamSeconds = staticImageSeconds + 10;
			var sessions = _dbRepository.GetSessions();

			double overdue = 0;
			while(true)
			{
				await Task.Delay(1000);

				//executer pending commands before processing further
				StreamingDeviceCommandModel command;
				if(_commands.TryDequeue(out command))
				{
					try
					{
						command.ExecutionAttempts++;

						var session = sessions.FirstOrDefault(s => s.SessioId == command.SessionId.Value);
						var isNew = session == null;
						switch(command.Command)
						{
							case StreamingDeviceCommands.UpdateSession:
								session = UpdateSession(session, command);
								if(isNew)
								{
									sessions.Add(session);
								}
								break;
							case StreamingDeviceCommands.CancelSession:
								session = CancelSession(session, command);
								break;
							default:
								throw new ArgumentOutOfRangeException();
						}

						command.ExecutionSucceeded = true;
						command.ExecutedOn = DateTime.UtcNow;
					}
					catch(Exception exception)
					{
						if(string.IsNullOrWhiteSpace(command.ExecutionMessages))
						{
							command.ExecutionMessages = exception.Message;
						}
						else
						{
							command.ExecutionMessages += $"{exception.Message};\n\r";
						}

						if(command.ExecutionAttempts >= command.MaxAttemptsAllowed)
						{
							command.ExecutedOn = DateTime.UtcNow;
							command.ExecutionSucceeded = false;
							_logger.Fatal(exception, "Could not execute command {@command}", command);
						}
						else
						{
							_logger.Error(exception, "Could not execute command {@command}", command);
						}
					}

					try
					{
						SaveCommandOnServer(command);
					}
					catch(Exception exception)
					{
						_logger.Fatal(exception, "Could not save command on the server {@command}", command);
					}
				}

				sessions = sessions.Where(s => s.Status != WorkflowState.Processed).OrderBy(s => s.StartTime).ToList();
				if(!sessions.Any()) continue;

				var vmixSession = sessions.FirstOrDefault(s => s.VmixUsed == true);
				foreach(var s in sessions)
				{
					if(_lockedSessions.ContainsKey(s.SessioId)) continue;
					//check if vmix is free to use for this session
					var isVmixSession = (vmixSession == null || vmixSession.Id == s.Id);
					if(isVmixSession) { SetSessionVmix(s, true); }

					//terminate outdated session
					if(s.EndTime <= DateTime.UtcNow)
					{
						_logger.Warning($"{s.Permalink}: {(DateTime.UtcNow - s.EndTime).TotalSeconds} seconds after end, terminate");
						_lockedSessions[s.SessioId] = s;
						StopStream(s, streamStopRetrySeconds);
						continue;
					}

					//start new session
					if(s.Status == WorkflowState.Idle && s.StartTime.AddSeconds(-linkTimeSeconds) <= DateTime.UtcNow)
					{
						_logger.Warning($"{s.Permalink}: {(s.StartTime - DateTime.UtcNow).TotalSeconds} seconds before start, link resources");
						_lockedSessions[s.SessioId] = s;
						LinkStream(s, linkRetrySeconds);
						continue;
					}

					//run this as soon as session linked resources
					if(s.Status == WorkflowState.Linked)
					{
						if(!isVmixSession) continue;
						_logger.Warning($"{s.Permalink}");
						if(!fakeRun)
						{
							var r = await LoadPreset(s);
							if(!r) continue; //something went wrong with loading preset
						}
						SetSessionStatus(s, WorkflowState.VmixLoaded);
						continue;
					}

					//run this as we approach session start. default is starting 10 seconds before static image
					if(s.Status == WorkflowState.VmixLoaded && s.StartTime.AddSeconds(-serverStreamSeconds) <= DateTime.UtcNow)
					{
						_logger.Warning($"{s.Permalink}: {(s.StartTime - DateTime.UtcNow).TotalSeconds} seconds before start, start server stream");
						_lockedSessions[s.SessioId] = s;
						StartStream(s, streamRetrySeconds);
						continue;
					}

					//run this as we approach session start. default is starting static image 30 seconds before start
					if(s.Status == WorkflowState.StreamingServer)
					{
						if(!isVmixSession) continue;
						overdue = (s.StartTime - DateTime.UtcNow).TotalSeconds;
						if(overdue > staticImageSeconds) continue; //wait
						_logger.Warning($"{s.Permalink}: {overdue} seconds before start, start static image stream");
						if(!fakeRun)
						{
							var r1 = StartClientStream(s);
							if(!r1) continue; //something went wrong
						}
						SetSessionStatus(s, WorkflowState.StreamingClient);
						continue;
					}

					//start session
					if(s.Status == WorkflowState.StreamingClient)
					{
						if(!isVmixSession) continue;
						overdue = (s.StartTime - DateTime.UtcNow).TotalSeconds;
						if(overdue > 0) continue; //wait
						_logger.Warning($"{s.Permalink}: {overdue} seconds before start, start program");
						if(!fakeRun)
						{
							var r2 = StartProgram(s);
							if(!r2) continue; //something went wrong
						}
						SetSessionStatus(s, WorkflowState.Program);
						continue;
					}
				}
			}
		}

		private async Task LinkStream(SessionState s, int retrySeconds)
		{
			try
			{
				AddSessionRetry(s);
				var request = new RestRequest($"streamLink/{s.SessioId}?retryNum={s.RetryCount}&deviceId={_deviceId}&requestRef={s.Permalink}&programStart={s.StartTime}&programEnd={s.EndTime}", Method.GET);
				_client.ExecuteAsync<VideoStreamModel>(request, async (m) =>
				{
					try
					{
						if(m.StatusCode != HttpStatusCode.OK)
						{
							_logger.Error($"{s.Permalink}: bad response: {m.Content}, retry in {retrySeconds} seconds");
							await Task.Delay(retrySeconds * 1000);
							return;
						}

						_logger.Debug($"{s.Permalink}: LinkStream success");

						SetSessionUrl(s, m.Data.PrimaryIngestUrl);
						SetSessionStatus(s, WorkflowState.Linked);
						ClearSessionRetry(s);
					}
					catch(Exception ex)
					{
						_logger.Debug(ex, "");
					}
					finally
					{
						_lockedSessions.Remove(s.SessioId);
					}
				});
			}
			catch(Exception ex)
			{
				_lockedSessions.Remove(s.SessioId);
				_logger.Debug(ex, "");
			}
		}

		private async Task StartStream(SessionState s, int retrySeconds)
		{
			try
			{
				AddSessionRetry(s);
				var request = new RestRequest($"streamStart/{s.SessioId}?retryNum={s.RetryCount}&deviceId={_deviceId}&requestRef={s.Permalink}&programStart={s.StartTime}&programEnd={s.EndTime}", Method.GET);
				_client.ExecuteAsync<VideoStreamModel>(request, async (m) =>
				{
					try
					{
						if(m.StatusCode != HttpStatusCode.OK)
						{
							_logger.Error($"{s.Permalink}: bad response: {m.Content}, retry in {retrySeconds} seconds");
							await Task.Delay(retrySeconds * 1000);
							return;
						}

						_logger.Debug($"{s.Permalink}: StartStream success");

						SetSessionUrl(s, m.Data.PrimaryIngestUrl);
						SetSessionStatus(s, WorkflowState.StreamingServer);
						ClearSessionRetry(s);
					}
					catch(Exception ex)
					{
						_logger.Debug(ex, "");
					}
					finally
					{
						_lockedSessions.Remove(s.SessioId);
					}
				});
			}
			catch(Exception ex)
			{
				_lockedSessions.Remove(s.SessioId);
				_logger.Debug(ex, "");
			}
		}

		private async Task StopStream(SessionState s, int retrySeconds)
		{
			try
			{
				AddSessionRetry(s);

				if(s.VmixUsed && s.SessionType != SessionType.Manual)
				{
					_logger.Warning($"{s.Permalink}");
					_streamingClient.StopProgram();
				}

				var request = new RestRequest($"streamStop/{s.SessioId}?retryNum={s.RetryCount}&deviceId={_deviceId}&requestRef={s.Permalink}", Method.GET);
				_client.ExecuteAsync<bool>(request, async (m) =>
				{
					try
					{
						if(m.StatusCode != HttpStatusCode.OK)
						{
							_logger.Error($"{s.Permalink}: bad response: {m.Content}, retry in {retrySeconds} seconds");
							await Task.Delay(retrySeconds * 1000);
							return;
						}

						_logger.Debug($"{s.Permalink}: StopStream success");

						if(s.VmixUsed && s.SessionType != SessionType.Manual)
						{
							_logger.Warning($"{s.Permalink}");
							_streamingClient.StopStreaming(true);
						}

						SetSessionStatus(s, WorkflowState.Processed);
						ClearSessionRetry(s);
					}
					catch(Exception ex)
					{
						_logger.Debug(ex, "");
					}
					finally
					{
						_lockedSessions.Remove(s.SessioId);
					}
				});
			}
			catch(Exception ex)
			{
				_lockedSessions.Remove(s.SessioId);
				_logger.Debug(ex, "");
			}
		}

		private void SetSessionStatus(SessionState s, WorkflowState status)
		{
			s.Status = status;
			_dbRepository.UpdateSession(s);
		}

		private void SetSessionUrl(SessionState s, string primaryUrl)
		{
			s.PrimaryIngestUrl = primaryUrl;
			_dbRepository.UpdateSession(s);
		}

		private void SetSessionVmix(SessionState s, bool vmixUsed)
		{
			s.VmixUsed = vmixUsed;
			_dbRepository.UpdateSession(s);
		}

		private void AddSessionRetry(SessionState s)
		{
			s.RetryCount++;
			_dbRepository.UpdateSession(s);
		}

		private void ClearSessionRetry(SessionState s)
		{
			s.RetryCount = 0;
			_dbRepository.UpdateSession(s);
		}

		#region vmix steps

		private async Task<bool> LoadPreset(SessionState s)
		{
			try
			{
				AddSessionRetry(s);
				if(s.SessionType != SessionType.Manual)
				{
					await _streamingClient.LoadVideoStreamPreset(s.VmixPreset, s.PrimaryIngestUrl);
				}
				ClearSessionRetry(s);
				return true;
			}
			catch(Exception ex)
			{
				ReportError(s, ex, "Load Preset");
				_logger.Debug(ex, "");
			}

			return false;
		}

		private bool StartClientStream(SessionState s)
		{
			try
			{
				AddSessionRetry(s);
				if(s.SessionType != SessionType.Manual)
				{
					FlushDns();
					_streamingClient.StartStreaming();
				}
				ClearSessionRetry(s);
				return true;
			}
			catch(Exception ex)
			{
				ReportError(s, ex, "Load Preset");
				_logger.Debug(ex, "");
			}

			return false;
		}

		private bool StartProgram(SessionState s)
		{
			try
			{
				AddSessionRetry(s);
				if(s.SessionType != SessionType.Manual)
				{
					_streamingClient.StartProgram();
				}
				ClearSessionRetry(s);
				return true;
			}
			catch(Exception ex)
			{
				ReportError(s, ex, "Load Preset");
				_logger.Debug(ex, "");
			}

			return false;
		}

		#endregion

		#region interface members

		public void Shutdown()
		{
			_running = false;
		}

		#endregion

		#region internal

		private void ThreadSafeFetchCommand()
		{
			var request = new RestRequest($"{_deviceId}/commands/next", Method.GET);
			var response = _clientDevice.Execute(request);

			// Not found if no command
			if(response.StatusCode == HttpStatusCode.NotFound)
			{
				//_logger.Debug("No command available, exiting");
				// ... so just exit
				return;
			}

			if(response.StatusCode != HttpStatusCode.OK)
			{
				_logger.Error("Failed fetching command, response {@response}", response);
				return;
			}

			_logger.Debug("Command retrieved {@command}", response.Content);
			var command = JsonConvert.DeserializeObject<StreamingDeviceCommandModel>(response.Content);
			_commands.Enqueue(command);
		}

		private void SaveCommandOnServer(StreamingDeviceCommandModel commandModel)
		{
			// TODO handle duplicates and queuing
			var request = new RestRequest($"{_deviceId}/commands/{commandModel.Id}", Method.PUT) { JsonSerializer = NewtonsoftJsonSerializer.Default };
			request.AddHeader("Content-Type", "application/json; charset=utf-8");
			request.AddJsonBody(commandModel);
			var response = _clientDevice.Execute<StreamingDeviceCommandModel>(request);
			if(response.StatusCode == HttpStatusCode.OK) return;

			_logger.Error("Could not update command because of {@status}, response {@resposne}", response.ErrorMessage ?? response.StatusDescription, response);
		}

		private SessionState UpdateSession(SessionState state, StreamingDeviceCommandModel command)
		{
			var isNew = state == null;
			if(isNew)
			{
				state = new SessionState
				{
					SessioId = command.SessionId.Value,
					Permalink = command.Permalink,
					VmixPreset = command.Preset,
					Status = WorkflowState.Idle,
					SessionType = (SessionType) command.Type
				};
			}

			state.StartTime = command.TimeStart.Value;
			state.EndTime = command.TimeEnd.Value;

			if(isNew)
			{
				_dbRepository.CreateSession(state);
			}
			else
			{
				_dbRepository.UpdateSession(state);
			}

			return state;
		}

		private SessionState CancelSession(SessionState state, StreamingDeviceCommandModel command)
		{
			if(state != null && state.Status != WorkflowState.Processed)
			{
				state.EndTime = new DateTime(1900, 1, 1); //trigger session ending by setting end time in the past
				_dbRepository.UpdateSession(state);
			}

			return state;
		}

		private void FlushDns()
		{
			var buffer = new StringBuilder();
			var process = new Process
			{
				StartInfo =
				{
					FileName = "ipconfig",
					Arguments = "/flushdns",
					RedirectStandardOutput = true,
					UseShellExecute = false
				}
			};
			process.OutputDataReceived += delegate (object o, DataReceivedEventArgs args) { buffer.AppendLine(args.Data); };
			process.Start();
			// Start the asynchronous read of the sort output stream.
			process.BeginOutputReadLine();
			process.WaitForExit();
			var output = buffer.ToString();
			if(string.IsNullOrWhiteSpace(output) || !output.Contains("Successfully flushed"))
			{
				_logger.Warning("Could not flush DNS Resolver Cache, process output: {@output}", output);
			}
		}

		private void ReportError(SessionState s, Exception ex, string message)
		{
			if(s.RetryCount != 5) return;
			_logger.Fatal(ex, $"{s.Permalink}: Streaming action {message} have failed 5 times. The system might be trying to retry further, but it is recommended to check the cause of this error.");
		}

		#endregion
	}
}


//#region

//using System;
//using System.Net;
//using System.Threading;
//using AutoMapper;
//using forte.devices.extensions;
//using forte.models.devices;
//using forte.services;
//using Newtonsoft.Json;
//using RestSharp;
//using StreamingDeviceState = forte.devices.models.StreamingDeviceState;
//using StreamingDeviceStatuses = forte.devices.models.StreamingDeviceStatuses;

//#endregion

//namespace forte.devices.services
//{
//	/// <summary>
//	///     Streaming device manager, responds to WebSocket messages when there are new commands, and then for each command:
//	///     - Fetches command
//	///     - Saves locally
//	///     - Executes the command
//	///     - Saves state locally
//	///     - Saves command state on the server
//	///     - Publishes new client state
//	/// </summary>
//	public class DeviceDaemon
//    {
//        public delegate void MessageReceivedDelegate(string message);

//        private static readonly object Key = new object();

//        private RestClient _client;
//        private readonly IConfigurationManager _configurationManager;
//        private Guid _deviceId;
//        private readonly IDeviceRepository _deviceRepository;
//        private readonly ILogger _logger;
//        private readonly IServerListener _serverListener;
//        private RestClient _streamClient;
//        private DateTime _lastPublishTime = DateTime.MinValue;

//        private readonly IStreamingClient _streamingClient;

//        public DeviceDaemon(IDeviceRepository deviceRepository, IStreamingClient streamingClient, ILogger logger,
//            IConfigurationManager configurationManager, IServerListener serverListener)
//        {
//            _deviceRepository = deviceRepository;
//            _streamingClient = streamingClient;
//            _logger = logger;
//            _configurationManager = configurationManager;
//            _serverListener = serverListener;
//            serverListener.MessageReceived += OnServerMessageReceived;
//        }

//        public void Init()
//        {
//            Init(Guid.Empty);
//        }
//        //public void Init(Guid deviceId)
//        //{
//        //    SetDefaultSettings();
//        //    var config = _configurationManager.GetDeviceConfig();
//        //    if (deviceId != Guid.Empty)
//        //    {
//        //        if (config.DeviceId != deviceId)
//        //        {
//        //            if (config.DeviceId != Guid.Empty)
//        //            {
//        //                _logger.Warning("New device identifier specified, changing from {@oldId} to {@newId}",
//        //                    config.DeviceId, deviceId);
//        //            }
//        //            else
//        //            {
//        //                _logger.Information("Setting new device identifier {@newId}", deviceId);
//        //            }
//        //            _configurationManager.UpdateSetting(nameof(models.StreamingDeviceConfig.DeviceId), deviceId);
//        //            var storedState = GetState();
//        //            if (storedState.DeviceId != deviceId)
//        //            {
//        //                storedState.DeviceId = deviceId;
//        //                _deviceRepository.Save(storedState);
//        //            }
//        //        }
//        //    }
//        //    _client = new RestClient($"{config.Get<string>(SettingParams.ServerApiPath)}/devices/");
//        //    _streamClient = new RestClient($"{config.Get<string>(SettingParams.ServerApiPath)}/streams/");
//        //    _deviceId = config.DeviceId;
//        //    _logger.Information("Device unique identifier {@deviceId}", _deviceId);
//        //}

//        private bool FetchRequested { get; set; }

//        private bool Stopped { get; set; }

//        public void ForceResetToIdle()
//        {
//            _logger.Warning("Force resetting device to idle");

//            var state = GetState();
//            //_streamingClient.ShutDown();
//            state.Status = StreamingDeviceStatuses.Idle;
//            _deviceRepository.Save(state);

//            _logger.Debug("Device reset to idle!");
//        }

//        public StreamingDeviceState GetState()
//        {
//            var state = _deviceRepository.GetDeviceState();
//            if (state != null) return state;

//            var config = _configurationManager.GetDeviceConfig();

//            state = new StreamingDeviceState
//            {
//                //DeviceId = config.DeviceId,
//                StateCapturedOn = DateTime.UtcNow,
//                Status = StreamingDeviceStatuses.Idle
//            };
//            _deviceRepository.Save(state);
//            return state;
//        }

//        private void TryPublishState()
//        {
//            try
//            {
//                PublishState();
//            }
//            catch (Exception exception)
//            {
//                _logger.Error(exception, "Could not publish state");
//            }
//        }

//        /// <summary>
//        ///     Publish device state to the server
//        /// </summary>
//        public bool PublishState()
//        {
//            _logger.Debug("Publishing state");

//            var deviceState = GetState();
//            var request = new RestRequest($"{deviceState.DeviceId}/state", Method.POST)
//            {
//                JsonSerializer = NewtonsoftJsonSerializer.Default
//            };
//            request.AddHeader("Content-Type", "application/json; charset=utf-8");
//            request.AddJsonBody(deviceState);
//            var response = _client.Execute(request);

//            if (response.StatusCode != HttpStatusCode.OK)
//            {
//                // Log error
//                throw new Exception(response.ErrorMessage ?? $"Publishing state, response was {response.StatusCode}");
//            }

//            _lastPublishTime = DateTime.Now;
//            _logger.Debug("State published");

//            return true;
//        }

//        public void QueryServer()
//        {
//            FetchRequested = true;
//        }

//        public void Stop()
//        {
//            _serverListener.Disconnect();
//            Stopped = true;
//        }


//        /// <summary>
//        ///     Stop streaming for the specified video stream identifier. The video stream identifier is there to ensure that the
//        ///     request is coming for the right stream
//        /// </summary>
//        /// <param name="command"></param>
//        public bool StopProgram(StreamingDeviceCommandModel command)
//        {
//            _logger.Debug("Stopping program for command {@command}", command);

//            var state = GetState();

//            switch (state.Status)
//            {
//                case StreamingDeviceStatuses.Idle:
//                    _logger.Error("Request to stop program against idle device (expected streaming program)");
//                    return false;
//                case StreamingDeviceStatuses.Streaming:
//                case StreamingDeviceStatuses.StreamingAndRecording:
//                case StreamingDeviceStatuses.Recording:
//                    _logger.Warning("Request to stop program against device without a running program. {@state}", state);
//                    return false;
//                case StreamingDeviceStatuses.StreamingProgram:
//                case StreamingDeviceStatuses.StreamingAndRecordingProgram:
//                case StreamingDeviceStatuses.RecordingProgram:
//                    state.Status = StreamingDeviceStatuses.Streaming;
//                    break;
//                case StreamingDeviceStatuses.Offline:
//                case StreamingDeviceStatuses.Error:
//                    throw new Exception($"Device with status {state.Status} cannot accept commands");
//                default:
//                    throw new ArgumentOutOfRangeException();
//            }

//            _deviceRepository.Save(state);

//            _logger.Debug("Program stopped.");

//            return true;
//        }

//        /// <summary>
//        ///     Stop streaming for the specified video stream identifier. The video stream identifier is there to ensure that the
//        ///     request is coming for the right stream
//        /// </summary>
//        /// <param name="command"></param>
//        public bool StopStreaming(StreamingDeviceCommandModel command)
//        {
//            _logger.Debug("Stopping streaming for command {@command}", command);

//            var state = GetState();

//            switch (state.Status)
//            {
//                case StreamingDeviceStatuses.Idle:
//                    _logger.Error("Request to stop streaming against idle device (expected streaming)");
//                    return false;
//                case StreamingDeviceStatuses.Streaming:
//                case StreamingDeviceStatuses.StreamingAndRecording:
//                case StreamingDeviceStatuses.Recording:
//                    //
//                    state.Status = StreamingDeviceStatuses.Idle;
//                    break;
//                case StreamingDeviceStatuses.StreamingProgram:
//                case StreamingDeviceStatuses.StreamingAndRecordingProgram:
//                case StreamingDeviceStatuses.RecordingProgram:
//                    _logger.Error("Request to stop stream against device running program");
//                    return false;
//                case StreamingDeviceStatuses.Offline:
//                case StreamingDeviceStatuses.Error:
//                    throw new Exception($"Device with status {state.Status} cannot accept commands");
//                default:
//                    throw new ArgumentOutOfRangeException();
//            }

//            state.ActiveVideoStreamId = null;
//            _deviceRepository.Save(state);

//            _logger.Debug("Streaming stopped.");

//            return true;
//        }



//        private void PublishTemporaryFailedState()
//        {
//            var state = GetState();
//            var status = state.Status;
//            if (status != StreamingDeviceStatuses.Error)
//            {
//                state.Status = StreamingDeviceStatuses.Error;
//                _deviceRepository.Save(state);
//            }
//            PublishState();
//            if (status != state.Status)
//            {
//                state.Status = status;
//                _deviceRepository.Save(state);
//            }
//        }



//        private void OnServerMessageReceived(string message)
//        {
//            if (message != "CommandAvailable") return;
//            FetchRequested = true;
//        }

//        private bool ResetToIdle(StreamingDeviceCommandModel command)
//        {
//            _logger.Debug("Resetting device to idle for command {@command}", command);

//            var state = GetState();

//            switch (state.Status)
//            {
//                case StreamingDeviceStatuses.Idle:
//                    // already idle, asssume it worked
//                    return true;
//                case StreamingDeviceStatuses.Streaming:
//                case StreamingDeviceStatuses.StreamingAndRecording:
//                case StreamingDeviceStatuses.Recording:
//                case StreamingDeviceStatuses.StreamingProgram:
//                case StreamingDeviceStatuses.StreamingAndRecordingProgram:
//                case StreamingDeviceStatuses.RecordingProgram:
//                    if (state.ActiveVideoStreamId != command.VideoStreamId)
//                        return false;
//                    //_streamingClient.ShutDown();
//                    state.Status = StreamingDeviceStatuses.Idle;
//                    break;
//                case StreamingDeviceStatuses.Offline:
//                case StreamingDeviceStatuses.Error:
//                    throw new Exception($"Device with status {state.Status} cannot accept commands");
//                default:
//                    throw new ArgumentOutOfRangeException();
//            }

//            _deviceRepository.Save(state);

//            _logger.Debug("Device reset to idle.");

//            return true;
//        }
//    }
//}