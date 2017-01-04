namespace Softjourn.SJCoins.Core.Interfaces
{
	public interface ILoginService
	{
		// Gets a value indicating whether the user is authenticated.
		bool IsAuthenticated { get; }

		// The login method to retrieve OAuth2 access tokens from an API.
		// "userName" The user Name (email address) </param>
		// "password" The users password
		// Returns true if the login is successful, false otherwise
		bool Login(string userName, string password);
	}
}
