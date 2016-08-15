namespace forte.devices.models
{
    public class Audio
    {
        /// <summary>
        /// Overall volume setting
        /// </summary>
        public double Volume { get; set; }

        /// <summary>
        /// Is the audio muted
        /// </summary>
        public bool Muted { get; set; }

        /// <summary>
        /// The left channel audio level
        /// </summary>
        public double LeftChannel { get; set; }

        /// <summary>
        /// Right channel audio level
        /// </summary>
        public double RightChannel { get; set; }
    }
}