#region

using System.IO;
using device.logging.sinks.signalr;
using forte.devices.config;
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
        private readonly IRuntimeConfig _cf;
		private readonly IHubContext _hubContext;

		public SeriLoggerEx
		(
			IRuntimeConfig cf,
			IHubContext hubContext
		)
        {
			_cf = cf;
			_hubContext = hubContext;
		}

        protected override LoggerConfiguration ConfigureSinks()
        {
			var logPath = _cf.LogPath;

            if (!string.IsNullOrWhiteSpace(logPath))
            {
				if(!Directory.Exists(logPath))
				{
					Directory.CreateDirectory(logPath);
				}

                FileSinkPattern = $"{logPath}\\device-{{Date}}.log";
            }

            _loggerConfiguration = base.ConfigureSinks();
			_loggerConfiguration.WriteTo.SignalR(_hubContext);

			return _loggerConfiguration;
        }
    }
}