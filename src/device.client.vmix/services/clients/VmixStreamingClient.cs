﻿#region

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading;
using AutoMapper;
using forte.devices.models;
using forte.devices.models.presets;
using forte.services;
using Polly;
using RestSharp;

#endregion

namespace forte.devices.services.clients
{
    public class VmixStreamingClient : IStreamingClient
    {
        const uint WM_CLOSE = 0x0010;

        //private readonly Regex _cameraRegex = new Regex(@"Webcam");
        private readonly Regex _cameraRegex = new Regex(@"RTSPTCP rtsp:\/\/root:pass@[0-9.]*\/axis-media\/media\.amp");
        private readonly RestClient _client;
        private readonly IConfigurationManager _configurationManager;
        private readonly ILogger _logger;

        public VmixStreamingClient(IConfigurationManager configurationManager, ILogger logger)
        {
            _configurationManager = configurationManager;
            _logger = logger;
            LoadDefaultSettings();
            var config = _configurationManager.GetDeviceConfig();
            _client = new RestClient(config.Get<string>(VmixSettingParams.VmixApiPath));
        }

        public StreamingClientState GetState()
        {
            var state = GetVmixState();
            if (state == null) return null;
            var clientState = Mapper.Map<StreamingClientState>(state);
            clientState.PresetLoadHash = GetVmixProcessHash();
            return clientState;
        }

        public string LoadVideoStreamPreset(VideoStreamModel videoStream, string preset)
        {
            var presetIdentifier = LoadPreset(videoStream, preset);
            if (presetIdentifier == null)
                throw new Exception("Could not load preset");
            return presetIdentifier;
        }

        public void ShutDown()
        {
            _logger.Debug("Shutting down VMIX");
            var state = GetVmixState();
            if (state == null) return;
            if (state.Streaming)
                StopStreaming();
            StopVmix();
        }

        public void StartProgram(DateTime? time = null)
        {
            _logger.Debug("Starting intro video...");

            var config = _configurationManager.GetDeviceConfig();

			var enableIntro = config.Get<bool>(VmixSettingParams.EnableIntro);
			_logger.Debug($"Starting intro video... Enable: {enableIntro}");

			var vmixState = GetVmixState();
			var openingVideo = vmixState.Inputs.Single(input => input.Role == InputRole.OpeningVideo);

			var mils = 0;
			var now = DateTime.UtcNow;
			if(time != null && time > now)
			{
				mils = (int) (time.Value - now).TotalMilliseconds;

				_logger.Debug($"Start program: Wait for class start {time.Value}, miliseconds to start: {mils}.");
				Thread.Sleep(mils);
			}

			if(enableIntro)
			{
				SetPreview(openingVideo);

				// Fade to intro video
				FadeToPreview();
			}

			// Set camera 1 at preview
			var cameraInput = vmixState.Inputs.First(input => input.Role == InputRole.Camera);
            SetPreview(cameraInput);
            _logger.Debug("Set camera 1 as preview.");

			if(enableIntro)
			{
				Thread.Sleep(openingVideo.Duration);
			}

            FadeToPreview();
            _logger.Debug("Switched to camera 1 video.");

            // Turn on audio (possibly fade)
            TurnAudioOn(vmixState);
            _logger.Debug("Turned audio on.");

            // Add logo overlay
            TurnOverlayOn();
            _logger.Debug("Turned logo overlay on.");

            // Activate playlist
            StartPlaylist();
            _logger.Debug("Started the playlist");
        }

