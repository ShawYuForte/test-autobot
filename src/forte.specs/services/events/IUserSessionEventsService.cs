using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using forte.models.classes.events;

namespace forte.services.events
{
    public interface IUserSessionEventsService
    {
        Task PushEventToStorageAsync(params UserSessionEventBase[] eventData);

        Task<IReadOnlyCollection<UserSessionEventBase>> GetAllUserSessionEventsAsync(Guid sessionId);
    }
}
