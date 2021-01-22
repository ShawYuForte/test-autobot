using System;

namespace forte.models
{
    public class StravaActivity
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public int ElapsedTime { get; set; } //in seconds

        public DateTime Date { get; set; }
    }
}
