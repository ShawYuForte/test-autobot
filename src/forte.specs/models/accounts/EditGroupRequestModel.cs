using System.Collections.Generic;
using Forte.Svc.Services.Models.Accounts;

namespace forte.models.accounts
{
    public class EditGroupRequestModel
    {
        /// <summary>
        /// The group of claims being edited.
        /// </summary>
        public GroupExModel Group { get; set; }

        /// <summary>
        /// The list of added claim IDs.
        /// </summary>
        public List<ClaimDefinitionModel> AddedClaims { get; set; }

        /// <summary>
        /// The list of removed claim IDs.
        /// </summary>
        public List<ClaimDefinitionModel> RemovedClaims { get; set; }
    }
}
