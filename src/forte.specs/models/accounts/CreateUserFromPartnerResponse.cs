namespace forte.models.accounts
{
    public class CreateUserFromPartnerResponse
    {
        public CreateUserFromPartnerResponse()
        {
        }

        public CreateUserFromPartnerResponse(string userId)
        {
            UserId = userId;
        }

        public string UserId { get; set; }
    }
}
