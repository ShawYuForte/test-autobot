namespace forte.services
{
    /// <summary>
    /// Interface for providing configuration settings needed by the application
    /// </summary>
    public interface IConfigProvider
    {
        /// <summary>
        /// Content root url for media assets
        /// </summary>
        /// <returns></returns>
        string GetContentRootPath();
    }
}
