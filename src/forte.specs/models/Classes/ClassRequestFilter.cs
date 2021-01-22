namespace forte.models.classes
{
    public class ClassRequestFilter : RequestFilter
    {
        /// <summary>
        ///     Return deleted classes (deleted only)
        /// </summary>
        public bool? Deleted { get; set; }

        /// <summary>
        ///     Field for sorting
        /// </summary>
        public string OrderBy { get; set; }

        /// <summary>
        ///     Order direction (asc, desc)
        /// </summary>
        public string OrderDir { get; set; }

        /// <summary>
        /// For studio content manager
        /// </summary>
        public bool ForStudioContentManager { get; set; }

        public string Status { get; set; }

        public string Name { get; set; }

        public string StudioName { get; set; }
    }
}
