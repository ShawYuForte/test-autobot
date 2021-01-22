using forte.models;

namespace forte.services
{
    public interface IAuditService
    {
        /// <summary>
        ///     Creates audit record
        /// </summary>
        /// <param name="action">action performed by user</param>
        /// <param name="userId">user that performed the action</param>
        void CreateAuditRecord(AuditAction action, string userId);

        /// <summary>
        ///     Creates user site visit audit record only if it is the first visit of the day
        /// </summary>
        /// <param name="userId">user that performed the action</param>
        void AuditUserVisit(string userId);
    }
}
