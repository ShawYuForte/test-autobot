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