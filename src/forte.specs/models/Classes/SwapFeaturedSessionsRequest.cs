using System;

namespace forte.models.classes
{
    public class SwapFeaturedSessionsRequest
    {
        public Guid FromSessionId { get; set; }

        public byte[] FromSessionVersion { get; set; }

        public Guid ToSessionId { get; set; }

        public byte[] ToSessionVersion { get; set; }
    }
}
