namespace forte.devices.models
{
    public class Input
    {
        /// <summary>
        /// Input title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Input fiile name
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Input file url (where file can be downloaded from)
        /// </summary>
        public string FileUrl { get; set; }

        /// <summary>
        /// Current file hash
        /// </summary>
        public string FileHash { get; set; }
    }
}