using System;

namespace forte.models.accounts
{
    /// <summary>
    /// This class represents account utilization
    /// </summary>
    public class AccountUtilization
    {
        public DateTime? LastLoginDate { get; set; }

        public int VideosWatched { get; set; }
    }
}