        void IStreamingClient.StartStreaming(DateTime? time = null)
        {
            var vmixState = GetVmixState();
            var config = _configurationManager.GetDeviceConfig();

            _logger.Debug("Loading static image intro...");
            var openingImage = vmixState.Inputs.Single(input => input.Role == InputRole.OpeninStaticImage);

			if(time != null)
			{
				var timeSecondsConfig = config.Get(VmixSettingParams.StaticImageTime, 30);
				_logger.Debug($"Seconds for static image: {timeSecondsConfig}");

				var mils = 0;
				var startTime = time.Value.AddSeconds(-timeSecondsConfig);
				var now = DateTime.UtcNow;
				if(startTime > now)
				{
					mils = (int) (startTime - now).TotalMilliseconds;

					_logger.Debug($"Start stream: Wait for class start {time.Value}, static image start: {startTime}, miliseconds to start: {mils}.");
					Thread.Sleep(mils);
				}
			}

			_logger.Debug("Ensuring audio is off...");
			TurnAudioOff(vmixState);

			SetActive(openingImage);

            _logger.Debug("Starting streaming...");

            var retryCountForStreamException = config.Get(VmixSettingParams.RetryCountForStreamException, 0);
            Policy.Handle<Exception>()
                .WaitAndRetry(retryCountForStreamException, attempt => TimeSpan.FromSeconds(5))
                .Execute(StartStreaming);

            _logger.Debug("Starting started.");
        }

        public void StopProgram(DateTime? time = null)
        {
            _logger.Debug("Stopping program...");

            var vmixState = GetVmixState();
			var config = _configurationManager.GetDeviceConfig();

			var enableOutro = config.Get<bool>(VmixSettingParams.EnableOutro);
			_logger.Debug($"Outro video... Enable: {enableOutro}");
			var enableOutroStatic = config.Get<bool>(VmixSettingParams.EnableOutroStatic);
			_logger.Debug($"Outro video static image... Enable: {enableOutroStatic}");

			var closingVideo = vmixState.Inputs.Single(input => input.Role == InputRole.ClosingVideo);
			if(enableOutro)
			{
				SetPreview(closingVideo);
				_logger.Debug("Placed closing video in preview.");
			}

            // Turn off audio (possibly fade)
            TurnAudioOff(vmixState);
            _logger.Debug("Turned audio off.");

            // Remove logo overlay
            TurnOverlayOff(vmixState);
            _logger.Debug("Turned logo overlay off.");

            // Stop playlist
            StopPlaylist();
            _logger.Debug("Stopped the playlist");

            // Fade to intro video
            FadeToPreview();
            _logger.Debug("Switched to closing video.");

			// Set ending background image as preview
			if(enableOutroStatic)
			{
				var closingImageInput = vmixState.Inputs.First(input => input.Role == InputRole.ClosingStaticImage);
				SetPreview(closingImageInput);
				_logger.Debug("Set closing image as preview.");
			}

			if(enableOutro)
			{
				Thread.Sleep(closingVideo.Duration);
			}

			if(enableOutroStatic)
			{
				FadeToPreview();
				_logger.Debug("Switched to closing image.");
			}

			if(!enableOutroStatic)
			{
				_logger.Debug("No outro closing image.");
				StopStreaming(true);
			}
        }

        public void StopStreaming(bool shutdownClient)
        {
			var state = GetVmixState();
			if(state == null) return;
			if(!state.Streaming) return;

			_logger.Debug("Stopping streaming...");
            StopStreaming();
            _logger.Debug("Stopped streaming.");

            if (shutdownClient) StopVmix();
        }

        private VmixState CallAndFetchState(string operation, string description)
        {
            try
            {
                var config = _configurationManager.GetDeviceConfig();
                var webRequest =
                    WebRequest.CreateHttp($"{config.Get<string>(VmixSettingParams.VmixApiPath)}{operation}");
                var response = (HttpWebResponse)webRequest.GetResponse();
                _logger.Debug("VMIX command HTTP response {@response}", response);
                if (response.StatusCode == HttpStatusCode.OK) return GetVmixState();

                var error = $"Could not {description} ({response.StatusDescription}";
                _logger.Error(error);
                throw new Exception(error);
            }
            catch (Exception exception)
            {
                _logger.Error(exception, "Calling vMix API failed for operation {@operation}", operation);
                throw;
            }
        }

