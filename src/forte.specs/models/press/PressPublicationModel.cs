using System;

namespace Forte.Svc.Services.Models.PressPublications
{
    /// <summary>
    /// Represents a Press Publication to use on site
    /// </summary>
    public class PressPublicationModel
    {
        /// <summary>
        ///     Entity record identifier
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Press Publication name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// CSS class to use for styling
        /// </summary>
        public string CssClass { get; set; }
    }
}
