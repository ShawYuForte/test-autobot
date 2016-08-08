using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using forte.devices.models;
using forte.devices.models.presets;
using RestSharp;

namespace forte.devices.services.clients
{
    public class VmixStreamingClient : IStreamingClient
    {
        private readonly Regex _cameraRegex = new Regex(@"RTSPTCP rtsp:\/\/root:pass@[0-9.]*\/axis-media\/media\.amp");
        private readonly RestClient _client;
        private readonly IConfigurationManager _configurationManager;

        public VmixStreamingClient(IConfigurationManager configurationManager)
        {
            _configurationManager = configurationManager;
            _client = new RestClient(ConfigurationManager.AppSettings["vmix:api"] ?? "http://localhost:8088/api");
        }

        public StreamingClientState GetState()
        {
            var state = GetVmixState();
            var vmixState = MatchPresetStateRoles(state);
            return VmixClientModule.Registrar.CreateMapper().Map<StreamingClientState>(vmixState);
        }

        private VmixState GetVmixState()
        {
            var request = new RestRequest("", Method.GET);
            var response = _client.Execute<VmixState>(request);
            return response.StatusCode != System.Net.HttpStatusCode.OK ? null : response.Data;
        }

        public void LoadVideoStreamPreset(VideoStreamModel videoStream)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        ///     Match predefined roles from the preset state
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        private VmixState MatchPresetStateRoles(VmixState state)
        {
            var preset = new StreamingPreset();
            
            var input = state.Inputs.FirstOrDefault(i => i.Title == preset.StartupImage.Title);
            if (input != null) input.Role = InputRole.OpeninStaticImage;

            //input = state.Inputs.FirstOrDefault(i => i.Title == preset.StartupVideo);
            //if (input != null) input.Role = InputRole.OpeningVideo;

            //input = state.Inputs.FirstOrDefault(i => i.Title == AppSettings.Instance.ClosingImage);
            //if (input != null) input.Role = InputRole.ClosingStaticImage;

            //input = state.Inputs.FirstOrDefault(i => i.Title == AppSettings.Instance.ClosingVideo);
            //if (input != null) input.Role = InputRole.ClosingVideo;

            //input = state.Inputs.FirstOrDefault(i => i.Title == AppSettings.Instance.OverlayImage);
            //if (input != null) input.Role = InputRole.LogoOverlay;

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

        private void EnsureVmixIsRunning()
        {
            if (GetVmixProcess() != null) return;

            var config = _configurationManager.GetDeviceConfig();
            var presetTemplateFilePath = config.Get<string>("VmixExecutablePath");

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
        }

        public void StopVmix()
        {
            var process = GetVmixProcess();
            process?.Kill();
        }

        public Process GetVmixProcess()
        {
            var config = _configurationManager.GetDeviceConfig();
            var presetTemplateFilePath = config.Get<string>("VmixExecutablePath");
            FileSystemInfo fileInfo = new FileInfo(presetTemplateFilePath);
            var sExeName = fileInfo.Name.Replace(fileInfo.Extension, "");

            var existingProcess = Process.GetProcessesByName(sExeName).FirstOrDefault();

            return existingProcess;
        }

        /// <summary>
        ///     Load presets based on a preset file defined in the app config
        /// </summary>
        public bool LoadPreset(VideoStreamModel videoStream)
        {
            EnsureVmixIsRunning();

            var config = _configurationManager.GetDeviceConfig();
            var presetTemplateFilePath = config.Get<string>("VmixPresetTemplateFilePath");
            var vmixPresetOutputFolder = config.Get<string>("VmixPresetFilePath") ?? Path.GetTempPath();
            var timeStamp = DateTime.UtcNow.ToString(CultureInfo.InvariantCulture).Replace(":", "").Replace("/", "");
            var vmixPresetOutputFile =
                $"{timeStamp}-{Guid.NewGuid()}.vmix";
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

            const int fixeSeconds = 5000;
            const int sixtySeconds = 60;
            var timeout = config.Get("VmixLoadTimeout", defaultValue: sixtySeconds);
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            while (!IsPresetLoaded(vmixPreset))
            {
                if (stopwatch.Elapsed.TotalSeconds > timeout)
                    throw new Exception($"Preset load timed out, could not load within {timeout} seconds.");
                Thread.Sleep(fixeSeconds);
            }

            return true;
        }

        private bool IsPresetLoaded(VmixPreset preset)
        {
            var state = GetVmixState();
            if (state.Inputs.Count != preset.Inputs.Count) return false;
            foreach (var input in preset.Inputs)
            {
                if (state.Inputs.All(i => !input.OriginalTitle.Contains(i.Title) && !i.Title.Contains(input.OriginalTitle))) return false;
            }
            return true;
        }
    }
}