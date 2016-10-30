using System;
using System.Net;
using forte.devices.extensions;
using forte.devices.models;
using forte.models.devices;
using Newtonsoft.Json;
using RestSharp;
using StreamingDeviceState = forte.devices.models.StreamingDeviceState;
using StreamingDeviceStatuses = forte.devices.models.StreamingDeviceStatuses;

namespace forte.devices.services
{
    public class DeviceSimulator
    {
        public DeviceSimulator(string server, Guid deviceId)
        {
            Server = server;
            DeviceId = deviceId;
        }

        public Guid DeviceId { get; private set; }
        public string Server { get; }

        public StreamingDeviceStatuses Status { get; private set; }

        public Guid? ActiveVideoStreamId { get; private set; }

        private void PrintHeading(string command)
        {
            Console.WriteLine();
            Console.WriteLine("-----------------------------------------------------------------------");
            Console.WriteLine($"- Executing '{command}'");
            Console.WriteLine("-----------------------------------------------------------------------");
        }

        private void PrintFooter(string command)
        {
            Console.WriteLine("-----------------------------------------------------------------------");
            Console.WriteLine($"- Executed '{command}'");
            Console.WriteLine("-----------------------------------------------------------------------");
            Console.WriteLine();
        }

        private void Help()
        {
            PrintHeading("available commands");
            Console.WriteLine("* clear: clear the screen");
            Console.WriteLine("* device [id]: get/set device identifier");
            Console.WriteLine("* help: this help");
            Console.WriteLine("* peek: peek at an available message, without processing it");
            Console.WriteLine("* receive [params]: receive and process message, additional params:");
            Console.WriteLine("  - s: (default) simulate successful response");
            Console.WriteLine("  - f: simulate failure response");
            Console.WriteLine("* send: send current simulated state to the server");
            Console.WriteLine("* state: get/set device state, additional params:");
            Console.WriteLine("* - status [value]: set device status");
            Console.WriteLine("* - stream [id]: set active stream identifier");
            PrintFooter("available commands");
        }

        private DeviceCommandModel FetchCommand()
        {
            var _client = new RestClient($"{Server}/devices/");
            var request = new RestRequest($"{DeviceId}/commands/next", Method.GET)
            {
                JsonSerializer = NewtonsoftJsonSerializer.Default
            };
            var response = _client.Execute<DeviceCommandModelEx>(request);
            // Not found if no command
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                // ... so just exit
                return null;
            }
            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception(response.ErrorMessage ?? response.StatusDescription);
            }
            return response.Data;
        }

        private void Peek()
        {
            PrintHeading("peek next command");
            var remoteCommand = FetchCommand();
            Console.WriteLine(JsonConvert.SerializeObject(remoteCommand, Formatting.Indented));
            PrintFooter("peek next command");
        }

        public void Run()
        {
            Console.WriteLine("Device Simulator, enter command (type 'help' to learn more):");
            string command;
            while ((command = Console.ReadLine()) != null)
            {
                switch (command.ToLower())
                {
                    case "clear":
                        Console.Clear();
                        break;

                    case "device":
                        Console.WriteLine();
                        Console.WriteLine($"Current device identifier '{DeviceId}");
                        Console.WriteLine();
                        continue;

                    case "help":
                        Help();
                        continue;

                    case "peek":
                        Peek();
                        continue;

                    case "quit":
                        return;

                    case "send":
                        Send();
                        continue;

                    case "state":
                        PrintState();
                        continue;
                }
                if (command.ToLower().StartsWith("device"))
                {
                    PrintHeading("set device identifier");
                    Guid deviceId;
                    Guid.TryParse(command.Split(' ')[1], out deviceId);
                    DeviceId = deviceId;
                    PrintFooter("set device identifier");
                }
            }
        }

        private void PrintState()
        {
            PrintHeading("print simulated state");
            var state = GetState();
            var serialized = JsonConvert.SerializeObject(state, Formatting.Indented);
            Console.WriteLine(serialized);
            PrintFooter("print simulated state");
        }

        private StreamingDeviceState GetState()
        {
            var deviceState = new StreamingDeviceState
            {
                DeviceId = DeviceId,
                ActiveVideoStreamId = ActiveVideoStreamId,
                StateCapturedOn = DateTime.UtcNow,
                Status = Status
            };
            return deviceState;
        }

        public void Send()
        {
            var _client = new RestClient($"{Server}/devices/");
            var deviceState = GetState();
            var request = new RestRequest($"{deviceState.DeviceId}/state", Method.POST)
            {
                JsonSerializer = NewtonsoftJsonSerializer.Default
            };
            request.AddHeader("Content-Type", "application/json; charset=utf-8");
            request.AddJsonBody(deviceState);
            var response = _client.Execute(request);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                // Log error
                throw new Exception(response.ErrorMessage ?? $"Publishing state, response was {response.StatusCode}");
            }
        }
    }
}