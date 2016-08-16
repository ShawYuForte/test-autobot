#region

using forte.services;
using Microsoft.AspNet.SignalR;

#endregion

namespace device.logging.services
{
    public interface IDeviceLogger : ILogger
    {
        /// <summary>
        ///     Configure signalr sink
        /// </summary>
        /// <param name="hubContext"></param>
        void ConfigureSignalRSink(IHubContext hubContext);
    }
}