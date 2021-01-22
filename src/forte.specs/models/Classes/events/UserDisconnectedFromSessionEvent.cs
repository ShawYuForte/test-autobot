namespace forte.models.classes.events
{
    public class UserDisconnectedFromSessionEvent : UserSessionEventBase
    {
        public UserDisconnectedFromSessionEvent()
        {
            EventType = UserSessionEventType.UserDisconnected;
        }

        public string UserId { get; set; }
    }
}
