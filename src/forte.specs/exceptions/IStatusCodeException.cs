using System.Net;

namespace forte.exceptions
{
    /// <summary>
    /// The interface for the exceptions providing the HTTP status code details.
    /// </summary>
    public interface IStatusCodeException
    {
        HttpStatusCode HttpStatusCode { get; set; }
    }
}
