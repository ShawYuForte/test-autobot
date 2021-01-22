using System;
using System.Threading.Tasks;
using forte.models.events;

namespace forte.services
{
    /// <summary>
    ///     Sends out notifications through the configured channels (e.g. Email)
    /// </summary>
    public interface INotificationService
    {
        /// <summary>
        ///     Notify subscribers of the provided event
        /// </summary>
        /// <param name="eventModel"></param>
        /// <returns></returns>
        Task NotifySubscribersAsync(EventModel eventModel);

        /// <summary>
        /// Register notification template for a particular event type
        /// </summary>
        /// <param name="eventTypeId"></param>
        /// <param name="templateId"></param>
        /// <returns></returns>
        Task RegisterTemplate(Guid eventTypeId, Guid templateId);
    }
}
