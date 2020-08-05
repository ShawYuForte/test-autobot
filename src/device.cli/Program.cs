#region

using System;
using System.Diagnostics;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CommandLine;
using device.logging;
using device.web;
using device.web.server;
using forte.devices.config;
using forte.devices.data;
using forte.devices.entities;
using forte.devices.options;
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

			if(!ProcessArgs(args))
			{
				return;
			}

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

			//mail
			container.RegisterType<MailService, MailService>(new ContainerControlledLifetimeManager());

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
		}

		private static bool ProcessArgs(string[] args)
		{
			if(args.Length == 0)
			{
				args = new string[] { "run" };
			}

			string verb = null;
			object options = null;
			var cliOptions = new CliOptions();
			Parser.Default.ParseArguments(args, cliOptions, (parsedVerb, parsedOptions) =>
			{
				verb = parsedVerb;
				options = parsedOptions;
			});

			try
			{
				switch(verb)
				{
					case RunOptions.VerbName:
						var optionsTyped = (RunOptions) options;
						if(optionsTyped.Background)
						{
							optionsTyped.Background = false;
							var process = new Process
							{
								StartInfo =
								{
									FileName = "device-cli.exe",
									UseShellExecute = false,
									CreateNoWindow = true,
									WindowStyle = ProcessWindowStyle.Hidden,
									Arguments = optionsTyped.ToArgs()
								}
							};

							process.Start();
							return false;
						}
						break;
					case UpgradeOptions.VerbName:
						//var optionsTyped1 = (UpgradeOptions) options;
						break;
					default:
						// Never happens
						break;
				}
			}
			catch(Exception exception)
			{
				Console.WriteLine($"Error: {exception.Message}");
				Environment.Exit(Parser.DefaultExitCodeFail);
			}

			return true;
		}

		private static void SetDefaults(IConfigurationManager configManager)
		{
			var config = configManager.GetDeviceConfig();

			//for upgrade reasons
			var guid = config.Get<Guid>(SettingParams.DeviceId);
			if(guid != Guid.Empty)
			{
				config = configManager.UpdateSetting(SettingParams.DeviceId, guid.ToString());
			}

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

			if(!config.Contains(SettingParams.VerboseDebug))
			{
				config = configManager.UpdateSetting(SettingParams.VerboseDebug, true);
			}
		}
    }
}