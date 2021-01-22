using System.Threading.Tasks;

namespace forte.services
{
    /// <summary>
    ///     Scheduler collects and runs scheduled tasks
    /// </summary>
    public interface IScheduler
    {
        /// <summary>
        /// Run all overdue work coordinated by delegates
        /// </summary>
        /// <returns></returns>
        Task RunAsync();
    }

    /// <summary>
    /// Represents a scheduled work delegate, called by the scheduler periodically
    /// </summary>
    public interface IScheduledWorkDelegate
    {
        /// <summary>
        /// Run any overdue work specific to this delegate
        /// </summary>
        /// <returns></returns>
        Task RunAsync();
    }
}
