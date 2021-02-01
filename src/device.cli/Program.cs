#region

using System;
using System.Diagnostics;
using System.Linq;
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
using forte.devices.data.enums;
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
		private static UnityContainer _container;

		private static void Main(string[] args)
		{
			//AppDomain.CurrentDomain.FirstChanceException += (sender, eventArgs) => {
			//	Console.WriteLine(eventArgs.Exception.ToString());
			//};

			//run -d "D:\!Projects\TDM-FORTE\iot\src\AutobotLauncher\bin\data" -l "D:\!Projects\TDM-FORTE\iot\src\AutobotLauncher\bin\logs" --pr "D:\!Projects\TDM-FORTE\iot\src\AutobotLauncher\bin\device-cli.2.2.3\tools\cli\preset\Forte Preset.vmix" -test-preset

			//args = new string[]
			//{
			//	//"run",
			//	//"-p",
			//	//"9000",
			//	//"-d",
			//	//"c:\\forte\\data",
			//	//"-l",
			//	//"c:\\forte\\logs"

			//	//"run",
			//	//"-b",
			////	"-d",
			////	@"D:\!Projects\TDM-FORTE\iot\src\AutobotLauncher\bin\data",
			////	"-l",
			////	@"D:\!Projects\TDM-FORTE\iot\src\AutobotLauncher\bin\logs",
			////	"--pr",
			////	@"D:\!Projects\TDM-FORTE\iot\src\AutobotLauncher\bin\preset\Forte Preset.vmix",
			////	"--test-api"
			//};

			var checkVersion = args.Any(arg => arg == "-version");
			if (checkVersion)
			{
				Console.WriteLine(Constants.Version);
				return;
			}

			var argsParsed = ProcessArgs(args);
			if (argsParsed == null) { return; }

			var port = 9000;
			var container = _container = new UnityContainer();

			//core configs
			CoreModule.SetDefaultSerializerSettings();
			container.RegisterType<IRuntimeConfig, RuntimeConfig>(new ContainerControlledLifetimeManager());
			var rc = _container.Resolve<IRuntimeConfig>();

			//set runtime configs
			if (!string.IsNullOrEmpty(argsParsed.DataPath)) { rc.DataPath = argsParsed.DataPath; }
			if (!string.IsNullOrEmpty(argsParsed.LogPath)) { rc.LogPath = argsParsed.LogPath; }
			rc.PresetPath = argsParsed.PresetPath;

			//web server
			container.RegisterType<IApiServer, ApiServer>(new ContainerControlledLifetimeManager());
			container.RegisterType<IServerListener, ServerListener>(new ContainerControlledLifetimeManager());
			WebModule.Registrar.RegisterDependencies(container);
			WebModule.Registrar.RegisterMappings();

			//logging
			LoggerModule.Registrar.RegisterDependencies(container);
			var logger = container.Resolve<ILogger>();

			//mail
			container.RegisterType<MailService, MailService>(new ContainerControlledLifetimeManager());

			//agora
			container.RegisterType<AgoraService, AgoraService>();

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

			container.RegisterType<IDeviceDaemon, StreamWorkflow>(new ContainerControlledLifetimeManager());
			var daemon = container.Resolve<IDeviceDaemon>();

			if (argsParsed.TestPreset)
			{
				daemon.RunPresetTest().Wait();
				return;
			}

			if (argsParsed.TestApi)
			{
				daemon.RunApiTest().Wait();
				return;
			}

			if (RuntimeUtility.IsAlreadyRunning())
			{
				Console.WriteLine("Daemon is already running.");
				return;
			}

			daemon.Start();
			daemon.Await(port);
		}

		private static RunOptions ProcessArgs(string[] args)
		{
			if (args.Length == 0)
			{
				args = new string[] { "run" };
			}

			object options = null;
			var cliOptions = new CliOptions();
			Parser.Default.ParseArguments(args, cliOptions, (parsedVerb, parsedOptions) =>
			{
				options = parsedOptions;
			});

			try
			{
				var optionsTyped = (RunOptions)options;
				if (optionsTyped.Background)
				{
					optionsTyped.Background = false;
					var process = new Process
					{
						StartInfo =
						{
							FileName = "device-cli.exe",
							//UseShellExecute = true,
							UseShellExecute = false,
							CreateNoWindow = true,
							WindowStyle = ProcessWindowStyle.Hidden,
							Arguments = optionsTyped.ToArgs()
						}
					};

					process.Start();
					return null;
				}

				return optionsTyped;
			}
			catch (Exception exception)
			{
				Console.WriteLine($"Error: {exception.Message}");
				return null;
			}

			return null;
		}

		private static void SetDefaults(IConfigurationManager configManager)
		{
			var config = configManager.GetDeviceConfig();

			//for upgrade reasons
			var guid = config.Get<Guid>(SettingParams.DeviceId);
			if (guid != Guid.Empty)
			{
				config = configManager.UpdateSetting(SettingParams.DeviceId, guid.ToString());
			}

			var customDeviceId = config.Get<string>(SettingParams.CustomDeviceId);
            if (customDeviceId == null)
			{
				config = configManager.UpdateSetting(SettingParams.CustomDeviceId, string.Empty);
			}

			var customDeviceIdPresent = config.Get<string>(SettingParams.CustomDeviceIdPresent);
			if (customDeviceIdPresent == null)
			{
				config = configManager.UpdateSetting(SettingParams.CustomDeviceIdPresent, "False");
			}
			else
			{
				config = configManager.UpdateSetting(SettingParams.CustomDeviceIdPresent, customDeviceId == null);
			}

			if (string.IsNullOrWhiteSpace(config.Get<string>(SettingParams.DeviceId)))
			{
				config = configManager.UpdateSetting(SettingParams.DeviceId, Guid.NewGuid().ToString());
			}

			if (string.IsNullOrWhiteSpace(config.Get<string>(SettingParams.ServerApiPath)))
			{
				config = configManager.UpdateSetting(SettingParams.ServerApiPath, "server\\api");
			}

			if (string.IsNullOrWhiteSpace(config.Get<string>(SettingParams.ServerRootPath)))
			{
				config = configManager.UpdateSetting(SettingParams.ServerRootPath, "server\\");
			}

			if (!config.Contains(SettingParams.VerboseDebug))
			{
				config = configManager.UpdateSetting(SettingParams.VerboseDebug, true);
			}

			if (string.IsNullOrWhiteSpace(config.Get<string>(SettingParams.DeviceName)))
			{
				config = configManager.UpdateSetting(SettingParams.DeviceName, "Forte-device");
			}

			if (string.IsNullOrWhiteSpace(config.Get<string>(SettingParams.AgoraAppId)))
			{
				config = configManager.UpdateSetting(SettingParams.AgoraAppId, "false");
			}

			if (!string.IsNullOrWhiteSpace(config.Get<string>(SettingParams.AgoraApiUrl)))
			{
				configManager.DeleteSetting(SettingParams.AgoraApiUrl);
			}

            if (!config.Contains(SettingParams.VMixFullScreen))
            {
                configManager.UpdateSetting(SettingParams.VMixFullScreen, false);
            }

            if (!config.Contains(SettingParams.AgoraRtmpUrl))
            {
                configManager.UpdateSetting(SettingParams.AgoraRtmpUrl, "rtmp://104.209.236.226:1935");
            }
		}
	}
}