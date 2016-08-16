#region

using System.IO;
using device.logging.sinks.signalr;
using forte.devices.services;
using forte.services;
using Microsoft.AspNet.SignalR;
using Serilog;

#endregion

namespace device.logging.services
{
    public class SeriLoggerEx : SeriLogger, IDeviceLogger
    {
        private LoggerConfiguration _loggerConfiguration;
        private readonly IRuntimeConfig _runtimeConfig;

        public SeriLoggerEx(IRuntimeConfig runtimeConfig)
        {
            _runtimeConfig = runtimeConfig;
        }

        public void ConfigureSignalRSink(IHubContext hubContext)
        {
            _loggerConfiguration.WriteTo.SignalR(hubContext);
            Log.Logger = _loggerConfiguration.CreateLogger();
        }

        protected override LoggerConfiguration ConfigureSerilog()
        {
            if (!string.IsNullOrWhiteSpace(_runtimeConfig.LogPath))
            {
                if (!Directory.Exists(_runtimeConfig.LogPath))
                    Directory.CreateDirectory(_runtimeConfig.LogPath);
                FileSinkPattern = $"{_runtimeConfig.LogPath}\\device-{{Date}}.log";
            }
            _loggerConfiguration = base.ConfigureSerilog();
            return _loggerConfiguration;
        }
    }
}