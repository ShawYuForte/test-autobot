#region

using device.logging.sinks.signalr;
using forte.services;
using Microsoft.AspNet.SignalR;
using Serilog;

#endregion

namespace device.logging.services
{
    public class SeriLoggerEx : SeriLogger, IDeviceLogger
    {
        private LoggerConfiguration _loggerConfiguration;

        public void ConfigureSignalRSink(IHubContext hubContext)
        {
            _loggerConfiguration.WriteTo.SignalR(hubContext);
            Log.Logger = _loggerConfiguration.CreateLogger();
        }

        protected override LoggerConfiguration ConfigureSerilog()
        {
            _loggerConfiguration = base.ConfigureSerilog();
            return _loggerConfiguration;
        }
    }
}