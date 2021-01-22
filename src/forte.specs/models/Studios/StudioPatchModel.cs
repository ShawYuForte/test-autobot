using System;
using forte.models;

namespace Forte.Svc.Services.Models.Studios
{
    public class StudioPatchRequestModel
    {
        /// <summary>
        /// Entity record identifier
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// The stored record version, used for optimistic concurrency
        /// </summary>
        public byte[] Version { get; set; }

        /// <summary>
        /// Patched Status of studio
        /// </summary>
        public StudioStatus? Status { get; set; }
    }
}
