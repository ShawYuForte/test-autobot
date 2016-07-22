namespace forte.devices.models
{
    public class VmixInput
    {
        public string Key { get; set; }
        public string Number { get; set; }
        public string Type { get; set; }
        public string Title { get; set; }
        public string State { get; set; }

        /// <summary>
        ///     Duration (if video or audio) in milliseconds
        /// </summary>
        public int Duration { get; set; }

        public InputRole Role { get; set; }
    }
}