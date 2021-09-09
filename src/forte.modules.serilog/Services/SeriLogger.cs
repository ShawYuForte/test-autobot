using System;
using System.Configuration;
using System.Data.Entity.Infrastructure.Interception;
using System.Net;
using System.Threading;
using forte.data;
using forte.EventLog;
using Microsoft.WindowsAzure.Storage;
using Serilog;
using Serilog.Configuration;
using Serilog.Debugging;
using Serilog.Events;
using Serilog.Sinks.MSSqlServer;

namespace forte.services
{
    public class SeriLogger : ILogger
    {
        private static readonly object LoggerLock = new object();
        private static ILogger _logger;

        public SeriLogger()
        {
            Logger = this;
        }

        public static ILogger Logger
        {
            get
            {
                lock (LoggerLock)
                {
                    if (_logger != null)
                    {
                        return _logger;
                    }

                    var logger = new SeriLogger();
                    logger.Configure();
                    _logger = logger;
                    return _logger;
                }
            }

            private set
            {
                lock (LoggerLock)
                {
                    _logger = value;
                }
            }
        }

        public bool? ColoredConsoleSink { get; set; }

        public int? FileSinkMaxSize { get; set; }

        public string FileSinkPattern { get; set; }

        public bool? LogEntityFrameworkCalls { get; set; }

        public string SqlSeverSinkConnectionString { get; set; }

        public LogEventLevel? SqlSeverSinkMinLevel { get; set; }

        protected string EmailServerSink { get; set; }

        protected string EmailServerSinkFromAddress { get; set; }

        protected string EmailServerSinkMinLevel { get; set; }

        protected string EmailServerSinkPassword { get; set; }

        protected string EmailServerSinkToAddress { get; set; }

        protected string EmailServerSinkUsername { get; set; }

        /// <summary>
        ///     Logs to storage if configured in config file.
        /// </summary>
        /// <param name="config">The configuration.</param>
        /// <returns>The fluent reference to configuration.</returns>
        public static LoggerConfiguration ConfigureAzureStorageSink(LoggerConfiguration config)
        {
            try
            {
                var logStorageConnectionName =
                    ConfigurationManager.AppSettings["forte:modules:serilog.sinks.azure.connection-name"];
                if (!string.IsNullOrEmpty(logStorageConnectionName))
                {
                    var connectionString =
                        ConfigurationManager.ConnectionStrings[logStorageConnectionName].ConnectionString;
                    var storageAccount = CloudStorageAccount.Parse(connectionString);

                    config = config.WriteTo.AzureTableStorage(storageAccount);
                }
            }
            catch (Exception exception)
            {
                SelfLog.WriteLine(exception.ToString());
            }
            return config;
        }

        public void Dispose()
        {
            try
            {
                var disposable = Log.Logger as IDisposable;
                if (disposable == null)
                {
                    return;
                }

                disposable.Dispose();
            }
            catch
            {
                // ignored
            }
        }

        public void Debug(string messageTemplate, params object[] propertyValues)
        {
            var id = Thread.CurrentThread.ManagedThreadId;
            Log.Debug($"{messageTemplate} [_tid:{id}]", propertyValues);
        }

        public void Debug(Exception exception, string messageTemplate, params object[] propertyValues)
        {
            var id = Thread.CurrentThread.ManagedThreadId;
            Log.Debug(exception, $"{messageTemplate} [_tid:{id}]", propertyValues);
        }

        public void Error(string messageTemplate, params object[] propertyValues)
        {
            var id = Thread.CurrentThread.ManagedThreadId;
            Log.Error($"{messageTemplate} [_tid:{id}]", propertyValues);
        }

        public void Error(Exception exception, string messageTemplate, params object[] propertyValues)
        {
            var id = Thread.CurrentThread.ManagedThreadId;
            Log.Error(exception, $"{messageTemplate} [_tid:{id}]", propertyValues);
        }

        public void Fatal(string messageTemplate, params object[] propertyValues)
        {
            var id = Thread.CurrentThread.ManagedThreadId;
            Log.Fatal($"{messageTemplate} [_tid:{id}]", propertyValues);
        }

