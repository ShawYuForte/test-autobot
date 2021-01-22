using System;
using System.ComponentModel.DataAnnotations;

namespace forte.models.classes
{
    public class ClassRadioStationModel
    {
        /// <summary>
        /// Id of entity
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Id of linked class
        /// </summary>
        [Required]
        public Guid ClassId { get; set; }

        /// <summary>
        /// Station Play duration in %
        /// </summary>
        [Required]
        [Range(1, 100)]
        public int PlayDuration { get; set; }

        /// <summary>
        /// Radio station identifier
        /// </summary>
        [Required]
        public string RadioStation { get; set; }

        /// <summary>
        /// Created date
        /// </summary>
        public DateTime Created { get; set; }

        /// <summary>
        /// Version
        /// </summary>
        public byte[] Version { get; set; }
    }
}
