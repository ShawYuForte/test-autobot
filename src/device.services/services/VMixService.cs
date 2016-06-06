using System.Configuration;
using System.IO;
using System.Net;
using System.Reflection;
using forte.device.extensions;
using forte.device.models;
using Newtonsoft.Json;
using RestSharp;

namespace forte.device.services
{
    public class VMixService
    {
        readonly RestClient _client;
        readonly VMixState _presetState;

        public VMixService()
        {
            _client = new RestClient(ConfigurationManager.AppSettings["apiPath"]);
            var path = new FileInfo(Assembly.GetExecutingAssembly().Location).Directory.FullName;
            var expectedStateFile =
                Path.Combine(path, "data\\expected-state.json");
            _presetState = JsonConvert.DeserializeObject<VMixState>(File.ReadAllText(expectedStateFile));
        }

        public VMixState FetchState()
        {
            var request = new RestRequest("", Method.GET);
            var response = _client.Execute<VMixState>(request);

            return response.Data;
        }

        public void LoadPreset()
        {
            var request = new RestRequest($"/?Function=OpenPreset&Value={ConfigurationManager.AppSettings["presetFilePath"]}", Method.GET)
            {
                Timeout = 1
            };
            _client.Execute<VMixState>(request);
        }

        public bool PresetLoaded()
        {
            var state = FetchState();
            return _presetState.SameAs(state);
        }
    }
}