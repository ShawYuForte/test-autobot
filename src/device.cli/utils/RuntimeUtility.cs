#region

using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;

#endregion

namespace forte.devices.utils
{
    public class RuntimeUtility
    {
        #region GetPortFileName

        public static string GetPortFileName()
        {
            var assemblyFileLocation = Assembly.GetAssembly(typeof(Program)).Location;
            // ReSharper disable once PossibleNullReferenceException
            var portFilePath = Path.Combine(new FileInfo(assemblyFileLocation).Directory.FullName, ".port");
            return portFilePath;
        }

        #endregion

        #region IsAlreadyRunning

        public static bool IsAlreadyRunning()
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

        public static string FetchRunningDaemonState()
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
    }
}