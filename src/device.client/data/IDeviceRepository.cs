using System;
using forte.devices.entities;

namespace forte.devices.data
{
    public interface IDeviceRepository
    {
        /// <summary>
        ///     Get application settings
        /// </summary>
        /// <returns></returns>
        Settings GetSettings();

        /// <summary>
        ///     Get application settings
        /// </summary>
        /// <returns></returns>
        DeviceConfig GetDeviceConfig();

        /// <summary>
        ///     Save settings
        /// </summary>
        /// <param name="settings"></param>
        /// <returns></returns>
        Settings SaveSettings(Settings settings);

        /// <summary>
        ///     Get video stream information
        /// </summary>
        /// <param name="videoStreamId"></param>
        /// <returns></returns>
        VideoStream GetVideoStream(Guid videoStreamId);

        /// <summary>
        ///     Save video stream information
        /// </summary>
        /// <param name="videoStream"></param>
        /// <returns></returns>
        VideoStream SaveVideoStream(VideoStream videoStream);
    }
}