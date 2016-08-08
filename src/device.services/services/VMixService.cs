﻿#region

using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;
using forte.device.models;
using forte.device.services;
using forte.devices.models;
using RestSharp;

#endregion

namespace forte.devices.services
{
    public class VMixService : Service
    {
        private readonly Regex _cameraRegex = new Regex(@"RTSPTCP rtsp:\/\/root:pass@[0-9.]*\/axis-media\/media\.amp");
        private readonly RestClient _client;

        public static VMixService Instance { get; } = new VMixService();

        private VMixService() : base("vMix")
        {
            _client = new RestClient(ConfigurationManager.AppSettings["apiPath"]);
        }

        /// <summary>
        ///     Fetch state from vMix API
        /// </summary>
        /// <returns></returns>
        public VMixState FetchState()
        {
            var request = new RestRequest("", Method.GET);
            var response = _client.Execute<VMixState>(request);

            return MatchPresetStateRoles(response.Data);
        }

        /// <summary>
        ///     Match predefined roles from the preset state
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        private VMixState MatchPresetStateRoles(VMixState state)
        {
            var input = state.Inputs.FirstOrDefault(i => i.Title == AppSettings.Instance.StartupImage);
            if (input != null) input.Role = InputRole.OpeninStaticImage;

            input = state.Inputs.FirstOrDefault(i => i.Title == AppSettings.Instance.StartupVideo);
            if (input != null) input.Role = InputRole.OpeningVideo;

            input = state.Inputs.FirstOrDefault(i => i.Title == AppSettings.Instance.ClosingImage);
            if (input != null) input.Role = InputRole.ClosingStaticImage;

            input = state.Inputs.FirstOrDefault(i => i.Title == AppSettings.Instance.ClosingVideo);
            if (input != null) input.Role = InputRole.ClosingVideo;

            input = state.Inputs.FirstOrDefault(i => i.Title == AppSettings.Instance.OverlayImage);
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

        /// <summary>
        ///     Check if the preset was loaded by comparing against a known / expected state (after preset is loaded)
        /// </summary>
        /// <returns></returns>
        public bool PresetLoaded()
        {
            var state = FetchState();
            return state.Inputs.Count(input => input.Role == InputRole.OpeninStaticImage) == 1 &&
                   state.Inputs.Count(input => input.Role == InputRole.OpeningVideo) == 1 &&
                   state.Inputs.Count(input => input.Role == InputRole.ClosingStaticImage) == 1 &&
                   state.Inputs.Count(input => input.Role == InputRole.ClosingVideo) == 1 &&
                   state.Inputs.Count(input => input.Role == InputRole.LogoOverlay) == 1 &&
                   state.Inputs.Count(input => input.Role == InputRole.Audio) >= 1 &&
                   state.Inputs.Count(input => input.Role == InputRole.Camera) > 0;
        }

        #region GetVmixProcess

        public Process GetVmixProcess()
        {
            FileSystemInfo fileInfo = new FileInfo(AppSettings.Instance.VmixExecutablePath);
            var sExeName = fileInfo.Name.Replace(fileInfo.Extension, "");

            var existingProcess = Process.GetProcessesByName(sExeName).FirstOrDefault();

            return existingProcess;
        }

        #endregion

        /// <summary>
        ///     Set the preview window to the specified input
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public VMixState SetPreview(VMixInput input)
        {
            return CallAndFetchState($"/?Function=PreviewInput&Input={input.Key}", "set preview");
        }

        /// <summary>
        ///     Set the preview window to the specified input
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public VMixState SetActive(VMixInput input)
        {
            return CallAndFetchState($"/?Function=ActiveInput&Input={input.Key}", "set active");
        }

        /// <summary>
        ///     Set the preview window to the specified input
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public VMixState SetOverlay(VMixInput input)
        {
            return CallAndFetchState($"/?Function=OverlayInput1&Input={input.Key}", "set overlay");
        }

        /// <summary>
        ///     Start streaming to the already selected channel
        /// </summary>
        /// <returns></returns>
        public VMixState StartStreaming()
        {
            var state = CallAndFetchState("/?Function=StartStreaming", "start streaming");

            return state;
        }

        /// <summary>
        ///     Start streaming to the already selected channel
        /// </summary>
        /// <returns></returns>
        public VMixState StartRecording()
        {
            var state = CallAndFetchState("/?Function=StartRecording", "start recording");

            return state;
        }

        private VMixState CallAndFetchState(string operation, string description)
        {
            var webRequest =
                (HttpWebRequest)WebRequest.CreateHttp($"{ConfigurationManager.AppSettings["apiPath"]}{operation}");
            var response = (HttpWebResponse)webRequest.GetResponse();
            if (response.StatusCode == HttpStatusCode.OK) return FetchState();

            var error = $"Could not {description} ({response.StatusDescription}";
            Log(error);
            throw new System.Exception(error);
        }

        public VMixState FadeToPreview()
        {
            var result = CallAndFetchState("/?Function=Transition1", "fade to preview");
            Thread.Sleep(2000);
            return result;
        }

        public VMixState TurnAudioOn()
        {
            var audioInputs = AppState.Instance.CurrentVmixState.Inputs.Where(input => input.Role == InputRole.Audio).ToList();
            VMixState state = null;
            foreach (var audioInput in audioInputs)
            {
                state = CallAndFetchState($"/?Function=AudioOn&Input={audioInput.Key}", "audio on");
            }
            return state;
        }

        public VMixState TurnAudioOff()
        {
            var audioInputs = AppState.Instance.CurrentVmixState.Inputs.Where(input => input.Role == InputRole.Audio).ToList();
            VMixState state = null;
            foreach (var audioInput in audioInputs)
            {
                state = CallAndFetchState($"/?Function=AudioOff&Input={audioInput.Key}", "audio off");
            }
            return state;
        }

        public VMixState TurnOverlayOn()
        {
            var overlayInput =
                AppState.Instance.CurrentVmixState.Inputs.Single(input => input.Role == InputRole.LogoOverlay);
            return CallAndFetchState($"/?Function=OverlayInput1In&Input={overlayInput.Key}", "turn overlay on");
        }

        public VMixState TurnOverlayOff()
        {
            var overlayInput =
                AppState.Instance.CurrentVmixState.Inputs.Single(input => input.Role == InputRole.LogoOverlay);
            return CallAndFetchState($"/?Function=OverlayInput1Off&Input={overlayInput.Key}", "turn overlay off");
        }

        public VMixState StartPlaylist()
        {
            CallAndFetchState($"/?Function=SelectPlayList&Value={ConfigurationManager.AppSettings["playlist-name"]}",
                "set playlist");
            return CallAndFetchState("/?Function=StartPlayList", "start playlist");
        }

        public VMixState StopPlaylist()
        {
            return CallAndFetchState("/?Function=StopPlayList", "stop playlist");
        }

        public VMixState StopStreaming()
        {
            return CallAndFetchState("/?Function=StopStreaming", "stop streaming");
        }

        public VMixState StopRecording()
        {
            return CallAndFetchState("/?Function=StopRecording", "stop recording");
        }
    }
}