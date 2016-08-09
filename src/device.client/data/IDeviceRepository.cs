using System;
using System.Collections.Generic;
using forte.devices.entities;
using forte.devices.models;

namespace forte.devices.data
{
    public interface IDeviceRepository
    {
        /// <summary>
        ///     Get application settings
        /// </summary>
        /// <returns></returns>
        List<DeviceSetting> GetSettings();

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
        List<DeviceSetting> SaveSettings(List<DeviceSetting> settings);

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

        /// <summary>
        /// Save a particular setting value
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="setting"></param>
        /// <param name="value"></param>
        void SaveSetting<T>(string setting, T value);
    }
}