        public bool CloseModalErrorWindowIfOpen()
        {
            var windowPtr = FindWindowByCaption(IntPtr.Zero, "vMix Error");
            if (windowPtr == IntPtr.Zero)
                return false;
            SendMessage(windowPtr, WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
            return true;
        }

        private string EnsureVmixIsRunning(bool startFresh)
        {
            var existingProcess = GetVmixProcess();
            if (existingProcess != null)
            {
                if (!startFresh) return existingProcess.Handle.ToString();
                StopVmix();
            }

            var config = _configurationManager.GetDeviceConfig();
            var presetTemplateFilePath = config.Get<string>(VmixSettingParams.VmixExePath);

            if (!File.Exists(presetTemplateFilePath))
                throw new Exception($"Cannot run vMix, file doesn't exist at '{presetTemplateFilePath}'");

            var vMixProcess = Process.Start(new ProcessStartInfo
            {
                FileName = presetTemplateFilePath,
                WindowStyle = ProcessWindowStyle.Hidden
            });

            if (vMixProcess == null)
                throw new Exception("Could not run vMix");

            while (string.IsNullOrWhiteSpace(vMixProcess.MainWindowTitle))
            {
                Thread.Sleep(100);
                vMixProcess.Refresh();
            }

            return vMixProcess.Handle.ToString();
        }

        public VmixState FadeToPreview()
        {
            var result = CallAndFetchState("/?Function=Transition1", "fade to preview");
            Thread.Sleep(2000);
            return result;
        }

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        /// <summary>
        ///     Find window by Caption only. Note you must pass IntPtr.Zero as the first parameter.
        /// </summary>
        [DllImport("user32.dll", EntryPoint = "FindWindow", SetLastError = true)]
        static extern IntPtr FindWindowByCaption(IntPtr ZeroOnly, string lpWindowName);

        public Process GetVmixProcess()
        {
            var config = _configurationManager.GetDeviceConfig();
            var presetTemplateFilePath = config.Get<string>(VmixSettingParams.VmixExePath);
            FileSystemInfo fileInfo = new FileInfo(presetTemplateFilePath);
            var sExeName = fileInfo.Name.Replace(fileInfo.Extension, "");

            var existingProcess = Process.GetProcessesByName(sExeName).FirstOrDefault();

            return existingProcess;
        }

        private string GetVmixProcessHash()
        {
            var vmixProcess = GetVmixProcess();
            return vmixProcess?.Handle.ToString();
        }

        private VmixState GetVmixState()
        {
            var request = new RestRequest("", Method.GET);
            var response = _client.Execute<VmixState>(request);
            //_logger.Debug("VMIX state {@state}", response);
            return response.StatusCode != HttpStatusCode.OK ? null : MatchPresetStateRoles(response.Data);
        }

        private bool IsPresetLoaded(VmixPreset preset)
        {
            var state = GetVmixState();
            if (state == null || state.Inputs.Count != preset.Inputs.Count) return false;
            foreach (var input in preset.Inputs)
                if (state.Inputs.All(i =>
                    !input.OriginalTitle.Contains(i.Title) &&
                    (input.Title == null || !input.Title.Contains(i.Title)) &&
                    !i.Title.Contains(input.OriginalTitle)))
                    return false;
            return true;
        }

        [DllImport("user32")]
        private static extern int IsWindowEnabled(int hWnd);

		private void LoadDefaultSettings()
		{
			var config = _configurationManager.GetDeviceConfig();
			if(string.IsNullOrWhiteSpace(config.Get<string>(VmixSettingParams.VmixApiPath)))
				config = _configurationManager.UpdateSetting(VmixSettingParams.VmixApiPath, "http://localhost:8088/api");
			if(string.IsNullOrWhiteSpace(config.Get<string>(VmixSettingParams.VmixExePath)))
				config = _configurationManager.UpdateSetting(VmixSettingParams.VmixExePath, @"C:\Program Files (x86)\vMix\vMix64.exe");
			if(string.IsNullOrWhiteSpace(config.Get<string>(VmixSettingParams.VmixPlaylistName)))
				config = _configurationManager.UpdateSetting(VmixSettingParams.VmixPlaylistName, "CameraSwitchingProgram");
			if(string.IsNullOrWhiteSpace(config.Get<string>(VmixSettingParams.VmixPresetTemplateFilePath)))
				config = _configurationManager.UpdateSetting(VmixSettingParams.VmixPresetTemplateFilePath, @"C:\forte\preset\Forte Preset.vmix");
			if(string.IsNullOrWhiteSpace(config.Get<string>(VmixSettingParams.BroadcastStartupImage)))
				config = _configurationManager.UpdateSetting(VmixSettingParams.BroadcastStartupImage, "logo_dark_background.jpg");
			if(string.IsNullOrWhiteSpace(config.Get<string>(VmixSettingParams.BroadcastStartupVideo)))
				config = _configurationManager.UpdateSetting(VmixSettingParams.BroadcastStartupVideo, "logo_dark_background_mantis.mp4");
			if(string.IsNullOrWhiteSpace(config.Get<string>(VmixSettingParams.BroadcastClosingImage)))
				config = _configurationManager.UpdateSetting(VmixSettingParams.BroadcastClosingImage, "logo_girl_warrior_stance.jpg");
			if(string.IsNullOrWhiteSpace(config.Get<string>(VmixSettingParams.BroadcastClosingVideo)))
				config = _configurationManager.UpdateSetting(VmixSettingParams.BroadcastClosingVideo, "logo_reveal_house.mp4");
			if(string.IsNullOrWhiteSpace(config.Get<string>(VmixSettingParams.BroadcastOverlayImage)))
				_configurationManager.UpdateSetting(VmixSettingParams.BroadcastOverlayImage, "overlay_1280_720.png");
			if(!config.Contains(VmixSettingParams.AutoCloseVmixErrorDialog))
				_configurationManager.UpdateSetting(VmixSettingParams.AutoCloseVmixErrorDialog, false);
			if(!config.Contains(VmixSettingParams.RetryCountForStreamException))
				_configurationManager.UpdateSetting(VmixSettingParams.RetryCountForStreamException, 0);

			if(!config.Contains(VmixSettingParams.EnableIntro))
				_configurationManager.UpdateSetting(VmixSettingParams.EnableIntro, true);
			if(!config.Contains(VmixSettingParams.EnableOutro))
				_configurationManager.UpdateSetting(VmixSettingParams.EnableOutro, true);
			if(!config.Contains(VmixSettingParams.EnableOutroStatic))
				_configurationManager.UpdateSetting(VmixSettingParams.EnableOutroStatic, true);
			if(!config.Contains(VmixSettingParams.StaticImageTime))
				_configurationManager.UpdateSetting(VmixSettingParams.StaticImageTime, 30);
		}

        /// <summary>
        ///     Load presets based on a preset file defined in the app config
        /// </summary>
        public string LoadPreset(VideoStreamModel videoStream, string preset)
        {
            var vmixProcessHandle = EnsureVmixIsRunning(startFresh: true);

            var config = _configurationManager.GetDeviceConfig();

			var presetTemplateFilePath = config.Get<string>(VmixSettingParams.VmixPresetTemplateFilePath);
			_logger.Debug($"Custom preset? {preset}");
			if(!string.IsNullOrEmpty(preset))
			{
				_logger.Debug($"Using preset {preset} instead of {presetTemplateFilePath}");
				if(!preset.StartsWith("Test"))
				{
					presetTemplateFilePath = preset;
				}
			}

            var vmixPresetOutputFolder = config.Get<string>(VmixSettingParams.VmixPresetFolderPath);
            if (string.IsNullOrWhiteSpace(vmixPresetOutputFolder)) vmixPresetOutputFolder = Path.GetTempPath();
            _logger.Debug("Using preset output path {@vmixPresetOutputFolder}", vmixPresetOutputFolder);
            var timeStamp = DateTime.UtcNow.ToString(CultureInfo.InvariantCulture).Replace(":", "").Replace("/", "");
            var vmixPresetOutputFile = $"{timeStamp}-{Guid.NewGuid()}.vmix";
            vmixPresetOutputFile = Path.Combine(vmixPresetOutputFolder, vmixPresetOutputFile);

            var vmixPreset = VmixPreset.FromFile(presetTemplateFilePath);
            vmixPreset.Outputs = new List<VmixStreamDestination>
            {
                new VmixStreamDestination("Primary", videoStream.PrimaryIngestUrl),
                //new VmixStreamDestination("Secondary", videoStream.SecondaryIngestUrl)
            };
            //_logger.Debug("Saving preset file {@vmixPresetOutputFile} for {@vmixPreset}", vmixPresetOutputFile, vmixPreset);
            _logger.Debug("Saving preset file {@vmixPresetOutputFile}", vmixPresetOutputFile);
            vmixPreset.ToFile(vmixPresetOutputFile);

            var requestUrl = $"/?Function=OpenPreset&Value={vmixPresetOutputFile}";
            var request = new RestRequest(requestUrl, Method.GET)
            {
                Timeout = 1
            };
            var response = _client.Execute<VmixState>(request);
            _logger.Debug("Open preset command HTTP resposne {@response}", response);

            const int fiveSeconds = 5000;
            const int twoMinutes = 120;
            var timeout = config.Get(VmixSettingParams.VmixLoadTimeout, defaultValue: twoMinutes);
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            while (!IsPresetLoaded(vmixPreset))
            {
                if (stopwatch.Elapsed.TotalSeconds > timeout)
                    throw new Exception($"Preset load timed out, could not load within {timeout} seconds.");
                Thread.Sleep(fiveSeconds);
            }

            return vmixProcessHandle;
        }

        /// <summary>
        ///     Match predefined roles from the preset state
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        private VmixState MatchPresetStateRoles(VmixState state)
        {
            if (state == null) return null;

            var config = _configurationManager.GetDeviceConfig();

            var input = state.Inputs.FirstOrDefault(i => i.Title == config.Get<string>(VmixSettingParams.BroadcastStartupImage));
            if (input != null) input.Role = InputRole.OpeninStaticImage;

            input = state.Inputs.FirstOrDefault(i => i.Title == config.Get<string>(VmixSettingParams.BroadcastStartupVideo));
            if (input != null) input.Role = InputRole.OpeningVideo;

            input = state.Inputs.FirstOrDefault(i => i.Title == config.Get<string>(VmixSettingParams.BroadcastClosingImage));
            if (input != null) input.Role = InputRole.ClosingStaticImage;

            input = state.Inputs.FirstOrDefault(i => i.Title == config.Get<string>(VmixSettingParams.BroadcastClosingVideo));
            if (input != null) input.Role = InputRole.ClosingVideo;

            input = state.Inputs.FirstOrDefault(i => i.Title == config.Get<string>(VmixSettingParams.BroadcastOverlayImage));
            if (input != null) input.Role = InputRole.LogoOverlay;

            var audioInputs = state.Inputs.Where(
                    i => i.Title.ToLower().Contains("audio") || i.Title.ToLower().Contains("microphone") ||
                         i.Title.ToLower() == "microphone" || i.Title.ToLower() == "music");

            foreach (var audioInput in audioInputs)
                audioInput.Role = InputRole.Audio;

            state.Inputs.Where(i => _cameraRegex.IsMatch(i.Title)).ToList().ForEach(i => i.Role = InputRole.Camera);

            state.Active = state.Inputs.FirstOrDefault(i => i.Number == state.ActiveNumber);
            state.Preview = state.Inputs.FirstOrDefault(i => i.Number == state.PreviewNumber);

            return state;
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        /// <summary>
        ///     Set the preview window to the specified input
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public VmixState SetActive(VmixInput input)
        {
            return CallAndFetchState($"/?Function=ActiveInput&Input={input.Key}", "set active");
        }

        /// <summary>
        ///     Set the preview window to the specified input
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public VmixState SetOverlay(VmixInput input)
        {
            return CallAndFetchState($"/?Function=OverlayInput1&Input={input.Key}", "set overlay");
        }

        /// <summary>
        ///     Set the preview window to the specified input
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public VmixState SetPreview(VmixInput input)
        {
            return CallAndFetchState($"/?Function=PreviewInput&Input={input.Key}", "set preview");
        }

        public VmixState StartPlaylist()
        {
            var config = _configurationManager.GetDeviceConfig();

            CallAndFetchState(
                $"/?Function=SelectPlayList&Value={config.Get<string>(VmixSettingParams.VmixPlaylistName)}",
                "set playlist");
            return CallAndFetchState("/?Function=StartPlayList", "start playlist");
        }

        /// <summary>
        ///     Start streaming to the already selected channel
        /// </summary>
        /// <returns></returns>
        public VmixState StartRecording()
        {
            var state = CallAndFetchState("/?Function=StartRecording", "start recording");
            if (!state.Recording) throw new Exception("Could not start recording");
            return state;
        }

        /// <summary>
        ///     Start streaming to the already selected channel
        /// </summary>
        /// <returns></returns>
        public VmixState StartStreaming()
        {
            var config = _configurationManager.GetDeviceConfig();
            CallAndFetchState("/?Function=StartStreaming", "start streaming");
            Thread.Sleep(5000);
            var state = GetVmixState();
            if (config.Get(VmixSettingParams.AutoCloseVmixErrorDialog, false))
            {
                // When streaming fails, API returns true, but an error dialog is displayed, so make sure no dialogs open
                CloseModalErrorWindowIfOpen();
                Thread.Sleep(1000);
            }
            if (state.Streaming && !VmixDialogsOpen()) return state;
            throw new Exception("Could not start streaming");
        }

        public VmixState StopPlaylist()
        {
            return CallAndFetchState("/?Function=StopPlayList", "stop playlist");
        }

        public VmixState StopRecording()
        {
            return CallAndFetchState("/?Function=StopRecording", "stop recording");
        }

        public VmixState StopStreaming()
        {
            return CallAndFetchState("/?Function=StopStreaming", "stop streaming");
        }

        public void StopVmix()
        {
            var process = GetVmixProcess();
            process?.Kill();
        }

        public VmixState TurnAudioOff(VmixState state = null)
        {
            state = state ?? GetVmixState();
            var audioInputs = state.Inputs.Where(input => input.Role == InputRole.Audio).ToList();
            foreach (var audioInput in audioInputs)
                state = CallAndFetchState($"/?Function=AudioOff&Input={audioInput.Key}", "audio off");
            return state;
        }

        public VmixState TurnAudioOn(VmixState state = null)
        {
            state = state ?? GetVmixState();
            var audioInputs = state.Inputs.Where(input => input.Role == InputRole.Audio).ToList();
            foreach (var audioInput in audioInputs)
                state = CallAndFetchState($"/?Function=AudioOn&Input={audioInput.Key}", "audio on");
            return state;
        }

        public VmixState TurnOverlayOff(VmixState state = null)
        {
            state = state ?? GetVmixState();
            var overlayInput = state.Inputs.Single(input => input.Role == InputRole.LogoOverlay);
            return CallAndFetchState($"/?Function=OverlayInput1Off&Input={overlayInput.Key}", "turn overlay off");
        }

        public VmixState TurnOverlayOn(VmixState state = null)
        {
            state = state ?? GetVmixState();
            var overlayInput = state.Inputs.Single(input => input.Role == InputRole.LogoOverlay);
            return CallAndFetchState($"/?Function=OverlayInput1In&Input={overlayInput.Key}", "turn overlay on");
        }

        private bool VmixDialogsOpen()
        {
            var vmixProcess = GetVmixProcess();
            if (vmixProcess == null) throw new Exception("VMix process is not running");
            var vmixWindowEnabled = IsWindowEnabled(vmixProcess.MainWindowHandle.ToInt32()) != 0;
            // VMix Window will be disabled, if there are dialogs showing
            return !vmixWindowEnabled;
        }
    }
}