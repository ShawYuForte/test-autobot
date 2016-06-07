using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using forte.device.extensions;
using forte.device.models;
using Newtonsoft.Json;
using RestSharp;

namespace forte.device.services
{
    public class VMixService
    {
        private readonly RestClient _client;
        private readonly VMixState _presetState;

        public VMixService()
        {
            _client = new RestClient(ConfigurationManager.AppSettings["apiPath"]);
            var path = new FileInfo(Assembly.GetExecutingAssembly().Location).Directory.FullName;
            var expectedStateFile =
                Path.Combine(path, "data\\expected-state.json");
            _presetState = JsonConvert.DeserializeObject<VMixState>(File.ReadAllText(expectedStateFile));
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
            var request = new RestRequest($"/?Function=PreviewInput&Input={input.Key}", Method.GET);
            _client.Execute<VMixState>(request);

            return FetchState();
        }

        /// <summary>
        ///     Set the preview window to the specified input
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public VMixState SetActive(VMixInput input)
        {
            var request = new RestRequest($"/?Function=ActiveInput&Input={input.Key}", Method.GET);
            _client.Execute<VMixState>(request);

            return FetchState();
        }

        /// <summary>
        ///     Set the preview window to the specified input
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public VMixState SetOverlay(VMixInput input)
        {
            var request = new RestRequest($"/?Function=OverlayInput1&Input={input.Key}", Method.GET);
            _client.Execute<VMixState>(request);

            return FetchState();
        }
    }
}