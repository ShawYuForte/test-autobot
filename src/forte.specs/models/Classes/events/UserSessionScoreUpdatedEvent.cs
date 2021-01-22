using System;

namespace forte.models.classes.events
{
    public class UserSessionScoreUpdatedEvent : UserSessionEventBase
    {
        public UserSessionScoreUpdatedEvent()
        {
            EventType = UserSessionEventType.ScoresUpdated;
        }

        public Guid Id { get; set; }

        public int ScoreValue { get; set; }

        public string UserId { get; set; }

        public int AllSessionsTotalScores { get; set; }
    }
}
