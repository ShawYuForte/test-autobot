namespace forte.models.accounts
{
    public class InvitationFilter : RequestFilter
    {
        /// <summary>
        ///     Filter by registration code
        /// </summary>
        public string RegistrationCode { get; set; }

        /// <summary>
        ///     Filter by email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        ///     Request that service return all invitations, not just ones sent by current user
        /// </summary>
        public bool ReturnAll { get; set; }
    }
}
