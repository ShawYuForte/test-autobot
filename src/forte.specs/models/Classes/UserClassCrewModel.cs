using System;

namespace forte.models.classes
{
    public class UserClassCrewModel
    {
        public DateTime Created { get; set; }

        public Guid Id { get; set; }

        /// <summary>
        /// The user ID, which this attribute belongs to.
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// The user ID, which is added to the user`s crew
        /// </summary>
        public string CrewUserId { get; set; }
    }
}
