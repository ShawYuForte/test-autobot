using System;
using System.Collections.Generic;
using Forte.Svc.Services.Models.Accounts;

namespace forte.models.accounts
{
    /// <summary>
    /// The extended user group of claims model.
    /// </summary>
    public class GroupExModel : GroupModel
    {
        public DateTime? Created { get; set; }

        /// <summary>
        /// Gets or sets the group claims definitions.
        /// </summary>
        public List<ClaimDefinitionModel> Claims { get; set; }

        /// <summary>
        /// Gets or sets the users assigned to the group.
        /// </summary>
        public List<UserModel> Users { get; set; }

        /// <summary>
        ///     The stored record version, used for optimistic concurrency
        /// </summary>
        public byte[] Version { get; set; }
    }
}
