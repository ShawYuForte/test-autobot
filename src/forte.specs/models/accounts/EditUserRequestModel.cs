using System.Collections.Generic;

namespace forte.models.accounts
{
    public class EditUserRequestModel
    {
        /// <summary>
        /// The user being edited.
        /// </summary>
        public UserExModel User { get; set; }

        /// <summary>
        /// The list of added claim group IDs.
        /// </summary>
        public List<GroupExModel> AddedGroups { get; set; }

        /// <summary>
        /// The list of removed claim group IDs.
        /// </summary>
        public List<GroupExModel> RemovedGroups { get; set; }
    }
}
