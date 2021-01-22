namespace forte.models.accounts
{
    public class UserRequest : RequestFilter
    {
        public string RegistrationCode { get; set; }

        public string Name { get; set; }

        public string Id { get; set; }

        public string GroupName { get; set; }
    }
}
