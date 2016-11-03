#region

using System;
using forte.devices.models;

#endregion

namespace forte.devices.services
{
    public interface IDeviceDaemon
    {
        void Init();
        void Init(Guid deviceId);

        /// <summary>
        ///     Force resets the device to idle, closing the streaming client in the process
        /// </summary>
        void ForceResetToIdle();

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
        ///     Fetches the next command, if available, from the server
        /// </summary>
        void QueryServer();

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