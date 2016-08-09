#region

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


 

        public VMixState StopRecording()
        {
            return CallAndFetchState("/?Function=StopRecording", "stop recording");
        }
    }
}