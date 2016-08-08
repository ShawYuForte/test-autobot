using System.Configuration;
using System.Linq;
using System.Text.RegularExpressions;
using forte.devices.models;
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
            var request = new RestRequest("", Method.GET);
            var response = _client.Execute<VmixState>(request);

            if (response.ResponseStatus != ResponseStatus.Completed) return null;
            var vmixState = MatchPresetStateRoles(response.Data);
            return VmixClientModule.Registrar.CreateMapper().Map<StreamingClientState>(vmixState);
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


        /// <summary>
        ///     Load presets based on a preset file defined in the app config
        /// </summary>
        private void LoadPreset()
        {
            var config = _configurationManager.GetDeviceConfig();
            var requestUrl = $"/?Function=OpenPreset&Value={config.Get<string>("VmixPresetFilePath")}";
            var request = new RestRequest(requestUrl, Method.GET)
            {
                Timeout = 1
            };
            _client.Execute<VmixState>(request);
        }
    }
}