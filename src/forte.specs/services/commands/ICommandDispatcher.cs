using System;
using System.Threading.Tasks;
using forte.models.commands;
using forte.models.events;

namespace forte.services.commands
{
    /// <summary>
    ///     Dispatches async commands based on target
    /// </summary>
    public interface ICommandDispatcher
    {
        /// <summary>
        ///     Dispatch command based on business rules
        /// </summary>
        /// <param name="commandJson"></param>
        Task DispatchAsync(string commandJson);

        /// <summary>
        ///     Dispatch command based on business rules
        /// </summary>
        /// <param name="command"></param>
        Task DispatchAsync(Command command);

        /// <summary>
        ///     Dispatch all overdue commands
        /// </summary>
        /// <returns></returns>
        Task DispatchAllOverdueAsync();

        /// <summary>
        ///     Dispatch all triggered by event
        /// </summary>
        /// <returns></returns>
        Task DispatchAllTriggeredByAsync(EventModel eventModel);

        /// <summary>
        /// Register a command with the dispatcher
        /// </summary>
        /// <param name="commandTypeId"></param>
        /// <param name="commandType"></param>
        void RegisterCommand(Guid commandTypeId, Type commandType);

        /// <summary>
        /// Register a command with the dispatcher
        /// </summary>
        /// <param name="commandTypeId"></param>
        /// <param name="commandDelegate"></param>
        void RegisterDelegate(Guid commandTypeId, ICommandDelegate commandDelegate);
    }
}
