using System.Collections.Generic;
using forte.models.studios;

namespace forte.models.trainers
{
    public class EditTrainerRequestModel
    {
        /// <summary>
        /// The trainer being edited
        /// </summary>
        public TrainerExModel Trainer { get; set; }

        /// <summary>
        /// The list of added studios.
        /// </summary>
        public List<StudioModel> AddedStudios { get; set; }

        /// <summary>
        /// The list of removed studios.
        /// </summary>
        public List<StudioModel> RemovedStudios { get; set; }
    }
}
