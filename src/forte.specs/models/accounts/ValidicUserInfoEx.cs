namespace forte.models.accounts
{
    public class ValidicUserInfoEx
    {
        public string ValidicUserId { get; set; }

        public string LocalUserId { get; set; }

        public string ValidicToken { get; set; }

        public string ValidicMobileToken { get; set; }

        public string ValidicMarketplaceUrl { get; set; }
    }

    public class ValidicUserInfo
    {
        public string LocalUserId { get; set; }

        public string ValidicMarketplaceUrl { get; set; }
    }
}
