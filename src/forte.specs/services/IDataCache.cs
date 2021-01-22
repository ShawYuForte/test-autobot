using System;
using System.Threading.Tasks;

namespace forte.services
{
    public interface IDataCache
    {
        /// <summary>
        ///     Clear cached data
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task ClearAsync(string key);

        /// <summary>
        ///     Get cached data value
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<T> GetAsync<T>(string key);

        /// <summary>
        ///     Cache data value
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        Task SetAsync<T>(string key, T data);

        /// <summary>
        ///     Cache data value with expiration
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="data"></param>
        /// <param name="expiresAfter"></param>
        /// <returns></returns>
        Task SetAsync<T>(string key, T data, TimeSpan expiresAfter);
    }
}
