namespace forte.device.models
{
    public enum Workflow
    {
        /// <summary>
        ///     Not started, not sure what the state of vMix is
        /// </summary>
        NotStarted,

        /// <summary>
        ///     User confirmed configuration settings are correct
        /// </summary>
        SettingsConfirmed,

        /// <summary>
        /// Program has been created, vMix is broadcasting, ready for class to start
        /// </summary>
        ReadyToStartProgram,

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