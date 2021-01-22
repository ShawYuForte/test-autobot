namespace forte.models.accounts
{
    public class InvitationPageModel : ResultPage<InvitationModel>
    {
        /// <summary>
        /// How many invitations user has remaining
        /// </summary>
        public int InvitationsRemaining { get; set; }
    }
}
