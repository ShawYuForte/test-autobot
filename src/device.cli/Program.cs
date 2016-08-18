#region

using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using CommandLine;
using device.logging;
using device.web;
using device.web.server;
using forte.devices.models;
using forte.devices.options;
using forte.devices.services;
using forte.services;
using Microsoft.Practices.Unity;
using Newtonsoft.Json.Linq;

#endregion

namespace forte.devices
{
    public class Program
    {
        private static void Main(string[] args)
        {
            var options = new CliOptions();
            if (!Parser.Default.ParseArguments(args, options, ExecuteOptions))
            {
                Environment.Exit(Parser.DefaultExitCodeFail);
            }
        }

        private static void ExecuteOptions(string verb, object options)
        {
            switch (verb)
            {
                case RunOptions.VerbName:
                    RunDaemon((RunOptions)options);
                    break;
                case UpgradeOptions.VerbName:
                    UpgradeDaemon((UpgradeOptions)options);
                    break;
                default:
                    // Never happens
                    break;
            }
        }

        private static void UpgradeDaemon(UpgradeOptions options)
        {
            try
            {
                var stateJson = FetchRunningDaemonState();
                var parsedState = JObject.Parse(stateJson);
                var status = parsedState.SelectToken("status").Value<StreamingDeviceStatuses>();
                switch (status)
                {
                    case StreamingDeviceStatuses.Idle:
                    case StreamingDeviceStatuses.Offline:
                    case StreamingDeviceStatuses.Error:
                        break;
                    case StreamingDeviceStatuses.Streaming:
                    case StreamingDeviceStatuses.StreamingProgram:
                    case StreamingDeviceStatuses.StreamingAndRecording:
                    case StreamingDeviceStatuses.StreamingAndRecordingProgram:
                    case StreamingDeviceStatuses.Recording:
                    case StreamingDeviceStatuses.RecordingProgram:
                        Console.WriteLine("Device is not idle, cannot upgrade at this time.");
                        Environment.Exit(Parser.DefaultExitCodeFail);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            catch (WebException)
            {
                // Ignore
            }

            var process = new Process();
            var startup = new ProcessStartInfo
            {
                 UseShellExecute = true,
                 
            };
        }

        #region IsAlreadyRunning

        private static bool IsAlreadyRunning()
        {
            try
            {
                FetchRunningDaemonState();
                return true;
            }
            catch (WebException)
            {
                var exeName = Assembly.GetAssembly(typeof(Program)).Location;
                var currentProcess = Process.GetCurrentProcess();

                var processes = Process.GetProcessesByName(Path.GetFileNameWithoutExtension(exeName));
                var otherProcess = processes.FirstOrDefault(proc => proc.Id != currentProcess.Id);
                if (otherProcess == null) return false;
                Console.WriteLine("Another process is running, but could not connect to API");
                Environment.Exit(CommandLine.Parser.DefaultExitCodeFail);
            }

            return false;
        }

        private static string FetchRunningDaemonState()
        {
            var port = 9000;
            var portFilePath = GetPortFileName();
            if (File.Exists(portFilePath))
            {
                var portString = File.ReadAllText(portFilePath);
                int.TryParse(portString, out port);
            }
            var webClient = new WebClient();
            return webClient.DownloadString($"http://localhost:{port}/api/device");
        }

        #endregion

        #region GetPortFileName

        private static string GetPortFileName()
        {
            var assemblyFileLocation = Assembly.GetAssembly(typeof(Program)).Location;
            // ReSharper disable once PossibleNullReferenceException
            var portFilePath = Path.Combine(new FileInfo(assemblyFileLocation).Directory.FullName, ".port");
            return portFilePath;
        }

        #endregion

        #region RunDaemon

        private static void RunDaemon(RunOptions options)
        {
            if (IsAlreadyRunning())
            {
                Console.WriteLine("Daemon is already running.");
                return;
            }

            var portFilePath = GetPortFileName();
            if (options.Port != 9000)
            {
                File.WriteAllText(portFilePath, $"{options.Port}");
            }
            else if (File.Exists(portFilePath))
            {
                File.Delete(portFilePath);
            }

            var container = new UnityContainer();
            WebModule.Registrar.RegisterDependencies(container);
            WebModule.Registrar.RegisterMappings();

            ClientModule.Registrar.RegisterDependencies(container);
            ClientModule.Registrar.RegisterMappings();

            var runtimeConfig = container.Resolve<IRuntimeConfig>();
            runtimeConfig.DataPath = options.DataPath;
            runtimeConfig.LogPath = options.LogPath;

            VmixClientModule.Registrar.RegisterDependencies(container);
            VmixClientModule.Registrar.RegisterMappings();

            LoggerModule.Registrar.RegisterDependencies(container);

            CoreModule.SetDefaultSerializerSettings();

            var configManager = container.Resolve<IConfigurationManager>();
            configManager.UpdateSetting(SettingParams.ServerRootPath, options.ServerUrl);
            configManager.UpdateSetting(SettingParams.ServerApiPath, $"{options.ServerUrl}/api");

            var logger = container.Resolve<ILogger>();

            var daemon = container.Resolve<IDeviceDaemon>();

            logger.Information("Running device local UI web server.");
            using (var server = container.Resolve<ApiServer>().Run(options.Port))
            {
                logger.Information("Running device daemon.");
                daemon.Run();
            }

            logger.Information("Closing device daemon.");
        }

        #endregion
    }
}