using System;
using System.Collections.Generic;
using Forte.Svc.Services.Models.PressPublications;

namespace forte.models.trainers
{
    /// <summary>
    ///     Represents a public trainer profile
    /// </summary>
    public class TrainerModel
    {
        /// <summary>
        ///     10 things about me
        /// </summary>
        public List<string> AboutMe { get; set; }

        /// <summary>
        ///     Trainer Bio
        /// </summary>
        public string Bio { get; set; }

        /// <summary>
        ///     The trainer birthday
        /// </summary>
        public DateTime? Birthday { get; set; }

        /// <summary>
        ///     Certifications of the trainer
        /// </summary>
        public string Certifications { get; set; }

        /// <summary>
        ///     Date and time last edited on
        /// </summary>
        public DateTime? EditedOn { get; set; }

        /// <summary>
        ///     Facebook profile link
        /// </summary>
        public string Facebook { get; set; }

        /// <summary>
        ///     Favorite quote
        /// </summary>
        public string FavoriteQuote { get; set; }

        /// <summary>
        ///     Feedback rating
        /// </summary>
        public int FeedbackRating { get; set; }

        /// <summary>
        ///     Name of trainer
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        ///     Full name (first name + last name)
        /// </summary>
        public string FullName => $"{FirstName} {LastName}".Trim();

        /// <summary>
        ///     Fun fitness fact
        /// </summary>
        public string FunFitnessFact { get; set; }

        /// <summary>
        ///     Google+ profile link
        /// </summary>
        public string GooglePlus { get; set; }

        /// <summary>
        ///     Entity record identifier
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        ///     profile image of trainer
        /// </summary>
        public string ImageUrl { get; set; }

        /// <summary>
        ///     Instagram profile link
        /// </summary>
        public string Instagram { get; set; }

        /// <summary>
        ///     Interested in
        /// </summary>
        public string InterestedIn { get; set; }

        /// <summary>
        ///     Surname of trainer
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        ///     LinkedIn profile link
        /// </summary>
        public string LinkedIn { get; set; }

        /// <summary>
        ///     Nickname
        /// </summary>
        public string Nickname { get; set; }

        /// <summary>
        ///     One word description
        /// </summary>
        public string OneWordDescription { get; set; }

        /// <summary>
        ///     The trainer permalink
        /// </summary>
        public string Permalink { get; set; }

        /// <summary>
        ///     Pinterest profile link
        /// </summary>
        public string Pinterest { get; set; }

        /// <summary>
        ///     Popularity rating, a system calculated metric
        /// </summary>
        public int PopularityRating { get; set; }

        /// <summary>
        ///     Trainer press links
        /// </summary>
        public List<PressLinkModel> PressLinks { get; set; }

        public string PrimaryEmail { get; set; }

        /// <summary>
        ///     The trainer primary studio.
        /// </summary>
        public TrainerStudioModel PrimaryStudio { get; set; }

        /// <summary>
        ///     profile image of trainer
        /// </summary>
        public string ProfileImageThumbUrl { get; set; }

        /// <summary>
        ///     profile image of trainer
        /// </summary>
        public string ProfileImageUrl { get; set; }

        /// <summary>
        ///     Pro tips
        /// </summary>
        public string ProTips { get; set; }

        /// <summary>
        ///     Date and time published on
        /// </summary>
        public DateTime? PublishedOn { get; set; }

        /// <summary>
        ///     Snapchat profile link
        /// </summary>
        public string Snapchat { get; set; }

        /// <summary>
        ///     Specialities of trainer
        /// </summary>
        public string Specialities { get; set; }

        /// <summary>
        ///     Profile Image of the Trainer
        /// </summary>
        public TrainerStatus Status { get; set; }

        /// <summary>
        ///     Twitter profile link
        /// </summary>
        public string Twitter { get; set; }

        /// <summary>
        ///     Main web site of trainer
        /// </summary>
        public string WebSite { get; set; }
    }
}
