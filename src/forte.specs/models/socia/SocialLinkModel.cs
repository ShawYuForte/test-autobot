using System;

namespace Forte.Svc.Services.Models.SocialLinks
{
    /// <summary>
    /// Represents a Social Media Network link to use on site
    /// </summary>
    public class SocialLinkModel
    {
        /// <summary>
        ///     Entity record identifier
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        ///     UTC date and time when this entity record was created
        /// </summary>
        public DateTime Created { get; set; }

        /// <summary>
        ///     The stored record version, used for optimistic concurrency
        /// </summary>
        public byte[] Version { get; set; }

        /// <summary>
        /// Social Platform name, e.g., Facebook
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Path for icon image to use
        /// </summary>
        public string IconPath { get; set; }

        /// <summary>
        /// CSS class to use for styling
        /// </summary>
        public string CssClass { get; set; }

        /// <summary>
        /// URL to to the site (e.g. personalized Facebook url)
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Order value for sorting links (lower should appear first)
        /// </summary>
        public int? Order { get; set; }
    }
}
