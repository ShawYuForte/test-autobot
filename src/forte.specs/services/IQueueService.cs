using System;
using System.Threading.Tasks;

namespace forte.services
{
    public interface IQueueService
    {
        /// <summary>
        ///     Default queue name
        /// </summary>
        string DefaultName { get; }

        /// <summary>
        ///     Queue a message to default queue
        /// </summary>
        /// <param name="message"></param>
        Task QueueAsync(string message, DateTime? sendTimeUtc = null);

        /// <summary>
        ///     Queue a message to specified queue
        /// </summary>
        /// <param name="message"></param>
        /// <param name="queue"></param>
        Task QueueAsync(string message, string queue, DateTime? sendTimeUtc = null);

        /// <summary>
        ///     Queue object to default queue
        /// </summary>
        /// <typeparam name="T"></typeparam>
        Task QueueAsync<T>(T obj, DateTime? sendTimeUtc = null);

        /// <summary>
        ///     Queue object to default queue
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="queue"></param>
        Task QueueAsync<T>(T obj, string queue, DateTime? sendTimeUtc = null);
    }
}
