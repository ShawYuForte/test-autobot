namespace forte.device.models
{
    public class VMixInput
    {
        public string Key { get; set; }
        public string Number { get; set; }
        public string Type { get; set; }
        public string Title { get; set; }
        public string State { get; set; }
        public InputRole Role { get; set; }
    }

    public enum InputRole
    {
        /// <summary>
        ///     No defined role
        /// </summary>
        None = 0,

        /// <summary>
        ///     Opening static (background) image
        /// </summary>
        OpeninStaticImage,

        /// <summary>
        ///     Opening video
        /// </summary>
        OpeningVideo,

        /// <summary>
        ///     Logo overlay (to be cut within the camera stream)
        /// </summary>
        LogoOverlay,

        /// <summary>
        ///     One of the cameras
        /// </summary>
        Camera,

        /// <summary>
        ///     Closing video
        /// </summary>
        ClosingVideo,

        /// <summary>
        ///     Closing static (background) image
        /// </summary>
        ClosingStaticImage,

        /// <summary>
        ///     Audio
        /// </summary>
        Audio
    }
}