#region

using System;
using System.IO;
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

            if (!string.IsNullOrWhiteSpace(_options.ServerUrl))
            {
                var configManager = container.Resolve<IConfigurationManager>();
                configManager.UpdateSetting(SettingParams.ServerRootPath, _options.ServerUrl);
                configManager.UpdateSetting(SettingParams.ServerApiPath, $"{_options.ServerUrl}/api");
            }

            var logger = container.Resolve<ILogger>();

            var daemon = container.Resolve<IDeviceDaemon>();

            logger.Information("Running device local UI web server.");
            using (var server = container.Resolve<ApiServer>().Run(_options.Port))
            {
                logger.Information("Running device daemon.");
                daemon.Run();
            }

            logger.Information("Closing device daemon.");
        }
    }
}