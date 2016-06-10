#region

using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading;
using forte.device.extensions;
using forte.device.models;
using Newtonsoft.Json;
using RestSharp;

#endregion

namespace forte.device.services
{
    public class VMixService : Service
    {
        private readonly RestClient _client;
        private VMixState _presetState;

        public VMixService()
        {
            _client = new RestClient(ConfigurationManager.AppSettings["apiPath"]);

            LoadExpectedPresets();
        }

        private void LoadExpectedPresets()
        {
            var path = new FileInfo(Assembly.GetExecutingAssembly().Location).Directory.FullName;
            var expectedStateFile =
                Path.Combine(path, "data\\expected-state.json");
            string expectedStateJson;

            if (File.Exists(expectedStateFile))
            {
                expectedStateJson = File.ReadAllText(expectedStateFile);
            }
            else
            {
                var assembly = Assembly.GetExecutingAssembly();
                const string resourceName = "forte.device.data.expected-state.json";

                using (var stream = assembly.GetManifestResourceStream(resourceName))
                using (var reader = new StreamReader(stream))
                {
                    expectedStateJson = reader.ReadToEnd();
                }
            }

            _presetState = JsonConvert.DeserializeObject<VMixState>(expectedStateJson);
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
            foreach (var input in state.Inputs)
            {
                var presetInput = _presetState.Inputs.FirstOrDefault(i => i.SameAs(input));
                if (presetInput == null) continue;
                input.Role = presetInput.Role;
            }

            state.Active = state.Inputs.FirstOrDefault(input => input.Number == state.ActiveNumber);
            state.Preview = state.Inputs.FirstOrDefault(input => input.Number == state.PreviewNumber);

            return state;
        }

        /// <summary>
        ///     Load presets based on a preset file defined in the app config
        /// </summary>
        public void LoadPreset()
        {
            var request =
                new RestRequest($"/?Function=OpenPreset&Value={ConfigurationManager.AppSettings["presetFilePath"]}",
                    Method.GET)
                {
                    Timeout = 1
                };
            _client.Execute<VMixState>(request);
        }

        /// <summary>
        ///     Check if the preset was loaded by comparing against a known / expected state (after preset is loaded)
        /// </summary>
        /// <returns></returns>
        public bool PresetLoaded()
        {
            var state = FetchState();
            return _presetState.SameAs(state);
        }

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
            return CallAndFetchState("/?Function=StartStreaming", "start streaming");
        }

        private VMixState CallAndFetchState(string operation, string description)
        {
            var webRequest =
                (HttpWebRequest) WebRequest.CreateHttp($"{ConfigurationManager.AppSettings["apiPath"]}{operation}");
            var response = (HttpWebResponse) webRequest.GetResponse();
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

        public VMixState ToggleAudio(VMixInput audioInput)
        {
            return CallAndFetchState($"/?Function=Audio&Input={audioInput.Key}", "toggle audio");
        }

        public VMixState ToggleOverlay(VMixInput overlayInput)
        {
            return CallAndFetchState($"/?Function=OverlayInput1&Input={overlayInput.Key}", "toggle overlay");
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
    }
}