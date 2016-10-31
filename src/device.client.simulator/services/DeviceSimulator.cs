using System;
using System.Net;
using System.Threading;
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

            _configurationManager.UpdateSetting("DeviceId", deviceId);
            _configurationManager.UpdateSetting(SettingParams.ServerApiPath, server);

            _serverListener = new ServerListener(_configurationManager, null);
        }

        private readonly IConfigurationManager _configurationManager = new SimulatorConfigManager();
        private readonly IServerListener _serverListener;

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
            //Console.WriteLine("* delay [s]: delay responses by 's' seconds");
            Console.WriteLine("* device [id]: get/set device identifier");
            Console.WriteLine("* help: this help");
            Console.WriteLine("* peek: peek at an available message, without processing it");
            Console.WriteLine("* receive [params]: receive and process message, additional params:");
            Console.WriteLine("  - s or f: simulate (s)uccessful or (f)ailure response ('s' is default)");
            //Console.WriteLine("  - auto: auto listen to messages (using WebSockets)");
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
            if (remoteCommand == null)
            {
                Console.WriteLine("No command available");
            }
            else
            {
                Console.WriteLine(JsonConvert.SerializeObject(remoteCommand, Formatting.Indented));
            }
            PrintFooter("peek next command");
        }

        private int _delay;

        public void Run()
        {
            Console.WriteLine("Device Simulator, enter command (type 'help' to learn more)");
            string command;
            Console.WriteLine();
            Console.Write("Command: ");
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
                        break;

                    case "help":
                        Help();
                        break;

                    case "peek":
                        Peek();
                        break;

                    case "receive":
                        Receive(true);
                        break;

                    case "quit":
                        return;

                    case "send":
                        Send();
                        break;

                    case "state":
                        PrintState();
                        break;

                    default:
                        try
                        {
                            var commandParams = command.Split(' ');
                            if (command.ToLower().StartsWith("delay"))
                            {
                                _delay = int.Parse(commandParams[1]);
                            }
                            if (command.ToLower().StartsWith("device"))
                            {
                                PrintHeading("set device identifier");
                                Guid deviceId;
                                Guid.TryParse(commandParams[1], out deviceId);
                                DeviceId = deviceId;
                                _configurationManager.UpdateSetting("DeviceId", deviceId);
                                PrintFooter("set device identifier");
                            }
                            if (command.ToLower().StartsWith("receive"))
                            {
                                var success = commandParams[1] != "f";
                                if ((commandParams.Length > 2 && commandParams[2] == "auto") ||
                                    commandParams[1] == "auto")
                                {
                                    AutoReceive(success, true);
                                }
                                else
                                {
                                    AutoReceive(success, false);
                                }
                                Receive(success);
                            }
                            if (command.ToLower().StartsWith("state"))
                            {
                                var field = commandParams[1];
                                if (field == "status")
                                {
                                    Status =
                                        (StreamingDeviceStatuses)
                                            Enum.Parse(typeof(StreamingDeviceStatuses), commandParams[2]);
                                }
                                else if (field == "stream")
                                {
                                    ActiveVideoStreamId = commandParams.Length > 2
                                        ? Guid.Parse(commandParams[2])
                                        : (Guid?) null;
                                }
                                else
                                {
                                    Console.WriteLine($"Unknown field '{field}'");
                                }
                                PrintState();
                            }
                        }
                        catch(Exception exception)
                        {
                            Console.WriteLine(exception.Message);
                            Console.WriteLine();
                        }
                        break;
                }

                Console.Write("Command: ");
            }
        }

        private bool _autoSucceed;

        private void AutoReceive(bool succeed, bool enabled)
        {
            _autoSucceed = succeed;
            if (enabled)
            {
                _serverListener.Connect();
                _serverListener.MessageReceived += _serverListener_MessageReceived;
            }
            else
            {
                _serverListener.Disconnect();
                _serverListener.MessageReceived -= _serverListener_MessageReceived;
            }
        }

        private void _serverListener_MessageReceived(string message)
        {
            Receive(_autoSucceed);
        }

        private void Receive(bool succeed)
        {
            var title = "receive command, respond with " + (succeed ? "success" : "failure");
            PrintHeading(title);
            var command = FetchCommand();
            if (command == null)
            {
                Console.WriteLine("No command available");
                PrintFooter(title);
                return;
            }
            switch (command.Command)
            {
                case DeviceCommands.StartStreaming:
                    Status = succeed ? StreamingDeviceStatuses.Streaming : StreamingDeviceStatuses.Error;
                    break;
                case DeviceCommands.StartProgram:
                    Status = succeed ? StreamingDeviceStatuses.StreamingProgram : StreamingDeviceStatuses.Error;
                    break;
                case DeviceCommands.StopStreaming:
                    Status = succeed ? StreamingDeviceStatuses.Idle : StreamingDeviceStatuses.Error;
                    break;
                case DeviceCommands.StopProgram:
                    Status = succeed ? StreamingDeviceStatuses.Streaming : StreamingDeviceStatuses.Error;
                    break;
                case DeviceCommands.ResetToIdle:
                    Status = succeed ? StreamingDeviceStatuses.Idle : StreamingDeviceStatuses.Error;
                    break;
                case DeviceCommands.UpdateState:
                default:
                    break;
            }

            if (_delay > 0)
            {
                Thread.Sleep(_delay);
            }
            Send();
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

            PrintHeading("sending state");
            if (response.StatusCode != HttpStatusCode.OK)
            {
                // Log error
                Console.WriteLine("ERROR: " +
                                  (response.ErrorMessage ?? $"Publishing state, response was {response.StatusCode}"));
                Console.WriteLine(response.Content);
            }
            else
            {
                Console.WriteLine("SUCCESS");
            }
            PrintFooter("sending state");
        }
    }
}