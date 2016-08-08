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
        /// <param name="videoStream"></param>
        void LoadVideoStreamPreset(VideoStreamModel videoStream);
    }
}