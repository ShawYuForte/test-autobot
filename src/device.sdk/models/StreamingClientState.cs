using System.Collections.Generic;

namespace forte.devices.models
{
    public class StreamingClientState
    {
        /// <summary>
        ///     The name of the streaming client software
        /// </summary>
        public string Software { get; set; }

        /// <summary>
        ///     Current version of the streaming client software
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        ///     Is the client recording
        /// </summary>
        public bool Recording { get; set; }

        /// <summary>
        ///     Is the client streaming
        /// </summary>
        public bool Streaming { get; set; }

        /// <summary>
        ///     All active inputs
        /// </summary>
        public IList<Input> ActiveInputs { get; set; }

        /// <summary>
        ///     Master audio settings
        /// </summary>
        public Audio MasterAudio { get; set; }
    }
}