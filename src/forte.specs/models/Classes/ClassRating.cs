using System;

namespace forte.models.classes
{
    public class ClassRating
    {
        public Guid ClassId { get; set; }

        public string Comments { get; set; }

        public Guid Id { get; set; }

        public int Rating { get; set; }

        public Guid? SessionAttendanceId { get; set; }

        public Guid? SessionId { get; set; }

        public string UserId { get; set; }

        public string UserName { get; set; }

        public DateTime Created { get; set; }

        public byte[] Version { get; set; }
    }

    public class ClassRatingFilter : RequestFilter
    {
        public bool AllUsers { get; set; }

        public Guid? ClassId { get; set; }

        public Guid? SessionId { get; set; }
    }
}
