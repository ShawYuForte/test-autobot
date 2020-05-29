#region

using System;
using forte.devices.models;

#endregion

namespace forte.devices.services
{
    /// <summary>
    ///     Represents a streaming client, a piece of software that does the stream to cloud publishing
    /// </summary>
    public interface IStreamingClient
    {
        /// <summary>
        ///     Get current state of the client
        /// </summary>
        /// <returns></returns>
        StreamingClientState GetState();

        /// <summary>
        ///     Loads the streaming software with presets based on the video stream provided
        /// </summary>
        /// <returns>Unique identifier for loaded preset</returns>
        /// <param name="videoStream"></param>
        string LoadVideoStreamPreset(VideoStreamModel videoStream, string preset);

        /// <summary>
        ///     Shut the client down
        /// </summary>
        void ShutDown();

        /// <summary>
        ///     Starts the program, video and image intros, and playlist
        /// </summary>
        void StartProgram(DateTime? time = null);

        /// <summary>
        ///     Starts streaming to the streaming service
        /// </summary>
        void StartStreaming();

        /// <summary>
        ///     Stops program by playing outro and stopping the playlist
        /// </summary>
        void StopProgram(DateTime? time = null);

        /// <summary>
        ///     Ends a broadcast, and optionally shuts down the streaming client software
        /// </summary>
        /// <param name="shutdownClient"></param>
        void StopStreaming(bool shutdownClient);
    }
}