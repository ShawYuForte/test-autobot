namespace forte.device.models
{
    public enum Workflow
    {
        /// <summary>
        ///     Not started, not sure what the state of vMix is
        /// </summary>
        NotStarted,

        /// <summary>
        ///     User confirmed Azure Media Services configuration is correct
        /// </summary>
        AzureInformationConfirmed,

        /// <summary>
        ///     vMix preset has been loaded and verified
        /// </summary>
        PresetLoadVerified,

        /// <summary>
        ///     Static image loaded, ready for Azure Program to be started
        /// </summary>
        ReadyForAzure,

        /// <summary>
        ///     Started streaming
        /// </summary>
        Streaming,

        /// <summary>
        ///     Streaming was completed successfully
        /// </summary>
        CompletedSession
    }
}