using System;
using System.ComponentModel.DataAnnotations;
using forte.models.studios;
using forte.models.trainers;

namespace forte.models.classes
{
    public class ClassModel
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
        /// Administrator of this class
        /// </summary>
        public string AdminUserId { get; set; }

        /// <summary>
        ///     The stored record version, used for optimistic concurrency
        /// </summary>
        public byte[] Version { get; set; }

        /// <summary>
        ///     Class name/title
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Class categories
        /// </summary>
        public ClassCategoryModel[] Categories { get; set; }

        /// <summary>
        ///     Event rich-text description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        ///     Class instructions (e.g. bring your own mat)
        /// </summary>
        public string Instructions { get; set; }

        public StudioModel Studio { get; set; }

        /// <summary>
        /// Class header image url
        /// </summary>
        public string HeaderImageUrl { get; set; }

        /// <summary>
        /// Class thumbnail image url
        /// </summary>
        public string ThumbImageUrl { get; set; }

        /// <summary>
        /// Session cover image url
        /// </summary>
        public string CoverImageUrl { get; set; }

        /// <summary>
        /// Session cover video url
        /// </summary>
        public string CoverVideoUrl { get; set; }

        /// <summary>
        /// Status of class
        /// </summary>
        [Required]
        [Display(Name = "Class Status")]
        public ClassStatus Status { get; set; }

        /// <summary>
        /// Time when it was published
        /// </summary>
        public DateTime? PublishedOn { get; set; }

        /// <summary>
        /// Time when it was edited
        /// </summary>
        public DateTime? EditedOn { get; set; }

        /// <summary>
        /// Equipment needed for this class
        /// </summary>
        public ClassEquipmentModel[] Equipment { get; set; }

        /// <summary>
        ///     Created by userId
        /// </summary>
        public string CreatedByUserId { get; set; }

        /// <summary>
        /// Class trainers
        /// </summary>
        public TrainerModel[] Trainers { get; set; }

        /// <summary>
        ///     Class radio stations
        /// </summary>
        public ClassRadioStationModel[] ClassRadioStations { get; set; }

        /// <summary>
        /// Gets or sets a value indicating this class difficulty.
        /// </summary>
        public int? Difficulty { get; set; }

        /// <summary>
        ///     The class permalink
        /// </summary>
        public string Permalink { get; set; }

        /// <summary>
        /// Class Type
        /// </summary>
        public ClassTypeModel ClassType { get; set; }
    }
}
