using System;
using System.Threading.Tasks;

namespace forte.services
{
    public interface INotificationHub
    {
        /// <summary>
        ///     Notify clients of a server business event
        /// </summary>
        /// <param name="eventId"></param>
        /// <param name="data"></param>
        Task NotifyClientsOnEvent(Guid eventId, string data);
    }
}
