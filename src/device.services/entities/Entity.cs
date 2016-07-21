using System;

namespace forte.devices.entities
{
    public abstract class Entity
    {
        /// <summary>
        ///     Entity record identifier
        /// </summary>
        public virtual Guid Id { get; set; }

        /// <summary>
        ///     UTC date and time when this entity record was created
        /// </summary>
        public DateTime Created { get; set; }

        /// <summary>
        ///     UTC date and time when this entity record was last modified
        /// </summary>
        public DateTime LastModified { get; set; }

        /// <summary>
        ///     The stored record version, used for optimistic concurrency
        /// </summary>
        public byte[] Version { get; set; }
    }
}