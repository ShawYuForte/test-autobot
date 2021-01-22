using System;

namespace forte.models.classes
{
    public class ClassEquipmentModel
    {
        /// <summary>
        /// Entity record identifier
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Class equipment name
        /// </summary>
        public string Name { get; set; }
    }

    public class ClassEquipmentExModel : ClassEquipmentModel
    {
        /// <summary>
        ///     UTC date and time when this class equipment was created
        /// </summary>
        public DateTime Created { get; set; }

        /// <summary>
        ///     The stored record version, used for optimistic concurrency
        /// </summary>
        public byte[] Version { get; set; }
    }
}
