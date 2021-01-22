using System;

namespace forte.models.files
{
    public class FileRecordModel
    {
        /// <summary>
        ///     Entity record identifier
        /// </summary>
        public virtual Guid Id { get; set; }

        /// <summary>
        ///     UTC date and time when this entity record was created
        /// </summary>
        public DateTime Created { get; set; }

        /// <summary>
        ///     Id of user that created this record
        /// </summary>
        public string CreatedByUserId { get; set; }

        /// <summary>
        ///     Full name of the user that created this record
        /// </summary>
        public string CreatedByUserFullName { get; set; }

        /// <summary>
        ///     The stored record version, used for optimistic concurrency
        /// </summary>
        public byte[] Version { get; set; }

        /// <summary>
        ///     Original file name
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        ///     Original file size
        /// </summary>
        public long FileSize { get; set; }

        /// <summary>
        ///     Blob name where the file was stored
        /// </summary>
        public string BlobName { get; set; }

        /// <summary>
        ///     Blob container where the file was stored
        /// </summary>
        public string BlobContainer { get; set; }

        /// <summary>
        ///     Blob file url
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        ///     Date and time last modified
        /// </summary>
        public DateTime LastModified { get; set; }

        /// <summary>
        ///     Id of user that last modified this file
        /// </summary>
        public string LastModifiedByUserId { get; set; }

        /// <summary>
        ///     Full name of the user that last modified the file
        /// </summary>
        public string LastModifiedByUserFullName { get; set; }

        /// <summary>
        ///     File expiration date. When set, the file will be deleted after the expiration date
        /// </summary>
        public DateTime? ExpiresOn { get; set; }

        /// <summary>
        ///     A copy of the file locally cached (if present)
        /// </summary>
        public string LocalCachedFilePath { get; set; }
    }
}
