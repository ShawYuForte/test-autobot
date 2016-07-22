using System;
using System.Threading.Tasks;
using forte.devices.models;

namespace forte.devices.services
{
    public interface IDeviceManager
    {
        /// <summary>
        ///     Publish device state to the server
        /// </summary>
        void PublishState();

        /// <summary>
        ///     Start streaming for the specified video stream identifier
        /// </summary>
        /// <param name="videoStreamId"></param>
        void StartStreaming(Guid videoStreamId);

        /// <summary>
        ///     Stop streaming for the specified video stream identifier. The video stream identifier is there to ensure that the
        ///     request is coming for the right stream
        /// </summary>
        /// <param name="videoStreamId"></param>
        void StopStreaming(Guid videoStreamId);

        /// <summary>
        /// Get app settings
        /// </summary>
        /// <returns></returns>
        Settings GetSettings();

        /// <summary>
        /// Get device config
        /// </summary>
        /// <returns></returns>
        DeviceConfig GetConfig();

        /// <summary>
        /// Message has been received from the server
        /// </summary>
        event MessageReceivedDelegate MessageReceived;

        /// <summary>
        /// Explicitly connect to the server
        /// </summary>
        /// <returns></returns>
        Task Connect();

        /// <summary>
        /// Explicitly disconnect from the server
        /// </summary>
        void Disconnect();

        /// <summary>
        /// Send a message to the server
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        Task Send(string message);
    }

    public delegate void MessageReceivedDelegate(string message);
}