using System;

namespace forte.models.classes
{
    public class ClassCategoryModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
    }

    public class ClassCategoryExModel : ClassCategoryModel
    {
        /// <summary>
        ///     UTC date and time when this class category was created
        /// </summary>
        public DateTime Created { get; set; }

        /// <summary>
        ///     The stored record version, used for optimistic concurrency
        /// </summary>
        public byte[] Version { get; set; }
    }
}
