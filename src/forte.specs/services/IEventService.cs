using System;
using System.Threading.Tasks;
using forte.models.events;

namespace forte.services
{
    /// <summary>
    ///     Defines the service to manage application events
    /// </summary>
    public interface IEventService
    {
        /// <summary>
        ///     Publish an event
        /// </summary>
        /// <param name="event"></param>
        Task PublishAsync(EventModel @event, DateTime? sendTimeUtc = null);

        /// <summary>
        ///     Process event
        /// </summary>
        /// <param name="event"></param>
        Task ProcessAsync(EventModel @event);

        /// <summary>
        ///     Process event
        /// </summary>
        /// <param name="eventJson"></param>
        Task ProcessAsync(string eventJson);
    }
}
