using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
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
		private ConcurrentDictionary<Guid, SessionState> _lockedSessions = new ConcurrentDictionary<Guid, SessionState>();

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
			_deviceId = config.Get<string>(SettingParams.DeviceId);
			_client = _client ?? new RestClient($"{apiPath}/streams/");
			_clientDevice = _clientDevice ?? new RestClient($"{apiPath}/devices/");

			Run();
		}

		private async void Run()
		{
			var config = _configurationManager.GetDeviceConfig();
			var linkRetrySeconds = 15;
			var streamRetrySeconds = 15;
			var streamStopRetrySeconds = 15;
			var linkTimeSeconds = 10 * 60;
			var staticImageSeconds = config.Get(VmixSettingParams.StaticImageTime, 30);
			var serverStreamSeconds = staticImageSeconds + 10;
			var sessions = _dbRepository.GetSessions();
			var seconds = 0;
			double overdue = 0;

			while(true)
			{
				await Task.Delay(1000);

				if(seconds != 0)
				{
					seconds--;
				}
				else if(seconds == 0)
				{
					StreamingDeviceCommandModel command = null;
					try
					{
						command = ThreadSafeFetchCommand();
					}
					catch(Exception exception)
					{
						_logger.Error(exception, exception.Message);
					}

					//executer pending commands before processing further
					if(command != null)
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
					else
					{
						seconds = 10;
					}
				}

				sessions = sessions.Where(s => s.Status != WorkflowState.Processed).OrderBy(s => s.StartTime).ToList();
				if(!sessions.Any()) continue;

				var vmixSession = sessions.FirstOrDefault(s => s.VmixUsed == true);
				foreach(var s in sessions)
				{
					if(_lockedSessions.ContainsKey(s.SessioId)) continue;
					//check if vmix is free to use for this session and it's time
					var isVmixSession = (vmixSession == null || vmixSession.Id == s.Id);
					var needsLinking = s.StartTime.AddSeconds(-linkTimeSeconds) <= DateTime.UtcNow;
					if(isVmixSession)
					{
						SetSessionVmix(s, needsLinking);
					}

					//terminate outdated session
					if(s.EndTime <= DateTime.UtcNow || s.Status == WorkflowState.CancelPending)
					{
						_logger.Warning($"{s.Permalink}: {(DateTime.UtcNow - s.EndTime).TotalSeconds} seconds after end, terminate");
						_lockedSessions[s.SessioId] = s;
						StopStream(s, streamStopRetrySeconds);
						continue;
					}

					//start new session
					if(s.Status == WorkflowState.Idle && needsLinking)
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
						//if(!fakeRun)
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
						//if(!fakeRun)
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
						//if(!fakeRun)
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
						_lockedSessions.TryRemove(s.SessioId, out var t);
					}
				});
			}
			catch(Exception ex)
			{
				ReportError(ex, s, "Link Stream");
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
						_lockedSessions.TryRemove(s.SessioId, out var t);
					}
				});
			}
			catch(Exception ex)
			{
				ReportError(ex, s, "Start Stream");
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

				var url = $"streamStop/{s.SessioId}?retryNum={s.RetryCount}&deviceId={_deviceId}&requestRef={s.Permalink}&deleteAsset={s.IsCancelled == true}";
				var request = new RestRequest(url, Method.GET);
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
						_lockedSessions.TryRemove(s.SessioId, out var t);
					}
				});
			}
			catch(Exception ex)
			{
				ReportError(ex, s, "Stop Stream");
			}
		}

		private void ReportError(Exception ex, SessionState s, string name)
		{
			_logger.Debug(ex, "");
			try
			{
				_lockedSessions.TryRemove(s.SessioId, out var t);
				ReportError(s, ex, name);
			}
			catch(Exception ex1)
			{
				_logger.Debug(ex1, "");
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

		private StreamingDeviceCommandModel ThreadSafeFetchCommand()
		{
			var request = new RestRequest($"{_deviceId}/commands/next", Method.GET);
			var response = _clientDevice.Execute(request);

			// Not found if no command
			if(response.StatusCode == HttpStatusCode.NotFound)
			{
				//_logger.Debug("No command available, exiting");
				// ... so just exit
				return null;
			}

			if(response.StatusCode != HttpStatusCode.OK)
			{
				_logger.Error("Failed fetching command, response {@response}", response);
				return null;
			}

			_logger.Debug("Command retrieved {@command}", response.Content);
			return JsonConvert.DeserializeObject<StreamingDeviceCommandModel>(response.Content);
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
					Status = WorkflowState.Idle,
					SessionType = (SessionType) command.Type
				};
			}

			state.StartTime = command.TimeStart.Value;
			state.EndTime = command.TimeEnd.Value;
			state.VmixPreset = command.Preset;

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
			if(state != null)
			{
				state.IsCancelled = true; 
				state.Status = WorkflowState.CancelPending;
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