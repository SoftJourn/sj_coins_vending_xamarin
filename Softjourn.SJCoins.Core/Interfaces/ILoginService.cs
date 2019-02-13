namespace Softjourn.SJCoins.Core.Managers.Interfaces
{
	public interface ILoginService
	{
        /// <summary>
        /// Gets a value indicating whether the user is authenticated.
        /// </summary>
        bool IsAuthenticated { get; }

        /// <summary>
        /// The login method to retrieve OAuth2 access tokens from an API.
        /// </summary>
        /// <param name="userName">The user Name (email address)</param>
        /// <param name="password">The users password</param>
        /// <returns>Returns true if the login is successful, false otherwise</returns>
        bool Login(string userName, string password);
	}
}