        public void Fatal(Exception exception, string messageTemplate, params object[] propertyValues)
        {
            var id = Thread.CurrentThread.ManagedThreadId;
            Log.Fatal(exception, $"{messageTemplate} [_tid:{id}]", propertyValues);
        }

        public void Information(string messageTemplate, params object[] propertyValues)
        {
            var id = Thread.CurrentThread.ManagedThreadId;
            Log.Information($"{messageTemplate} [_tid:{id}]", propertyValues);
        }

        public void Warning(string messageTemplate, params object[] propertyValues)
        {
            var id = Thread.CurrentThread.ManagedThreadId;
            Log.Warning($"{messageTemplate} [_tid:{id}]", propertyValues);
        }

        public void Warning(Exception exception, string messageTemplate, params object[] propertyValues)
        {
            var id = Thread.CurrentThread.ManagedThreadId;
            Log.Warning(exception, $"{messageTemplate} [_tid:{id}]", propertyValues);
        }

        public void Configure()
        {
            var configuration = ConfigureSinks();
            Log.Logger = configuration.CreateLogger();
        }

        /// <summary>
        ///     Sets the minimum log level from configuration file.
        /// </summary>
        /// <param name="minimumLevel">The minimum level.</param>
        /// <returns>The fluent reference to configuration.</returns>
        protected static LoggerConfiguration ConfigureMinimumLogLevel(LoggerMinimumLevelConfiguration minimumLevel)
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

        protected virtual LoggerConfiguration ConfigureSinks()
        {
            var configuration = new LoggerConfiguration();

            ConfigureMinimumLogLevel(configuration.MinimumLevel);
            ConfigureEntityFrameworkInterceptor(configuration);
            ConfigureSqlStorageSink(configuration);
            ConfigureRollingFileSink(configuration);
            ConfigureColoredConsoleSink(configuration);
            ConfigureEmailSink(configuration);
            ConfigureWindowEventSink(configuration);

            return configuration;
        }

        private void ConfigureColoredConsoleSink(LoggerConfiguration configuration)
        {
            bool consoleConfigured;

            bool.TryParse(ConfigurationManager.AppSettings["forte:modules:serilog.sinks.console"], out consoleConfigured);

            if (!consoleConfigured && (!ColoredConsoleSink.HasValue || !ColoredConsoleSink.Value))
            {
                return;
            }

            configuration.WriteTo.ColoredConsole();
        }

        private void ConfigureEmailSink(LoggerConfiguration configuration)
        {
            var mailServer = EmailServerSink ??
                             ConfigurationManager.AppSettings["forte:modules:serilog.sinks.email.server"];

            if (string.IsNullOrWhiteSpace(mailServer))
            {
                return;
            }

            // Create network credentials to access your SendGrid account
            var username = EmailServerSinkUsername ??
                           ConfigurationManager.AppSettings["forte:modules:serilog.sinks.email.user-name"];
            var password = EmailServerSinkPassword ??
                           ConfigurationManager.AppSettings["forte:modules:serilog.sinks.email.password"];

            // ReSharper disable once RedundantAssignment
            var restrictedToMinimumLevel = LogEventLevel.Fatal;

            Enum.TryParse(
                EmailServerSinkMinLevel ?? ConfigurationManager.AppSettings["forte:modules:serilog.sinks.email.min-level"],
                true,
                out restrictedToMinimumLevel);

            var from = EmailServerSinkFromAddress ?? ConfigurationManager.AppSettings["forte:ops.email"];
            var to = EmailServerSinkToAddress ?? from;

            var credentials = new NetworkCredential(username, password);

            configuration.WriteTo.Email(
                from,
                to,
                mailServer,
                credentials,
                restrictedToMinimumLevel: restrictedToMinimumLevel);
        }

