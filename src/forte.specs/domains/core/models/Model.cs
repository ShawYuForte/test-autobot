using System;

namespace forte.domains.core.models
{
    public class Model
    {
        /// <summary>
        ///     UTC date and time when this entity record was created
        /// </summary>
        public DateTime Created { get; set; }

        /// <summary>
        ///     Model global identifier
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        ///     The stored record version, used for optimistic concurrency
        /// </summary>
        public byte[] Version { get; set; }
    }
}
