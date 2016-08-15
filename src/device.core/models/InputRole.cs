namespace forte.devices.models
{
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