        /// <summary>
        ///     Configures logging of the Entity Framework queries.
        /// </summary>
        /// <param name="configuration">The logging configuration.</param>
        /// <returns>The fluent reference to configuration.</returns>
        // ReSharper disable once UnusedMethodReturnValue.Local
        private LoggerConfiguration ConfigureEntityFrameworkInterceptor(LoggerConfiguration configuration)
        {
            bool logEntityFrameworkCalls = LogEntityFrameworkCalls ?? bool.TryParse(
                                               ConfigurationManager.AppSettings[
                                                   "forte:modules:serilog.intercept-entity-framework"],
                                               out logEntityFrameworkCalls) && logEntityFrameworkCalls;

            if (logEntityFrameworkCalls)
            {
                DbInterception.Add(new ForteEntityFrameworkLoggingInterceptor(this));
            }

            return configuration;
        }

        private void ConfigureRollingFileSink(LoggerConfiguration configuration)
        {
            var fileSizeLimitBytes = 4096;
            var pathFormat = FileSinkPattern ??
                             ConfigurationManager.AppSettings["forte:modules:serilog.sinks.file.pattern"];

            if (string.IsNullOrWhiteSpace(pathFormat))
            {
                return;
            }

            if (FileSinkMaxSize.HasValue ||
                int.TryParse(
                    ConfigurationManager.AppSettings["forte:modules:serilog.sinks.file.max-size"],
                    out fileSizeLimitBytes))
            {
                configuration.WriteTo.RollingFile(pathFormat, fileSizeLimitBytes: FileSinkMaxSize ?? fileSizeLimitBytes);
            }
            else
            {
                configuration.WriteTo.RollingFile(pathFormat);
            }
        }

        private void ConfigureSqlStorageSink(LoggerConfiguration configuration)
        {
            var connectionString = SqlSeverSinkConnectionString ??
                                   ConfigurationManager.AppSettings["forte:modules:serilog.sinks.sql.connection-name"];
            var tableName = ConfigurationManager.AppSettings["forte:modules:serilog.sinks.sql.table-name"] ?? "opslogs";

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(SqlSeverSinkConnectionString) &&
                (ConfigurationManager.ConnectionStrings[connectionString] != null))
            {
                connectionString = ConfigurationManager.ConnectionStrings[connectionString].ConnectionString;
            }

            // ReSharper disable once RedundantAssignment
            var restrictedToMinimumLevel = LogEventLevel.Warning;

            if (!Enum.TryParse(
                ConfigurationManager.AppSettings["forte:modules:serilog.sinks.sql.min-level"],
                true,
                out restrictedToMinimumLevel))
            {
                Enum.TryParse(
                    ConfigurationManager.AppSettings["forte:modules:serilog.min-log-level"],
                    true,
                    out restrictedToMinimumLevel);
            }

            var columnOptions = new ColumnOptions();

            // Don't include the Properties XML column.
            columnOptions.Store.Remove(StandardColumn.Properties);

            // Do include the log event data as JSON.
            columnOptions.Store.Add(StandardColumn.LogEvent);

            configuration.WriteTo.MSSqlServer(
                connectionString,
                tableName,
                autoCreateSqlTable: true,
                batchPostingLimit: 5,
                restrictedToMinimumLevel: SqlSeverSinkMinLevel ?? restrictedToMinimumLevel,
                columnOptions: columnOptions);
        }

        private void ConfigureWindowEventSink(LoggerConfiguration configuration)
        {
            if (!Enum.TryParse(
              ConfigurationManager.AppSettings["forte:modules:serilog.sinks.windows.min-level"],
              true,
              out LogEventLevel restrictedToMinimumLevel))
            {
                Enum.TryParse(
                    ConfigurationManager.AppSettings["forte:modules:serilog.min-log-level"],
                    true,
                    out restrictedToMinimumLevel);
            }

            configuration.WriteTo.Logger(win => win.WriteTo.EventLog(
                                       ConfigurationManager.AppSettings["forte:modules:serilog.sinks.windows.source"],
                                       manageEventSource: true,
                                       restrictedToMinimumLevel: restrictedToMinimumLevel,
                                       eventIdProvider: new DeviceEventIdProvider())
                                      .Filter.ByExcluding(
                                       ConfigurationManager.AppSettings["forte:modules:serilog.sinks.windows:filter:ByExcluding.expression"]));
        }
    }
}
