namespace forte.models.files
{
    public class FileUploadRequest
    {
        public string Container { get; set; }

        public int? CropHeight { get; set; }

        public int? CropWidth { get; set; }

        public int? CropX { get; set; }

        public int? CropY { get; set; }

        /// <summary>
        ///     Specify whether to delete the local copy
        /// </summary>
        public bool DeleteLocal { get; set; }

        public string LocalFilePath { get; set; }
    }
}
