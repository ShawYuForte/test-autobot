using System;

namespace forte.models.classes
{
    public class CreateUserActivityModel
    {
        public string UserId { get; set; }

        public string AddressedToUserId { get; set; }

        public UserActivityType UserActivityType { get; set; }

        public Guid? SessionId { get; set; }

        /// <summary>
        ///   Contains message for MessageShared UserActivityType and Badge name for BadgeEarned UserActivityType
        /// </summary>
        public string Comment { get; set; }
    }
}
