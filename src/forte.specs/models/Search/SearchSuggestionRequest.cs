namespace forte.models.search
{
    /// <summary>
    /// The global search suggestions request filters
    /// </summary>
    public class SearchSuggestionRequest : SearchRequest
    {
        /// <summary>
        /// Initializes a new instance of GlobalSuggestionRequest class
        /// with Take=5
        /// </summary>
        public SearchSuggestionRequest()
        {
            Take = 5;
        }
    }
}
