using System;
using System.Collections.Generic;
using forte.models.trainers;

namespace forte.models.classes
{
    public class EditClassRequestModel
    {
        /// <summary>
        ///     The class being edited
        /// </summary>
        public ClassExModel Class { get; set; }

        /// <summary>
        ///     The list of added trainers IDs.
        /// </summary>
        public List<TrainerExModel> AddedTrainers { get; set; }

        /// <summary>
        ///     The list of removed trainers IDs.
        /// </summary>
        public List<TrainerExModel> RemovedTrainers { get; set; }

        /// <summary>
        ///     The list of added category IDs.
        /// </summary>
        public List<Guid> AddedCategories { get; set; }

        /// <summary>
        ///     The list of removed category IDs.
        /// </summary>
        public List<Guid> RemovedCategories { get; set; }

        /// <summary>
        ///     The list of added equipment.
        /// </summary>
        public List<ClassEquipmentModel> AddedEquipment { get; set; }

        /// <summary>
        ///     The list of removed equipment.
        /// </summary>
        public List<ClassEquipmentModel> RemovedEquipment { get; set; }
    }
}
