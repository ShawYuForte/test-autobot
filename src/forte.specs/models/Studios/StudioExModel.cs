using System;

namespace forte.models.studios
{
    public class StudioExModel : StudioModel
    {
        /// <summary>
        ///     Admin user id that created the studio
        /// </summary>
        public string CreatedByUserId { get; set; }

        /// <summary>
        ///     Streaming device identifier
        /// </summary>
        public Guid? StreamingDeviceId { get; set; }
    }
}
