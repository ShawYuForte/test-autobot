using System;
using forte.devices.entities;
using forte.devices.models;
using Settings = forte.devices.entities.Settings;

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

        /// <summary>
        ///     Get device state from the database
        /// </summary>
        /// <returns></returns>
        StreamingDeviceState GetDeviceState();

        /// <summary>
        /// Save streaming device state
        /// </summary>
        /// <param name="deviceState"></param>
        void Save(StreamingDeviceState deviceState);

        /// <summary>
        /// Save command to local repo
        /// </summary>
        /// <param name="deviceCommandEntity"></param>
        /// <returns></returns>
        DeviceCommandEntity SaveCommand(DeviceCommandEntity deviceCommandEntity);
    }
}