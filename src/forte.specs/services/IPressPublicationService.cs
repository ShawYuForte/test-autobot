using Forte.Svc.Services.Models.PressPublications;

namespace forte.services
{
    /// <summary>
    /// Describes the service to access the Forte app press publications.
    /// </summary>
    public interface IPressPublicationService
    {
        /// <summary>
        /// Retrieve the list of Forte app press publications.
        /// </summary>
        /// <returns>The paged list of press publications.</returns>
        PressPublicationPageModel GetPublications();
    }
}
