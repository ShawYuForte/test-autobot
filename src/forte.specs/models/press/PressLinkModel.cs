using System;

namespace Forte.Svc.Services.Models.PressPublications
{
    /// <summary>
    /// Represents a Press Link to use on site
    /// </summary>
    public class PressLinkModel
    {
        /// <summary>
        /// Press publication identifier
        /// </summary>
        public Guid PublicationId { get; set; }

        /// <summary>
        /// CSS class to use for styling
        /// </summary>
        public string CssClass { get; set; }

        /// <summary>
        /// Url to link to
        /// </summary>
        public string Url { get; set; }
    }
}
