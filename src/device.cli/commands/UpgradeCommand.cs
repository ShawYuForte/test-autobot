#region

using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using CommandLine;
using forte.devices.models;
using forte.devices.options;
using forte.devices.utils;
using Newtonsoft.Json.Linq;

#endregion

namespace forte.devices.commands
{
    public class UpgradeCommand
    {
        private readonly UpgradeOptions _options;

        public UpgradeCommand(UpgradeOptions options)
        {
            _options = options;
        }

        public void Run()
        {
            DownloadNewPackage();
            Console.WriteLine("Attempting to update...");

            try
            {
                var stateJson = RuntimeUtility.FetchRunningDaemonState();
                var parsedState = JObject.Parse(stateJson);
                var statusString = parsedState.SelectToken("status").Value<string>();
                var status = (StreamingDeviceStatuses) Enum.Parse(typeof(StreamingDeviceStatuses), statusString);
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

            var startup = new ProcessStartInfo("choco.exe")
            {
                UseShellExecute = true,
                CreateNoWindow = false,
                Arguments = $"upgrade device-cli --source {_options.Repo} -y"
            };
            Process.Start(startup);
        }

        private void DownloadNewPackage()
        {
            if (!Directory.Exists(_options.Repo))
                Directory.CreateDirectory(_options.Repo);

            var existingPackages = Directory.GetDirectories(_options.Repo);
            Console.WriteLine("Looking for updates from nuget feed...");

            var startup = new ProcessStartInfo("nuget.exe")
            {
                UseShellExecute = false,
                //CreateNoWindow = true,
                RedirectStandardOutput = true,
                Arguments = string.IsNullOrWhiteSpace(_options.Source) ? "install device-cli" : $"install device-cli -source {_options.Source}",
                WorkingDirectory = _options.Repo
            };
            var process = Process.Start(startup);
            Console.WriteLine(process.StandardOutput.ReadToEnd());
            process.WaitForExit();
            if (process.ExitCode != 0)
            {
                Console.WriteLine($"Nuget process exited with code {process.ExitCode}");
            }

            var latestPackages = Directory.GetDirectories(_options.Repo);
            if (latestPackages.Any(current => existingPackages.All(previous => current != previous)))
            {
                Console.WriteLine("New package downloaded!");
                return;
            }

            Console.WriteLine("No new versions found.");
        }
    }
}