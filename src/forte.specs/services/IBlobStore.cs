using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Forte.Svc.Services.Models.Blobs;

namespace forte.services
{
    public interface IBlobStore
    {
        /// <summary>
        ///     Upload file to blob store
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="container"></param>
        /// <returns></returns>
        Task<Blob> UploadFileAsync(string filePath, string container, string contentType = "text/plain");

        /// <summary>
        ///     Fetch blobs in specified container
        /// </summary>
        /// <param name="container"></param>
        /// <returns></returns>
        Task<List<Blob>> FetchBlobsAsync(string container, string prefix = "");

        /// <summary>
        /// Set Blob metadata value
        /// </summary>
        /// <param name="container"></param>
        /// <param name="blobName"></param>
        /// <param name="metadata"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        Task<Blob> SetBlobMetadataValueAsync(string container, string blobName, string metadata, string value);

        /// <summary>
        /// Download blob
        /// </summary>
        /// <param name="container"></param>
        /// <param name="fileBlobName"></param>
        /// <returns></returns>
        Task<string> DownloadBlobAsync(string container, string fileBlobName);

        /// <summary>
        /// Downloads the string contents from a given url
        /// </summary>
        /// <param name="url">URL from where to retrive the contents</param>
        /// <returns>String contents retrived</returns>
        Task<string> DownloadStringAsync(string url);

        /// <summary>
        /// Gets blob details from provided blob url
        /// </summary>
        /// <param name="blobUrl">Blob Url to retrieve details for</param>
        /// <returns>Blob details</returns>
        Task<Blob> GetBlobDetailsAsync(string blobUrl);

        /// <summary>
        /// Gets blob contents as a readable stream
        /// </summary>
        /// <param name="blobUrl">Blob Url to retrieve</param>
        /// <returns>Stream with blob contents</returns>
        Task<Stream> GetBlobStreamAsync(string blobUrl);

        /// <summary>
        /// Uploads stream contents to a blob. Creates or replaces specified blob if it already exists.
        /// </summary>
        /// <param name="container">Blob container name</param>
        /// <param name="blobName">Blob name to create</param>
        /// <param name="stream">Stream with contents to upload to blob</param>
        /// <param name="contentType">Mime Content Type to associate with created blob</param>
        /// <returns>Blob details</returns>
        Task<Blob> UploadStreamToBlobAsync(string container, string blobName, Stream stream, string contentType);

        /// <summary>
        /// Deletes the specified blobs
        /// </summary>
        /// <param name="blobUrls">List of blob urls to delete</param>
        /// <returns></returns>
        Task DeleteBlobAsync(params string[] blobUrls);
    }
}
