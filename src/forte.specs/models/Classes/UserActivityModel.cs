using System;
using System.Collections.Generic;

namespace forte.models.classes
{
    public class UserActivityModel : CreateUserActivityModel
    {
        public UserActivityModel()
        {
            UserActivityLikes = new List<UserActivityLikeModel>();
            UserActivityComments = new List<UserActivityCommentModel>();
        }

        public Guid Id { get; set; }

        public string UserName { get; set; }

        public int UserActivityTypeId
        {
            get
            {
                return (int)UserActivityType;
            }
        }

        public string UserImgUrl { get; set; }

        public string ClassName { get; set; }

        public string ClassImgUrl { get; set; }

        public string BadgeName { get; set; }

        public string BadgeImgUrl { get; set; }

        public DateTime Created { get; set; }

        public bool WasRead { get; set; }

        public IEnumerable<UserActivityLikeModel> UserActivityLikes { get; set; }

        public IEnumerable<UserActivityCommentModel> UserActivityComments { get; set; }
    }
}
