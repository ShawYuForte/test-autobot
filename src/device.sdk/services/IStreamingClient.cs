using forte.devices.models;

namespace forte.devices.services
{
    /// <summary>
    ///     Represents a streaming client, a piece of software that does the stream to cloud publishing
    /// </summary>
    public interface IStreamingClient
    {
        /// <summary>
        /// Get current state of the client
        /// </summary>
        /// <returns></returns>
        StreamingClientState GetState();

        /// <summary>
        /// Loads the streaming software with presets based on the video stream provided
        /// </summary>
        /// <returns>Unique identifier for loaded preset</returns>
        /// <param name="videoStream"></param>
        string LoadVideoStreamPreset(VideoStreamModel videoStream);

        /// <summary>
        /// Starts streaming to the streaming service
        /// </summary>
        void StartStreaming();

        /// <summary>
        /// Starts the program, video and image intros, and playlist
        /// </summary>
        void StartProgram();

        /// <summary>
        /// Ends a broadcast, and optionally shuts down the streaming client software
        /// </summary>
        /// <param name="shutdownClient"></param>
        void StopStreaming(bool shutdownClient);

        /// <summary>
        /// Stops program by playing outro and stopping the playlist
        /// </summary>
        void StopProgram();
    }
}