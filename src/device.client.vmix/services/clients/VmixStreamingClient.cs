using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;
using AutoMapper;
using forte.devices.models;
using forte.devices.models.presets;
using forte.services;
using RestSharp;

namespace forte.devices.services.clients
{
    public class VmixStreamingClient : IStreamingClient
    {
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
            _client = new RestClient(config.Get<string>(SettingParams.VmixApiPath));
        }

        private void LoadDefaultSettings()
        {
            var config = _configurationManager.GetDeviceConfig();
            if (string.IsNullOrWhiteSpace(config.Get<string>(SettingParams.VmixApiPath)))
                config = _configurationManager.UpdateSetting(SettingParams.VmixApiPath, "http://localhost:8088/api");
            if (string.IsNullOrWhiteSpace(config.Get<string>(SettingParams.VmixExePath)))
                config = _configurationManager.UpdateSetting(SettingParams.VmixExePath, @"d:\Program Files (x86)\vMix\vMix.exe");
            if (string.IsNullOrWhiteSpace(config.Get<string>(SettingParams.VmixPlaylistName)))
                config = _configurationManager.UpdateSetting(SettingParams.VmixPlaylistName, "CameraSwitchingProgram");
            if (string.IsNullOrWhiteSpace(config.Get<string>(SettingParams.VmixPresetTemplateFilePath)))
                config = _configurationManager.UpdateSetting(SettingParams.VmixPresetTemplateFilePath, @"C:\forte\preset\Forte Preset.vmix");
            if (string.IsNullOrWhiteSpace(config.Get<string>(SettingParams.BroadcastStartupImage)))
                config = _configurationManager.UpdateSetting(SettingParams.BroadcastStartupImage, "logo_dark_background.jpg");
            if (string.IsNullOrWhiteSpace(config.Get<string>(SettingParams.BroadcastStartupVideo)))
                config = _configurationManager.UpdateSetting(SettingParams.BroadcastStartupVideo, "logo_dark_background_mantis.mp4");
            if (string.IsNullOrWhiteSpace(config.Get<string>(SettingParams.BroadcastClosingImage)))
                config = _configurationManager.UpdateSetting(SettingParams.BroadcastClosingImage, "logo_girl_warrior_stance.jpg");
            if (string.IsNullOrWhiteSpace(config.Get<string>(SettingParams.BroadcastClosingVideo)))
                config = _configurationManager.UpdateSetting(SettingParams.BroadcastClosingVideo, "logo_reveal_house.mp4");
            if (string.IsNullOrWhiteSpace(config.Get<string>(SettingParams.BroadcastOverlayImage)))
                _configurationManager.UpdateSetting(SettingParams.BroadcastOverlayImage, "overlay_1280_720.png");
        }

        public StreamingClientState GetState()
        {
            var state = GetVmixState();
            if (state == null) return null;
            var clientState = Mapper.Map<StreamingClientState>(state);
            clientState.PresetLoadHash = GetVmixProcessHash();
            return clientState;
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
            return response.StatusCode != HttpStatusCode.OK ? null : MatchPresetStateRoles(response.Data);
        }

        public string LoadVideoStreamPreset(VideoStreamModel videoStream)
        {
            var presetIdentifier = LoadPreset(videoStream);
            if (presetIdentifier == null)
                throw new Exception("Could not load preset");
            return presetIdentifier;
        }

        void IStreamingClient.StartStreaming()
        {
            _logger.Debug("Loading static image intro...");

            var vmixState = GetVmixState();
            var openingVideo = vmixState.Inputs.Single(input => input.Role == InputRole.OpeninStaticImage);
            SetActive(openingVideo);

            _logger.Debug("Starting streaming...");

            StartStreaming();

            _logger.Debug("Starting started.");
        }

