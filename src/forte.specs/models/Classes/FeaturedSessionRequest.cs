using System;

namespace forte.models.classes
{
    public class FeaturedSessionRequest : RequestFilter
    {
        public Guid? SessionId { get; set; }

        public decimal? Ordinal { get; set; }
    }
}
