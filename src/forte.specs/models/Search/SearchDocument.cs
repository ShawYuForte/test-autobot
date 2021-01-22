using System;
using System.Text.Json.Serialization;
using forte.converters;

namespace forte.models.search
{
    /// <summary>
    ///     The global search document details to index
    /// </summary>
    public class SearchDocument
    {
        /// <summary>
        ///     The class category IDs for class session type documents.
        /// </summary>
        public string[] CategoryIds { get; set; }

        /// <summary>
        ///     The class category names for class session type documents.
        /// </summary>
        public string[] CategoryNames { get; set; }

        /// <summary>
        ///     The city for studio/class session type documents.
        /// </summary>
        public string City { get; set; }

        /// <summary>
        ///     The session class ID.
        /// </summary>
        public string ClassId { get; set; }

        /// <summary>
        ///     The session class Permalink.
        /// </summary>
        public string ClassPermalink { get; set; }

        /// <summary>
        ///     The class type id for class session type documents.
        /// </summary>
        public string ClassTypeId { get; set; }

        /// <summary>
        ///     The class type name for class session type documents.
        /// </summary>
        public string ClassTypeName { get; set; }

        /// <summary>
        ///     The search document description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        ///     The class difficulty for class session type documents.
        /// </summary>
        [JsonConverter(typeof(IntToStringConverter))]
        public int? Difficulty { get; set; }

        /// <summary>
        ///     The class session duration for class session type documents.
        /// </summary>
        [JsonConverter(typeof(IntToStringConverter))]
        public int? Duration { get; set; }

        /// <summary>
        ///     The class session end time in UTC for class session type documents.
        /// </summary>
        public DateTime? EndTime { get; set; }

        /// <summary>
        ///     The class equipment Ids for class session type documents.
        /// </summary>
        public string[] EquipmentIds { get; set; }

        /// <summary>
        ///     The class equipment for class session type documents.
        /// </summary>
        public string[] EquipmentNames { get; set; }

        /// <summary>
        ///     The search document entity ID.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        ///     The search document image URL.
        /// </summary>
        public string ImageUrl { get; set; }

        /// <summary>
        ///     Does the class need equipment, for class session type documents.
        /// </summary>
        public bool? NeedsEquipment { get; set; }

        /// <summary>
        ///     The search document entity Permalink.
        /// </summary>
        public string Permalink { get; set; }

        /// <summary>
        ///     The class session start time in UTC for class session type documents.
        /// </summary>
        public DateTime? StartTime { get; set; }

        /// <summary>
        ///     The class session status for class session type documents.
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        ///     The studio hours for studio type documents.
        /// </summary>
        public string StudioHours { get; set; }

        /// <summary>
        ///     The class session studio ID.
        /// </summary>
        public string StudioId { get; set; }

        /// <summary>
        ///     The class session studio name.
        /// </summary>
        public string StudioName { get; set; }

        /// <summary>
        ///     The class session studio Permalink.
        /// </summary>
        public string StudioPermalink { get; set; }

        /// <summary>
        ///     The search document title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        ///     The class session trainers ID.
        /// </summary>
        public string TrainerId { get; set; }

        /// <summary>
        ///     The class session trainer name.
        /// </summary>
        public string TrainerName { get; set; }

        /// <summary>
        ///     The class session trainers Permalink.
        /// </summary>
        public string TrainerPermalink { get; set; }

        /// <summary>
        ///     The trainer specialities for trainer type documents.
        /// </summary>
        public string TrainerSpecialities { get; set; }

        /// <summary>
        /// Ids of users which added session to favorite
        /// </summary>
        public string[] FavoriteUserIds { get; set; }

        public bool HasVideoUrl { get; set; }

        public string VideoUrl { get; set; }
    }
}
