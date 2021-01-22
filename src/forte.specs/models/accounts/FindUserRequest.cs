namespace forte.models.accounts
{
    public class FindUserRequest : RequestFilter
    {
        /// <summary>
        /// When specified, user with specified email is returned
        /// </summary>
        public string Email { get; set; }
    }
}
