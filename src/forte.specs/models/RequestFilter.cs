using Newtonsoft.Json;

namespace forte.models
{
    /// <summary>
    ///     The basic request filter object for retrieving paged lists.
    /// </summary>
    public class RequestFilter
    {
        public RequestFilter()
        {
            Extended = false;
            IncludeInactive = false;
        }

        /// <summary>
        ///     Indicates results should include inactive results
        /// </summary>
        public bool? IncludeInactive { get; set; }

        /// <summary>
        ///     Indicates what data to return - basic (by default) or extended if <c>true</c>.
        /// </summary>
        public bool? Extended { get; set; }

        /// <summary>
        ///     The number of items to skip. 0 by default
        /// </summary>
        public int? Skip { get; set; }

        /// <summary>
        ///     The number of items to take. All by default.
        /// </summary>
        public int? Take { get; set; }

        /// <summary>
        ///     Entity record version, used for optimistic concurrency validation
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        ///     Convert the request version from string to a byte array
        /// </summary>
        /// <returns></returns>
        public byte[] GetVersionBytes()
        {
            // This is kind of a working hack, it's not worth investigating unless there is an issue in the future
            var jsonString = $"{{ \"version\": \"{Version}\" }}";
            var entity = JsonConvert.DeserializeObject<VersionedResponse>(jsonString);
            return entity.Version;
        }

        /// <summary>
        ///     Determines whether the extended results are requested or not.
        /// </summary>
        /// <returns><c>true</c> if extended, otherwise <c>false</c>.</returns>
        public bool IsExtended()
        {
            return Extended.HasValue && Extended.Value;
        }

        private class VersionedResponse
        {
            public byte[] Version { get; set; }
        }
    }
}
