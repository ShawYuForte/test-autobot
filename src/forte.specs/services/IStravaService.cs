using System;
using System.Threading.Tasks;
using forte.models;
using forte.models.classes;

namespace forte.services
{
    public interface IStravaService
    {
        /// <summary>
        /// Create strava activity
        /// </summary>
        /// <param name="activity"></param>
        /// <param name="authorizationCode"></param>
        /// <returns></returns>
        Task<StravaActivityDto> CreateActivity(StravaActivity activity, string authorizationCode);

        /// <summary>
        /// Upload image from htttp to strava
        /// </summary>
        /// <param name="uploadUrl"></param>
        /// <param name="imageUrl"></param>
        /// <returns></returns>
        Task UploadImage(string uploadUrl, string imageUrl);

        /// <summary>
        /// Create auth token for strava by code
        /// </summary>
        /// <param name="code">auth code, recieved from strava's popup</param>
        /// <returns>auth token</returns>
        Task<string> CreateAuthToken(string code);

        /// <summary>
        /// Share session to strava
        /// </summary>
        /// <param name="authorizationCode"> not required param if user is not authorized</param>
        /// <returns>Result of sharing, fields will be null if everything gone ok</returns>
        Task<ShareActivityResult> ShareSessionToStrava(Guid sessionId, DateTime date, string authorizationCode = null, string comment = null);

        /// <summary>
        /// Get Strava Athlete
        /// </summary>
        /// <param name="token">auth token</param>
        /// <returns>strava athlete</returns>
        Task<StravaAthlete> GetStravaAthlete(string token);
    }
}
