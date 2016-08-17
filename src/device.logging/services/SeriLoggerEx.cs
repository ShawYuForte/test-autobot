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
    public class SeriLoggerEx : SeriLogger
    {
        private LoggerConfiguration _loggerConfiguration;
        private readonly IRuntimeConfig _runtimeConfig;
        private readonly IHubContext _hubContext;

        public SeriLoggerEx(IRuntimeConfig runtimeConfig, IHubContext hubContext)
        {
            _runtimeConfig = runtimeConfig;
            _hubContext = hubContext;
        }

        protected override LoggerConfiguration ConfigureSinks()
        {
            if (!string.IsNullOrWhiteSpace(_runtimeConfig.LogPath))
            {
                if (!Directory.Exists(_runtimeConfig.LogPath))
                    Directory.CreateDirectory(_runtimeConfig.LogPath);
                FileSinkPattern = $"{_runtimeConfig.LogPath}\\device-{{Date}}.log";
            }

            _loggerConfiguration = base.ConfigureSinks();
            _loggerConfiguration.WriteTo.SignalR(_hubContext);

            return _loggerConfiguration;
        }
    }
}