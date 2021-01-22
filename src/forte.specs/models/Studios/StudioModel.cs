using System;
using System.Collections.Generic;
using System.Linq;
using forte.models.classes;
using Forte.Svc.Services.Models.PressPublications;

namespace forte.models.studios
{
    public class StudioModel
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
        ///     Studio name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Studio categories
        /// </summary>
        public ClassCategoryModel[] Categories { get; set; }

        /// <summary>
        ///     Descrition of Studio
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        ///     Image of studio
        /// </summary>
        public string ImageUrl { get; set; }

        /// <summary>
        ///     Contact for Studio
        /// </summary>
        public string Contact { get; set; }

        /// <summary>
        ///     When the studio is open
        /// </summary>
        public string Hours { get; set; }

        /// <summary>
        ///     Class header image url
        /// </summary>
        public string HeaderImageUrl { get; set; }

        /// <summary>
        ///     Class thumbnail image url
        /// </summary>
        public string ThumbImageUrl { get; set; }

        /// <summary>
        /// Status of class
        /// </summary>
        public StudioStatus Status { get; set; }

        /// <summary>
        /// The rejection reason.
        /// </summary>
        public string RejectReason { get; set; }

        /// <summary>
        /// Time when it whas published
        /// </summary>
        public DateTime? PublishedOn { get; set; }

        /// <summary>
        /// Time when it was edited
        /// </summary>
        public DateTime? EditedOn { get; set; }

        /// <summary>
        ///  Street address
        /// </summary>
        public string StreetAddress { get; set; }

        /// <summary>
        ///  City
        /// </summary>
        public string City { get; set; }

        /// <summary>
        ///  Postal code
        /// </summary>
        public string PostalCode { get; set; }

        /// <summary>
        ///  Main web site of studio
        /// </summary>
        public string WebSite { get; set; }

        /// <summary>
        /// Facebook profile link
        /// </summary>
        public string Facebook { get; set; }

        /// <summary>
        /// Google+ profile link
        /// </summary>
        public string GooglePlus { get; set; }

        /// <summary>
        /// Twitter profile link
        /// </summary>
        public string Twitter { get; set; }

        /// <summary>
        /// LinkedIn profile link
        /// </summary>
        public string LinkedIn { get; set; }

        /// <summary>
        /// Instagram profile link
        /// </summary>
        public string Instagram { get; set; }

        /// <summary>
        /// Snapchat profile link
        /// </summary>
        public string Snapchat { get; set; }

        /// <summary>
        /// Pinterest profile link
        /// </summary>
        public string Pinterest { get; set; }

        /// <summary>
        ///  Phone number
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        ///  Main studio email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        ///  State
        /// </summary>
        public string State { get; set; }

        /// <summary>
        ///  Country
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// Studio location
        /// </summary>
        public string Location
        {
            get
            {
                var locationParts = new List<string> { StreetAddress, City, State, PostalCode };
                return string.Join(", ", locationParts.Where(part => !string.IsNullOrWhiteSpace(part)));
            }
        }

        /// <summary>
        ///     The studio permalink
        /// </summary>
        public string Permalink { get; set; }

        /// <summary>
        /// The studio press links in JSON formatted string
        /// </summary>
        public List<PressLinkModel> PressLinks { get; set; }

        /// <summary>
        /// Timezone name in tz format
        /// </summary>
        public string TimezoneName { get; set; }

        /// <summary>
        /// Studio next Live Session start time in UTC
        /// </summary>
        public DateTime? NextLiveStartTime { get; set; }

        /// <summary>
        /// Studio next Live Session end time in UTC
        /// </summary>
        public DateTime? NextLiveEndTime { get; set; }

        public int OnDemandClassCount { get; set; }
    }
}
