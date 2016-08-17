#region

using forte.devices.models;

#endregion

namespace forte.devices.services
{
    public interface IDeviceDaemon
    {
        /// <summary>
        ///     Fetches the next command, if available, from the server
        /// </summary>
        void QueryServer();

        /// <summary>
        ///     Retrieve current device state
        /// </summary>
        /// <returns></returns>
        StreamingDeviceState GetState();

        /// <summary>
        ///     Publish device state to the server
        /// </summary>
        bool PublishState();

        /// <summary>
        ///     Run and block
        /// </summary>
        void Run();

        /// <summary>
        ///     Stop the blocking daemon
        /// </summary>
        void Stop();
    }
}