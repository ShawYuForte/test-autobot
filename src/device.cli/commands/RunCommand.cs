#region

using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using CommandLine;
using device.logging;
using device.web;
using device.web.server;
using forte.devices.models;
using forte.devices.options;
using forte.devices.services;
using forte.devices.utils;
using forte.services;
using Microsoft.Practices.Unity;

#endregion

namespace forte.devices.commands
{
    public class RunCommand
    {
        private readonly RunOptions _options;

        [DllImport("kernel32.dll")]
        static extern bool FreeConsole();


        public RunCommand(RunOptions options)
        {
            _options = options;
        }

        public void Run()
        {
            if (RuntimeUtility.IsAlreadyRunning())
            {
                Console.WriteLine("Daemon is already running.");
                return;
            }

            if (_options.Background)
            {
                Console.WriteLine("Running in the background...");
                RunSilent();
                return;
            }

            var portFilePath = RuntimeUtility.GetPortFileName();
            if (_options.Port != 9000)
            {
                File.WriteAllText(portFilePath, $"{_options.Port}");
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
            runtimeConfig.DataPath = _options.DataPath;
            runtimeConfig.LogPath = _options.LogPath;

            VmixClientModule.Registrar.RegisterDependencies(container);
            VmixClientModule.Registrar.RegisterMappings();

            LoggerModule.Registrar.RegisterDependencies(container);

            CoreModule.SetDefaultSerializerSettings();

            var configManager = container.Resolve<IConfigurationManager>();
            if (!string.IsNullOrWhiteSpace(_options.ServerUrl))
            {
                configManager.UpdateSetting(SettingParams.ServerRootPath, _options.ServerUrl);
                configManager.UpdateSetting(SettingParams.ServerApiPath, $"{_options.ServerUrl}/api");
            }
            else
            {
                var config = configManager.GetDeviceConfig();
                if (string.IsNullOrWhiteSpace(config.Get<string>(SettingParams.ServerApiPath)))
                {
                    Console.WriteLine("Server URL is required when running for the first time.");
                    Environment.Exit(Parser.DefaultExitCodeFail);
                }
            }

            var logger = container.Resolve<ILogger>();

            var daemon = container.Resolve<IDeviceDaemon>();

            if (_options.Background)
            {
                Console.WriteLine("Running silent...");
                //FreeConsole();
            }

            logger.Information("Initializing...");
            if (!string.IsNullOrWhiteSpace(_options.DeviceId))
            {
                logger.Debug("New device id specified {@deviceId}", _options.DeviceId);
                Guid deviceId;

                if (_options.DeviceId.ToLower() == "new")
                {
                    deviceId = Guid.NewGuid();
                }
                else if (!Guid.TryParse(_options.DeviceId, out deviceId))
                {
                    Console.WriteLine("Device identifier is not a valid Guid.");
                    Environment.Exit(Parser.DefaultExitCodeFail);
                }
                logger.Debug("New device id {@deviceId}", deviceId);
                daemon.Init(deviceId);
            }
            else
            {
                daemon.Init();
            }

            logger.Information("Running device local UI web server.");
            using (var server = container.Resolve<ApiServer>().Run(_options.Port))
            {
                logger.Information("Running device daemon.");
                daemon.Run();
            }

            logger.Information("Closing device daemon.");
        }

        private void RunSilent()
        {
            // next run should not be silent, otherwise it will keep looping
            _options.Background = false;
            Console.WriteLine($"device-cli.exe {_options.ToArgs()}");

            var process = new Process
            {
                StartInfo =
                {
                    FileName = "device-cli.exe",
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    WindowStyle = ProcessWindowStyle.Hidden,
                    Arguments = _options.ToArgs()
                }
            };

            process.Start();
        }
    }
}