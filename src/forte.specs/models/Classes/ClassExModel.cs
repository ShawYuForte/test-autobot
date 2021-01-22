using System;
using forte.models.studios;

namespace forte.models.classes
{
    public class ClassExModel : ClassModel
    {
        /// <summary>
        ///     Class rating
        /// </summary>
        public decimal? Rating { get; set; }

        /// <summary>
        ///     Streaming device identifier
        /// </summary>
        public Guid? StreamingDeviceId { get; set; }

        public new StudioExModel Studio { get; set; }
    }
}
