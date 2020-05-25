#region

using System;
using CommandLine;
using forte.devices.commands;
using forte.devices.options;

#endregion

namespace forte.devices
{
    public class Program
    {
        private static void Main(string[] args)
        {
   //         new RunCommand(new RunOptions
			//{
			//	ServerUrl = "https://qaapic84efe53.forte.fit/",
			//	//ServerUrl = "http://dev-api.forte.fit",
			//	DeviceId = "2ea4a061-1ee5-4cb7-8599-53984dedf9be",
			//	DataPath = "c:\\forte\\data",
			//	LogPath = "c:\\forte\\logs",
			//	Port = 9000
			//}).Run();
			//return;

			//pre  xcopy /s /y /e /I $(SolutionDir)\device.web\client $(TargetDir)\client
			/*post 
				if not exist "$(TargetDir)x86" md "$(TargetDir)x86"
				xcopy /s /y "$(SolutionDir)packages\Microsoft.SqlServer.Compact.4.0.8876.1\NativeBinaries\x86\*.*" "$(TargetDir)x86"
				if not exist "$(TargetDir)amd64" md "$(TargetDir)amd64"
				xcopy /s /y "$(SolutionDir)packages\Microsoft.SqlServer.Compact.4.0.8876.1\NativeBinaries\amd64\*.*" "$(TargetDir)amd64"
			*/

			string verb = null;
            object options = null;

            var cliOptions = new CliOptions();
            if (!Parser.Default.ParseArguments(args, cliOptions, (parsedVerb, parsedOptions) =>
            {
                verb = parsedVerb;
                options = parsedOptions;
            }))
            {
                Environment.Exit(Parser.DefaultExitCodeFail);
            }

            try
            {
                switch (verb)
                {
                    case RunOptions.VerbName:
                        new RunCommand((RunOptions)options).Run();
                        break;
                    case SimulatorOptions.VerbName:
                        new SimulatorCommand((SimulatorOptions)options).Run();
                        break;
                    case UpgradeOptions.VerbName:
                        new UpgradeCommand((UpgradeOptions)options).Run();
                        break;
                    default:
                        // Never happens
                        break;
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Error: {exception.Message}");
                Environment.Exit(Parser.DefaultExitCodeFail);
            }
        }
    }
}