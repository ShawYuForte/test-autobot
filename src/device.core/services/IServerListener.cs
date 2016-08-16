using System;

namespace forte.devices.services
{
    public interface IServerListener : IDisposable
    {
        /// <summary>
        /// Message has been received from the server
        /// </summary>
        event MessageReceivedDelegate MessageReceived;

        /// <summary>
        /// Explicitly connect to the server
        /// </summary>
        /// <returns></returns>
        void Connect();

        /// <summary>
        /// Explicitly disconnect from the server
        /// </summary>
        void Disconnect();
    }

    public delegate void MessageReceivedDelegate(string message);
}