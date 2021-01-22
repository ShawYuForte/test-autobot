using System.Threading.Tasks;
using forte.models.events;

namespace forte.services
{
    /// <summary>
    /// Defines an event processor.
    /// </summary>
    public interface IEventProcessor
    {
        /// <summary>
        /// Processes the published event if the processor supports it.
        /// </summary>
        /// <param name="eventModel"></param>
        Task<bool> ProcessAsync(EventModel eventModel);
    }
}
