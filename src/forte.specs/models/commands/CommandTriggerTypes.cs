namespace Forte.Domains.Commands.Entities
{
    public enum CommandTriggerTypes
    {
        /// <summary>
        ///     Command is executed as soon as it's received
        /// </summary>
        Immediate = 0,

        /// <summary>
        ///     Command is executed on a schedule
        /// </summary>
        Scheduled,

        /// <summary>
        ///     Command is executed based on a fired event
        /// </summary>
        EventBased,
    }
}
