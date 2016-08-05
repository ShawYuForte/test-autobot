using System.IO;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Deserializers;
using RestSharp.Serializers;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;

namespace forte.devices.extensions
{
    public class NewtonsoftJsonSerializer : ISerializer, IDeserializer
    {
        private readonly JsonSerializer _serializer;

        public NewtonsoftJsonSerializer()
        {
            _serializer = JsonSerializer.CreateDefault(CoreModule.GetSerializerSettings());
        }

        public static NewtonsoftJsonSerializer Default => new NewtonsoftJsonSerializer();

        public string ContentType
        {
            get { return "application/json"; }
            set { }
        }

        public string DateFormat { get; set; }

        public string Namespace { get; set; }

        public string RootElement { get; set; }

        public string Serialize(object obj)
        {
            using (var stringWriter = new StringWriter())
            {
                using (var jsonTextWriter = new JsonTextWriter(stringWriter))
                {
                    _serializer.Serialize(jsonTextWriter, obj);

                    return stringWriter.ToString();
                }
            }
        }

        public T Deserialize<T>(IRestResponse response)
        {
            var content = response.Content;

            using (var stringReader = new StringReader(content))
            {
                using (var jsonTextReader = new JsonTextReader(stringReader))
                {
                    return _serializer.Deserialize<T>(jsonTextReader);
                }
            }
        }
    }
}