using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using AutobotLauncher.Enums;
using AutobotLauncher.Utils;

namespace AutobotLauncher
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private MainWindowViewModel _model = new MainWindowViewModel();

		public MainWindow()
		{
			InitializeComponent();

			DataContext = _model;

			Check();
		}

		private void Check()
		{
			_model.IsNugetInstalled = _model.IsNugetInstalled == true || "nuget".ProcessExists();
			_model.IsChocoInstalled = _model.IsChocoInstalled == true || "choco".ProcessExists();
			_model.IsClientInstalled = _model.IsClientInstalled == true || "device-cli".ProcessExists("-t");
			_model.IsVmixInstalled = File.Exists(Constants.VmixPath);
			_model.IsApiConnected = _model.IsClientInstalled;

			_model.SetStatus();
		}

		private void ClickCheck(object sender, RoutedEventArgs e)
		{
			if (_model.StatusEnum == LaunchStatus.None)
			{
				Check();
			}

			if (_model.StatusEnum == LaunchStatus.None)
			{
				//launch
				return;
			}

			if (_model.StatusEnum == LaunchStatus.AutoInstall)
			{
				if (_model.IsChocoInstalled != true)
				{
					InstallChoco();
				}
			}

			Check();
		}

		private void InstallChoco()
		{
			"nuget".ProcessRunAndWaitAsAdmin("install chocolatey");
			"powershell.exe".ProcessRunAndWaitAsAdmin(@"-executionpolicy unrestricted \chocolatey.0.10.14\tools\Install.ps1");

			Console.WriteLine("InstallChoco complete");
		}

		private void InstallClient()
		{
		}

		//public void Run()
		//{
		//	DownloadNewPackage();
		//	Console.WriteLine("Attempting to update...");

		//	try
		//	{
		//		var stateJson = RuntimeUtility.FetchRunningDaemonState();
		//		var parsedState = JObject.Parse(stateJson);
		//		var statusString = parsedState.SelectToken("status").Value<string>();
		//		var status = (StreamingDeviceStatuses) Enum.Parse(typeof(StreamingDeviceStatuses), statusString);
		//		switch (status)
		//		{
		//			case StreamingDeviceStatuses.Idle:
		//			case StreamingDeviceStatuses.Offline:
		//			case StreamingDeviceStatuses.Error:
		//				break;
		//			case StreamingDeviceStatuses.Streaming:
		//			case StreamingDeviceStatuses.StreamingProgram:
		//			case StreamingDeviceStatuses.StreamingAndRecording:
		//			case StreamingDeviceStatuses.StreamingAndRecordingProgram:
		//			case StreamingDeviceStatuses.Recording:
		//			case StreamingDeviceStatuses.RecordingProgram:
		//				Console.WriteLine("Device is not idle, cannot upgrade at this time.");
		//				Environment.Exit(Parser.DefaultExitCodeFail);
		//				break;
		//			default:
		//				throw new ArgumentOutOfRangeException();
		//		}
		//	}
		//	catch (WebException)
		//	{
		//		// Ignore
		//	}

		//	var startup = new ProcessStartInfo("choco.exe")
		//	{
		//		UseShellExecute = true,
		//		CreateNoWindow = false,
		//		Arguments = $"upgrade device-cli --source {_options.Repo} -y"
		//	};
		//	Process.Start(startup);
		//}

		//private void DownloadNewPackage()
		//{
		//	if (!Directory.Exists(_options.Repo))
		//		Directory.CreateDirectory(_options.Repo);

		//	var existingPackages = Directory.GetDirectories(_options.Repo);
		//	Console.WriteLine("Looking for updates from nuget feed...");
		//	Console.WriteLine();

		//	var process = new Process
		//	{
		//		StartInfo =
		//		{
		//			FileName = "nuget.exe",
		//			UseShellExecute = false,
		//			RedirectStandardOutput = true,
		//			Arguments = string.IsNullOrWhiteSpace(_options.Source)
		//				? "install device-cli"
		//				: $"install device-cli -source {_options.Source}",
		//			WorkingDirectory = _options.Repo
		//		}
		//	};
		//	process.OutputDataReceived += Process_OutputDataReceived;

		//	process.Start();

		//	// Start the asynchronous read of the sort output stream.
		//	process.BeginOutputReadLine();

		//	process.WaitForExit();

		//	Console.WriteLine();
		//	if (process.ExitCode != 0)
		//	{
		//		Console.WriteLine($"Nuget process exited with code {process.ExitCode}");
		//	}

		//	var latestPackages = Directory.GetDirectories(_options.Repo);
		//	if (latestPackages.Any(current => existingPackages.All(previous => current != previous)))
		//	{
		//		Console.WriteLine("New package downloaded!");
		//		return;
		//	}

		//	Console.WriteLine("No new versions found.");
		//}
	}
}
