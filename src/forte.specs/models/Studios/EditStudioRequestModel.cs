using System;
using System.Collections.Generic;
using forte.models.studios;
using forte.models.trainers;

namespace Forte.Svc.Services.Models.Studios
{
    public class EditStudioRequestModel
    {
        /// <summary>
        ///     Class being edited
        /// </summary>
        public StudioExModel Studio { get; set; }

        /// <summary>
        ///     Added class trainers
        /// </summary>
        public List<TrainerExModel> AddedTrainers { get; set; }

        /// <summary>
        ///     Removed class trainers
        /// </summary>
        public List<TrainerExModel> RemovedTrainers { get; set; }

        /// <summary>
        /// The list of added category IDs.
        /// </summary>
        public List<Guid> AddedCategories { get; set; }

        /// <summary>
        /// The list of removed category IDs.
        /// </summary>
        public List<Guid> RemovedCategories { get; set; }
    }
}
