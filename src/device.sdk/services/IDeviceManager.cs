using System;

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
    }
}