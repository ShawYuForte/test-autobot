using System;
using System.Configuration;
using System.Data.Entity.Infrastructure.Interception;
using System.Net;
using Serilog;
using Serilog.Configuration;
using Serilog.Debugging;
using Serilog.Events;

namespace forte.devices.services
{
    public class SeriLogger : ILogger
    {
        public SeriLogger()
        {
            ConfigureSerilog();
        }

        public void Debug(string messageTemplate, params object[] propertyValues)
        {
            Log.Debug(messageTemplate, propertyValues);
        }

        public void Error(string messageTemplate, params object[] propertyValues)
        {
            Log.Error(messageTemplate, propertyValues);
        }

        public void Error(Exception exception, string messageTemplate, params object[] propertyValues)
        {
            Log.Error(exception, messageTemplate, propertyValues);
        }

        public void Fatal(string messageTemplate, params object[] propertyValues)
        {
            Log.Fatal(messageTemplate, propertyValues);
        }

        public void Fatal(Exception exception, string messageTemplate, params object[] propertyValues)
        {
            Log.Fatal(exception, messageTemplate, propertyValues);
        }

        public void Warning(string messageTemplate, params object[] propertyValues)
        {
            Log.Warning(messageTemplate, propertyValues);
        }

        public void Warning(Exception exception, string messageTemplate, params object[] propertyValues)
        {
            Log.Warning(exception, messageTemplate, propertyValues);
        }

        private void ConfigureSerilog()
        {
            //SelfLog.Out = Console.Out;
            var configuration = new LoggerConfiguration();

            ConfigureMinimumLogLevel(configuration.MinimumLevel);
            ConfigureRollingFileSink(configuration);
            ConfigureColoredConsoleSink(configuration);
            ConfigureEmailSink(configuration);

            Log.Logger = configuration.CreateLogger();
        }

        private void ConfigureEmailSink(LoggerConfiguration configuration)
        {
            var mailServer = ConfigurationManager.AppSettings["forte:modules:serilog.sinks.email.server"];

            if (string.IsNullOrWhiteSpace(mailServer)) return;

            // Create network credentials to access your SendGrid account
            var username = ConfigurationManager.AppSettings["forte:modules:serilog.sinks.email.user-name"];
            var password = ConfigurationManager.AppSettings["forte:modules:serilog.sinks.email.password"];

            LogEventLevel restrictedToMinimumLevel = LogEventLevel.Fatal;

            Enum.TryParse(ConfigurationManager.AppSettings["forte:modules:serilog.sinks.email.min-level"],
                ignoreCase: true, result: out restrictedToMinimumLevel);

            var from = ConfigurationManager.AppSettings["forte:ops.email"];
            var to = from;

            var credentials = new NetworkCredential(username, password);

            configuration.WriteTo.Email(
                    fromEmail: from,
                    toEmail: to,
                    mailServer: mailServer,
                    networkCredential: credentials,
                    restrictedToMinimumLevel: restrictedToMinimumLevel);
        }

        private void ConfigureColoredConsoleSink(LoggerConfiguration configuration)
        {
            var consoleConfigured = false;

            bool.TryParse(ConfigurationManager.AppSettings["forte:modules:serilog.sinks.console"], out consoleConfigured);

            if (!consoleConfigured) return;

            configuration.WriteTo.ColoredConsole();
        }

        private void ConfigureRollingFileSink(LoggerConfiguration configuration)
        {
            var fileSizeLimitBytes = 4096;
            var pathFormat = ConfigurationManager.AppSettings["forte:modules:serilog.sinks.file.pattern"];

            if (string.IsNullOrWhiteSpace(pathFormat)) return;

            if (int.TryParse(ConfigurationManager.AppSettings["forte:modules:serilog.sinks.file.max-size"], out fileSizeLimitBytes))
                configuration.WriteTo.RollingFile(pathFormat, fileSizeLimitBytes: fileSizeLimitBytes);
            else
                configuration.WriteTo.RollingFile(pathFormat);
        }

        /// <summary>
        ///     Sets the minimum log level from configuration file.
        /// </summary>
        /// <param name="minimumLevel">The minimum level.</param>
        /// <returns>The fluent reference to configuration.</returns>
        public static LoggerConfiguration ConfigureMinimumLogLevel(LoggerMinimumLevelConfiguration minimumLevel)
        {
            try
            {
                var logLevel = ConfigurationManager.AppSettings["forte:modules:serilog.min-log-level"] ?? "warn";
                switch (logLevel.ToLower())
                {
                    case "debug":
                        return minimumLevel.Debug();
                    case "info":
                        return minimumLevel.Information();
                    case "warn":
                        return minimumLevel.Warning();
                    case "error":
                        return minimumLevel.Error();
                    case "fatal":
                        return minimumLevel.Fatal();
                }
            }
            catch (Exception exception)
            {
                SelfLog.WriteLine(exception.ToString());
            }
            // Default level is warnning
            return minimumLevel.Warning();
        }

        public void Dispose()
        {
            try
            {
                var disposable = Log.Logger as IDisposable;
                if (disposable == null) return;
                disposable.Dispose();
            }
            catch
            {
            }
        }
    }
}