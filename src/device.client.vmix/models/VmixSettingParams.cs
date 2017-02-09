namespace forte.devices.models
{
    public static class VmixSettingParams
    {
        /// <summary>
        ///     Specifies whether the vMix error dialog is to be auto-closed
        /// </summary>
        public static string AutoCloseVmixErrorDialog => "auto-close-vmix-error-dialog";

        public static string BroadcastClosingImage => "broadcast-closing-image";
        public static string BroadcastClosingVideo => "broadcast-closing-video";
        public static string BroadcastOverlayImage => "broadcast-overlay-image";
        public static string BroadcastStartupImage => "broadcast-startup-image";
        public static string BroadcastStartupVideo => "broadcast-startup-video";

        /// <summary>
        ///     Specifies how many times vMix should retry to stream, in case of exception
        /// </summary>
        public static string RetryCountForStreamException => "retry-count-for-stream-exception";

        public static string VmixApiPath => "vmix-api-path";
        public static string VmixExePath => "vmix-exe-path";
        public static string VmixLoadTimeout => "vmix-load-timeout";
        public static string VmixPlaylistName => "vmix-playlist-name";
        public static string VmixPresetFolderPath => "vmix-preset-folder-path";
        public static string VmixPresetTemplateFilePath => "vmix-preset-template-file-path";
    }
}