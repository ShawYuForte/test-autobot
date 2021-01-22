using System;
using System.Threading.Tasks;
using forte.models.files;

namespace forte.services
{
    public interface IFileService
    {
        /// <summary>
        ///     Upload a file from specified location, and delete local copy. The file is uploaded as unclaimed with an expiration
        ///     date after 24 hours
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="container"></param>
        /// <param name="deleteLocal">Specify whether to delete the local copy</param>
        /// <returns></returns>
        Task<FileRecordModel> UploadAsync(string filePath, string container, bool deleteLocal);

        /// <summary>
        ///     Upload a file from specified location, and delete local copy. The file is uploaded as unclaimed with an expiration
        ///     date after 24 hours. The upload request has support for cropping
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<FileRecordModel> UploadAsync(FileUploadRequest request);

        /// <summary>
        ///     Claims an uploaded file, the expiration time is removed on the file
        /// </summary>
        /// <param name="fileRecordId"></param>
        /// <returns></returns>
        Task<FileRecordModel> ClaimFileAsync(Guid fileRecordId);

        /// <summary>
        ///     Get an existing file record
        /// </summary>
        /// <param name="fileRecordId"></param>
        /// <exception cref="RecordNotFoundException"></exception>
        /// <returns></returns>
        Task<FileRecordModel> GetFileRecordAsync(Guid fileRecordId);

        /// <summary>
        /// Download file based on record locally
        /// </summary>
        /// <param name="fileRecordModel"></param>
        /// <param name="overwriteCache">If true, will download the file even if present locally</param>
        /// <returns></returns>
        Task<string> DownloadLocallyAsync(FileRecordModel fileRecordModel, bool overwriteCache);
    }
}
