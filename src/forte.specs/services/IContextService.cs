namespace forte.services
{
    /// <summary>
    ///     The context service provides information on the current context. Who is signed in, if anyone,
    ///     what permissions they have, etc.
    /// </summary>
    public interface IContextService
    {
        /// <summary>
        ///     Determines whether the current user has a specific permission.
        /// </summary>
        /// <param name="claim">The <see cref="WellKnownClaims" /> to check.</param>
        /// <returns>The result if the user has a specified claim.</returns>
        bool AssertContextClaim(string claim);

        /// <summary>
        ///     Retrieve the current context user id.  If the context is not authenticated, the method returns null
        /// </summary>
        /// <returns></returns>
        string GetCurrentUserId();

        /// <summary>
        ///     Retrieve the current context username.  If the context is not authenticated, the method returns null
        /// </summary>
        /// <returns></returns>
        string GetCurrentUsername();

        /// <summary>
        ///     Determine whether the current context is authenticated
        /// </summary>
        /// <returns></returns>
        bool IsContextAuthenticated();

        /// <summary>
        ///     Determine whether the current context user is the system user (elevated privileges)
        /// </summary>
        /// <returns></returns>
        bool IsCurrentSystemUser();
    }
}
