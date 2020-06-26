#region

using System;
using AutoMapper;
using device.logging;
using device.web;
using device.web.server;
using forte.devices.config;
using forte.devices.data;
using forte.devices.entities;
using forte.devices.services;
using forte.devices.utils;
using forte.devices.workflow;
using forte.models;
using forte.services;
using Microsoft.Practices.Unity;

#endregion

namespace forte.devices
{
	public class Program
    {
		private static void Main(string[] args)
        {
			if(RuntimeUtility.IsAlreadyRunning())
			{
				Console.WriteLine("Daemon is already running.");
				return;
			}

			//var portFilePath = RuntimeUtility.GetPortFileName();
			//if(_options.Port != 9000)
			//{
			//	File.WriteAllText(portFilePath, $"{_options.Port}");
			//}
			//else if(File.Exists(portFilePath))
			//{
			//	File.Delete(portFilePath);
			//}

			var port = 9000;
			var container = new UnityContainer();

			//core configs
			CoreModule.SetDefaultSerializerSettings();
			container.RegisterType<IRuntimeConfig, RuntimeConfig>(new ContainerControlledLifetimeManager());
			var cf = container.Resolve<IRuntimeConfig>();
			cf.DataPath = "c:\\forte\\data";
			cf.LogPath = "c:\\forte\\logs";

			//logging
			LoggerModule.Registrar.RegisterDependencies(container);
			var logger = container.Resolve<ILogger>();

			//db
			container.RegisterType<DbRepository, DbRepository>(new ContainerControlledLifetimeManager());

			//settings
			Mapper.CreateMap<DataValue, DeviceSetting>();
			Mapper.CreateMap<DeviceSetting, DataValue>();
			container.RegisterType<IConfigurationManager, ConfigurationManager>(new HierarchicalLifetimeManager());
			var cm = container.Resolve<IConfigurationManager>();
			SetDefaults(cm);

			//vmix
			VmixClientModule.Registrar.RegisterDependencies(container);
			VmixClientModule.Registrar.RegisterMappings();

			//web server
			container.RegisterType<IApiServer, ApiServer>(new ContainerControlledLifetimeManager());
			container.RegisterType<IServerListener, ServerListener>(new ContainerControlledLifetimeManager());
			WebModule.Registrar.RegisterDependencies(container);
			WebModule.Registrar.RegisterMappings();

			container.RegisterType<IDeviceDaemon, StreamWorkflow>(new ContainerControlledLifetimeManager());
			var daemon = container.Resolve<IDeviceDaemon>();
			daemon.Start();
			daemon.Await(port);


			//string verb = null;
			//         object options = null;

			//         var cliOptions = new CliOptions();
			//         if (!Parser.Default.ParseArguments(args, cliOptions, (parsedVerb, parsedOptions) =>
			//         {
			//             verb = parsedVerb;
			//             options = parsedOptions;
			//         }))
			//         {
			//             Environment.Exit(Parser.DefaultExitCodeFail);
			//         }

			//         try
			//         {
			//             switch (verb)
			//             {
			//                 case RunOptions.VerbName:
			//                     new RunCommand((RunOptions)options).Run();
			//                     break;
			//                 case SimulatorOptions.VerbName:
			//                     new SimulatorCommand((SimulatorOptions)options).Run();
			//                     break;
			//                 case UpgradeOptions.VerbName:
			//                     new UpgradeCommand((UpgradeOptions)options).Run();
			//                     break;
			//                 default:
			//                     // Never happens
			//                     break;
			//             }
			//         }
			//         catch (Exception exception)
			//         {
			//             Console.WriteLine($"Error: {exception.Message}");
			//             Environment.Exit(Parser.DefaultExitCodeFail);
			//         }
		}

		private static void SetDefaults(IConfigurationManager configManager)
		{
			var config = configManager.GetDeviceConfig();

			if(string.IsNullOrWhiteSpace(config.Get<string>(SettingParams.DeviceId)))
			{
				config = configManager.UpdateSetting(SettingParams.DeviceId, Guid.NewGuid().ToString());
			}

			if(string.IsNullOrWhiteSpace(config.Get<string>(SettingParams.ServerApiPath)))
			{
				config = configManager.UpdateSetting(SettingParams.ServerApiPath, "server\\api");
			}

			if(string.IsNullOrWhiteSpace(config.Get<string>(SettingParams.ServerRootPath)))
			{
				config = configManager.UpdateSetting(SettingParams.ServerRootPath, "server\\");
			}
		}
    }
}