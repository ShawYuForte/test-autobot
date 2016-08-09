using System;
using System.Threading.Tasks;
using forte.devices.models;

namespace forte.devices.services
{
    public interface IStreamingDevice
    {
        /// <summary>
        /// Fetches the next command, if available, from the server
        /// </summary>
        void FetchCommand();

        /// <summary>
        ///     Publish device state to the server
        /// </summary>
        bool PublishState();

        /// <summary>
        ///     Start streaming for the specified video stream identifier
        /// </summary>
        /// <param name="command"></param>
        bool StartStreaming(DeviceCommandModel command);

        /// <summary>
        ///     Prepares for streaming the specified video stream
        /// </summary>
        /// <param name="command"></param>
        bool PrepareForStream(DeviceCommandModel command);

        /// <summary>
        ///     Stop streaming for the specified video stream identifier. The video stream identifier is there to ensure that the
        ///     request is coming for the right stream
        /// </summary>
        /// <param name="command"></param>
        bool StopStreaming(DeviceCommandModel command);

        /// <summary>
        /// Get device config
        /// </summary>
        /// <returns></returns>
        StreamingDeviceConfig GetConfig();

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

        /// <summary>
        /// Send a message to the server
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        Task Send(string message);
    }

    public delegate void MessageReceivedDelegate(string message);
}