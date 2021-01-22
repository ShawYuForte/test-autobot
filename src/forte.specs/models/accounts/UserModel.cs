using System;

namespace forte.models.accounts
{
    /// <summary>
    /// The base user model.
    /// </summary>
    public class UserModel
    {
        public UserModel()
        {
            Created = DateTime.UtcNow;
        }

        public string Id { get; set; }

        public string Email { get; set; }

        public string UserName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string HeaderImageUrl { get; set; }

        public string ProfileImageUrl { get; set; }

        public bool? Suspended { get; set; }

        public DateTime Created { get; set; }

        public Guid? StudioId { get; set; }

        public bool CanManageStudioContent { get; set; }

        public bool IsAdmin { get; set; }

        public string LinkedTrainerId { get; set; }
    }
}
