using System;

namespace forte.models.classes
{
    public class ClassTypeModel
    {
        /// <summary>
        /// Entity record identifier
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Class type name
        /// </summary>
        public string Name { get; set; }
    }

    public class ClassTypeExModel : ClassTypeModel
    {
        /// <summary>
        ///     UTC date and time when this class type was created
        /// </summary>
        public DateTime Created { get; set; }

        /// <summary>
        ///     The stored record version, used for optimistic concurrency
        /// </summary>
        public byte[] Version { get; set; }
    }
}
