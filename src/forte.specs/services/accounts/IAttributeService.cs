using System.Collections.Generic;
using Forte.Svc.Services.Models.Accounts;

namespace forte.services
{
    /// <summary>
    /// Represents the dynamic attributes service
    /// </summary>
    public interface IAttributeService
    {
        /// <summary>
        /// Gets the dynamic field values bound to user personal info.
        /// </summary>
        /// <param name="userId">The user ID.</param>
        /// <returns>The list of dynamic attributes for the user.</returns>
        List<AttributeModel> GetUserAttributes(string userId);

        /// <summary>
        /// Updates the dynamic field values bound to user personal info.
        /// </summary>
        /// <param name="userId">The user ID.</param>
        /// <param name="attributes">The attributes to update.</param>
        /// <returns>The list of dynamic attributes for the user.</returns>
        List<AttributeModel> UpdateUserAttributes(string userId, List<AttributeModel> attributes);
    }
}
