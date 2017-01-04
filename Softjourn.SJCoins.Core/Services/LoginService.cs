using System;
using Softjourn.SJCoins.Core.Interfaces;

namespace Softjourn.SJCoins.Core.Services
{
	public class LoginService: ILoginService
	{
		public LoginService() // e.g. LoginService(IMyApiClient client)
		{
			// this constructor would most likely contain some form of API Client that performs
			// the message creation, sending and deals with the response from a remote API
		}

		// Gets a value indicating whether the user is authenticated.
		public bool IsAuthenticated { get; private set; }

		// Gets the error message.
		public string ErrorMessage { get; private set; }

		// The login method to retrieve OAuth2 access tokens from an API.
		public bool Login(string userName, string password)
		{
			//try
			//{
			//	//IsAuthenticated = _apiClient.ExchangeUserCredentialsForTokens(userName, password); // somethind like that
			//	return IsAuthenticated;
			//}
			//catch (ArgumentException argex)
			//{
			//	ErrorMessage = argex.Message;
			//	IsAuthenticated = false;
			//	return IsAuthenticated;
			//}

			// Now simply returns true to mock a real login service call
			return true;
		}
	}
}
