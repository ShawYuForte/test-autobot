using System;
using CommandLine;
using device.logging;
using device.web;
using device.web.server;
using forte.devices.models;
using forte.devices.options;
using forte.devices.services;
using forte.services;
using Microsoft.Practices.Unity;

namespace forte.devices
{
    public class Program
    {
        private static void Main(string[] args)
        {
            var cliOptions = new CliOptions();
            if (!Parser.Default.ParseArguments(args, cliOptions))
            {
                return;
            }

            var container = new UnityContainer();
            WebModule.Registrar.RegisterDependencies(container);
            WebModule.Registrar.RegisterMappings();

            ClientModule.Registrar.RegisterDependencies(container);
            ClientModule.Registrar.RegisterMappings();

            var runtimeConfig = container.Resolve<IRuntimeConfig>();
            runtimeConfig.DataPath = cliOptions.DataPath;
            runtimeConfig.LogPath = cliOptions.LogPath;

            VmixClientModule.Registrar.RegisterDependencies(container);
            VmixClientModule.Registrar.RegisterMappings();

            LoggerModule.Registrar.RegisterDependencies(container);

            CoreModule.SetDefaultSerializerSettings();

            var configManager = container.Resolve<IConfigurationManager>();
            configManager.UpdateSetting(SettingParams.ServerRootPath, cliOptions.ServerUrl);
            configManager.UpdateSetting(SettingParams.ServerApiPath, $"{cliOptions.ServerUrl}/api");

            var logger = container.Resolve<ILogger>();

            var daemon = container.Resolve<IDeviceDaemon>();

            logger.Information("Running device local UI web server.");
            using (var server = container.Resolve<ApiServer>().Run(cliOptions.Port))
            {
                logger.Information("Running device daemon.");
                daemon.Run();
            }

            logger.Information("Closing device daemon.");
        }
    }
}