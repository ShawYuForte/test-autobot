using System;

namespace forte.models.accounts
{
    public class BadgeActionModel
    {
        public string Action { get; set; }

        public string UserId { get; set; }

        public DateTime Created { get; set; }

        public DateTime Date { get; set; }
    }
}
