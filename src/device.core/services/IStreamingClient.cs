#region

using System;
using System.Threading.Tasks;
using forte.devices.models;

#endregion

namespace forte.devices.services
{
    /// <summary>
    ///     Represents a streaming client, a piece of software that does the stream to cloud publishing
    /// </summary>
    public interface IStreamingClient
    {
		Task<string> LoadVideoStreamPreset(string preset, string primaryUrl);
		void StartStreaming();
		void StopStreaming(bool shutdownClient);
		void StartProgram();
		void StopProgram();
    }
}