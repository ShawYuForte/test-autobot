using System.Collections.Generic;
using forte.models.classes;

namespace forte.models.accounts
{
    public class EditUserInfoRequestModel
    {
        /// <summary>
        /// The user being edited.
        /// </summary>
        public UserExModel User { get; set; }

        /// <summary>
        ///     Class Types that where added to user.
        /// </summary>
        public List<ClassTypeModel> AddedClassTypes { get; set; }

        /// <summary>
        ///     Class Types that where removed from user.
        /// </summary>
        public List<ClassTypeModel> RemovedClassTypes { get; set; }
    }
}
