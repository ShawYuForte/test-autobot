namespace Forte.Svc.Services.Models.Accounts
{
    public class UserPatchRequestModel
    {
        /// <summary>
        /// The stored record version, used for optimistic concurrency
        /// </summary>
        public byte[] Version { get; set; }

        /// <summary>
        /// Patched Status of user
        /// </summary>
        public bool? IsSuspended { get; set; }
    }
}
