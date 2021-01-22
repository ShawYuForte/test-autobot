using System;

namespace forte.models.classes
{
    /// <summary>
    /// The watched session model
    /// </summary>
    public class WatchedSessionModel
    {
        public DateTime WatchedOn { get; set; }

        public SessionModel Session { get; set; }

        public string UserId { get; set; }

        public string UserName { get; set; }

        public Guid Id { get; set; }
    }
}
