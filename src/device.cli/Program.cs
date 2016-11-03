#region

using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using CommandLine;
using device.logging;
using device.web;
using device.web.server;
using forte.devices.commands;
using forte.devices.models;
using forte.devices.options;
using forte.devices.services;
using forte.devices.utils;
using forte.services;
using Microsoft.Practices.Unity;
using Newtonsoft.Json.Linq;

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