        public void StartProgram()
        {
            _logger.Debug("Starting intro video...");

            var vmixState = GetVmixState();

            var openingVideo = vmixState.Inputs.Single(input => input.Role == InputRole.OpeningVideo);
            SetPreview(openingVideo);

            // Fade to intro video
            FadeToPreview();

            // Set camera 1 at preview
            var cameraInput = vmixState.Inputs.First(input => input.Role == InputRole.Camera);
            SetPreview(cameraInput);
            _logger.Debug("Set camera 1 as preview.");

            Thread.Sleep(openingVideo.Duration);

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

        public void StopStreaming(bool shutdownClient)
        {
            _logger.Debug("Stopping streaming...");
            StopStreaming();
            _logger.Debug("Stopped streaming.");

            if (shutdownClient) StopVmix();
        }

        public void StopProgram()
        {
            _logger.Debug("Stopping program...");

            var vmixState = GetVmixState();

            var closingVideo = vmixState.Inputs.Single(input => input.Role == InputRole.ClosingVideo);
            SetPreview(closingVideo);
            _logger.Debug("Placed closing video in preview.");

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
            var closingImageInput = vmixState.Inputs.First(input => input.Role == InputRole.ClosingStaticImage);
            SetPreview(closingImageInput);
            _logger.Debug("Set closing image as preview.");

            Thread.Sleep(closingVideo.Duration);

            FadeToPreview();
            _logger.Debug("Switched to closing image.");
        }


        /// <summary>
        ///     Start streaming to the already selected channel
        /// </summary>
        /// <returns></returns>
        public VmixState StartStreaming()
        {
            var state = CallAndFetchState("/?Function=StartStreaming", "start streaming");

            var retries = 10;

            while (!state.Streaming && retries-- > 0)
            {
                Thread.Sleep(1000);
                state = GetVmixState();
            }

            if (!state.Streaming) throw new Exception("Could not start streaming");

            return state;
        }

        /// <summary>
        ///     Start streaming to the already selected channel
        /// </summary>
        /// <returns></returns>
        public VmixState StartRecording()
        {
            var state = CallAndFetchState("/?Function=StartRecording", "start recording");
            var retries = 10;

            while (!state.Recording && retries-- > 0)
            {
                Thread.Sleep(1000);
                state = GetVmixState();
            }

            if (!state.Recording) throw new Exception("Could not start recording");

            return state;
        }

        public VmixState StopStreaming()
        {
            return CallAndFetchState("/?Function=StopStreaming", "stop streaming");
        }

        public VmixState StopPlaylist()
        {
            return CallAndFetchState("/?Function=StopPlayList", "stop playlist");
        }

        public VmixState TurnOverlayOff(VmixState state = null)
        {
            state = state ?? GetVmixState();
            var overlayInput = state.Inputs.Single(input => input.Role == InputRole.LogoOverlay);
            return CallAndFetchState($"/?Function=OverlayInput1Off&Input={overlayInput.Key}", "turn overlay off");
        }

        public VmixState TurnAudioOff(VmixState state = null)
        {
            state = state ?? GetVmixState();
            var audioInputs = state.Inputs.Where(input => input.Role == InputRole.Audio).ToList();
            foreach (var audioInput in audioInputs)
            {
                state = CallAndFetchState($"/?Function=AudioOff&Input={audioInput.Key}", "audio off");
            }
            return state;
        }

        public VmixState StartPlaylist()
        {
            var config = _configurationManager.GetDeviceConfig();

            CallAndFetchState($"/?Function=SelectPlayList&Value={config.Get<string>(SettingParams.VmixPlaylistName)}",
                "set playlist");
            return CallAndFetchState("/?Function=StartPlayList", "start playlist");
        }

        public VmixState TurnOverlayOn(VmixState state = null)
        {
            state = state ?? GetVmixState();
            var overlayInput = state.Inputs.Single(input => input.Role == InputRole.LogoOverlay);
            return CallAndFetchState($"/?Function=OverlayInput1In&Input={overlayInput.Key}", "turn overlay on");
        }

        public VmixState TurnAudioOn(VmixState state = null)
        {
            state = state ?? GetVmixState();
            var audioInputs = state.Inputs.Where(input => input.Role == InputRole.Audio).ToList();
            foreach (var audioInput in audioInputs)
            {
                state = CallAndFetchState($"/?Function=AudioOn&Input={audioInput.Key}", "audio on");
            }
            return state;
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

        public VmixState FadeToPreview()
        {
            var result = CallAndFetchState("/?Function=Transition1", "fade to preview");
            Thread.Sleep(2000);
            return result;
        }

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

        public VmixState StopRecording()
        {
            return CallAndFetchState("/?Function=StopRecording", "stop recording");
        }

        private VmixState CallAndFetchState(string operation, string description)
        {
            try
            {
                var config = _configurationManager.GetDeviceConfig();
                var webRequest =
                    WebRequest.CreateHttp($"{config.Get<string>(SettingParams.VmixApiPath)}{operation}");
                var response = (HttpWebResponse) webRequest.GetResponse();
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

        /// <summary>
        ///     Match predefined roles from the preset state
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        private VmixState MatchPresetStateRoles(VmixState state)
        {
            if (state == null) return null;

            var config = _configurationManager.GetDeviceConfig();

            var input = state.Inputs.FirstOrDefault(i => i.Title == config.Get<string>(SettingParams.BroadcastStartupImage));
            if (input != null) input.Role = InputRole.OpeninStaticImage;

            input = state.Inputs.FirstOrDefault(i => i.Title == config.Get<string>(SettingParams.BroadcastStartupVideo));
            if (input != null) input.Role = InputRole.OpeningVideo;

            input = state.Inputs.FirstOrDefault(i => i.Title == config.Get<string>(SettingParams.BroadcastClosingImage));
            if (input != null) input.Role = InputRole.ClosingStaticImage;

            input = state.Inputs.FirstOrDefault(i => i.Title == config.Get<string>(SettingParams.BroadcastClosingVideo));
            if (input != null) input.Role = InputRole.ClosingVideo;

            input = state.Inputs.FirstOrDefault(i => i.Title == config.Get<string>(SettingParams.BroadcastOverlayImage));
            if (input != null) input.Role = InputRole.LogoOverlay;

            var audioInputs =
                state.Inputs.Where(
                    i => (i.Title.ToLower().Contains("audio") && i.Title.ToLower().Contains("microphone")) ||
                    i.Title.ToLower() == "microphone" || i.Title.ToLower() == "music");
            foreach (var audioInput in audioInputs)
            {
                audioInput.Role = InputRole.Audio;
            }

            state.Inputs.Where(i => _cameraRegex.IsMatch(i.Title)).ToList().ForEach(i => i.Role = InputRole.Camera);

            state.Active = state.Inputs.FirstOrDefault(i => i.Number == state.ActiveNumber);
            state.Preview = state.Inputs.FirstOrDefault(i => i.Number == state.PreviewNumber);

            return state;
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
            var presetTemplateFilePath = config.Get<string>(SettingParams.VmixExePath);

            if (!File.Exists(presetTemplateFilePath))
            {
                throw new Exception($"Cannot run vMix, file doesn't exist at '{presetTemplateFilePath}'");
            }

            var vMixProcess = Process.Start(new ProcessStartInfo
            {
                FileName = presetTemplateFilePath,
                WindowStyle = ProcessWindowStyle.Hidden
            });

            if (vMixProcess == null)
            {
                throw new Exception("Could not run vMix");
            }

            while (string.IsNullOrWhiteSpace(vMixProcess.MainWindowTitle))
            {
                Thread.Sleep(100);
                vMixProcess.Refresh();
            }

            return vMixProcess.Handle.ToString();
        }

        public void StopVmix()
        {
            var process = GetVmixProcess();
            process?.Kill();
        }

        public Process GetVmixProcess()
        {
            var config = _configurationManager.GetDeviceConfig();
            var presetTemplateFilePath = config.Get<string>(SettingParams.VmixExePath);
            FileSystemInfo fileInfo = new FileInfo(presetTemplateFilePath);
            var sExeName = fileInfo.Name.Replace(fileInfo.Extension, "");

            var existingProcess = Process.GetProcessesByName(sExeName).FirstOrDefault();

            return existingProcess;
        }

        /// <summary>
        ///     Load presets based on a preset file defined in the app config
        /// </summary>
        public string LoadPreset(VideoStreamModel videoStream)
        {
            var vmixProcessHandle = EnsureVmixIsRunning(startFresh: true);

            var config = _configurationManager.GetDeviceConfig();
            var presetTemplateFilePath = config.Get<string>(SettingParams.VmixPresetTemplateFilePath);
            var vmixPresetOutputFolder = config.Get<string>(SettingParams.VmixPresetFolderPath) ?? Path.GetTempPath();
            var timeStamp = DateTime.UtcNow.ToString(CultureInfo.InvariantCulture).Replace(":", "").Replace("/", "");
            var vmixPresetOutputFile = $"{timeStamp}-{Guid.NewGuid()}.vmix";
            vmixPresetOutputFile = Path.Combine(vmixPresetOutputFolder, vmixPresetOutputFile);

            var vmixPreset = VmixPreset.FromFile(presetTemplateFilePath);
            vmixPreset.Outputs = new List<VmixStreamDestination>
            {
                new VmixStreamDestination("Primary", videoStream.PrimaryIngestUrl),
                new VmixStreamDestination("Secondary", videoStream.SecondaryIngestUrl)
            };
            vmixPreset.ToFile(vmixPresetOutputFile);

            var requestUrl = $"/?Function=OpenPreset&Value={vmixPresetOutputFile}";
            var request = new RestRequest(requestUrl, Method.GET)
            {
                Timeout = 1
            };
            _client.Execute<VmixState>(request);

            const int fiveSeconds = 5000;
            const int twoMinutes = 120;
            var timeout = config.Get(SettingParams.VmixLoadTimeout, defaultValue: twoMinutes);
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

        private bool IsPresetLoaded(VmixPreset preset)
        {
            var state = GetVmixState();
            if (state == null || state.Inputs.Count != preset.Inputs.Count) return false;
            foreach (var input in preset.Inputs)
            {
                if (state.Inputs.All(i => 
                    !input.OriginalTitle.Contains(i.Title) && 
                    (input.Title == null || !input.Title.Contains(i.Title)) && 
                    !i.Title.Contains(input.OriginalTitle)))
                    return false;
            }
            return true;
        }
    }
}