using forte.models.trainers;

namespace forte.models.classes
{
    public class PublicClassTrainerModel
    {
        /// <summary>
        /// Gets or sets the class.
        /// </summary>
        /// <value>The class.</value>
        public ClassModel Class { get; set; }

        /// <summary>
        /// Gets or sets the trainer.
        /// </summary>
        /// <value>The trainer.</value>
        public TrainerModel Trainer { get; set; }
    }
}
