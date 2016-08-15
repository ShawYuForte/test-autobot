namespace forte.devices.models
{
    public enum ExecutionStatus
    {
        /// <summary>
        ///     Command has been received
        /// </summary>
        Received = 0,

        /// <summary>
        ///     Command has been executed successfully
        /// </summary>
        Executed = 1,

        /// <summary>
        ///     Command execution failed
        /// </summary>
        Failed = 2
    }
}