using System.Collections.Generic;

namespace forte.models
{
    /// <summary>
    /// Represents a paged collection result of the <see cref="T"/> type.
    /// </summary>
    /// <typeparam name="T">Type of collection records.</typeparam>
    public class ResultPage<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ResultPage{T}"/> class with default values.
        /// </summary>
        public ResultPage()
        {
            Items = new List<T>();
            TotalCount = 0;
            Skip = 0;
            Take = int.MaxValue;
        }

        /// <summary>
        /// Returned items
        /// </summary>
        public List<T> Items { get; set; }

        /// <summary>
        /// Total non-paginated count of results
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// Items skipped to generate this result
        /// </summary>
        public int Skip { get; set; }

        /// <summary>
        /// Items taken after the skip to generate this result
        /// </summary>
        public int Take { get; set; }
    }
}
