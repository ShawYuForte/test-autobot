using Forte.Svc.Services.Models.SocialLinks;

namespace forte.services
{
    /// <summary>
    /// Describes the service to access the Forte app social links.
    /// </summary>
    public interface ISocialLinkService
    {
        /// <summary>
        /// Retrieve the list of Forte app social links.
        /// </summary>
        /// <returns>The paged list of social links.</returns>
        SocialLinkPageModel GetLinks();
    }
}
