using System;

namespace forte.models.classes
{
    public class WatchedHistoryPageModel : ResultPage<WatchedSessionModel>
    {
    }

    public class WatchedHistoryRequestFilter : RequestFilter
    {
        public string UserId { get; set; }

        public Guid? SessionId { get; set; }
    }
}
