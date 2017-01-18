using RestSharp.Portable;
using RestSharp.Portable.Deserializers;
using Softjourn.SJCoins.Core.API;
using Softjourn.SJCoins.Core.API.Model;
using Softjourn.SJCoins.Core.Exceptions;
using Softjourn.SJCoins.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Softjourn.SJCoins.Core.API
{
    public class ApiClient : BaseApiClient
    {

        public const string GrandTypePassword = "password";
        public const string GrandTypeRefreshToken = "refresh_token";

        public const string BaseUrl = "https://sjcoins-testing.softjourn.if.ua";
        public const string LoginAuthorizationHeader = "Basic dXNlcl9jcmVkOnN1cGVyc2VjcmV0";

        public const string VendingApiVersion = "v1/";
        public const string CoinsApiVersion = "api/v1/";

        public const string UrlAuthService = "/auth/";
        public const string UrlVendingService = "/vending/" + VendingApiVersion;
        public const string UrlCoinService = "/coins/" + CoinsApiVersion;

        public const string UrlLogin = UrlAuthService + "oauth/token";

        //public const string UrlGetMachinesList = UrlVendingService + "machines";
        //public const string UrlLogin = UrlAuthService + "oauth/token";
        //public const string UrlLogin = UrlAuthService + "oauth/token";

        public ApiClient() : base(){

            }

        public async Task<Session> MakeLoginRequest(string userName, string password)
        {
            var apiClient = GetApiClient();
            var request = new RestRequest(UrlLogin, Method.POST);
            request.AddParameter("username", userName);
            request.AddParameter("password", password);
            request.AddParameter("grant_type", GrandTypePassword);
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            request.AddHeader("Authorization", LoginAuthorizationHeader);
            JsonDeserializer deserial = new JsonDeserializer();
            
            try
            {
                IRestResponse response = await apiClient.Execute(request);

               if (response.StatusCode == HttpStatusCode.OK) { 
                    var content = response.Content;
                    Session session = deserial.Deserialize<Session>(response);
                    SaveTokens(session);
                return session;
            } else {
                    apiClient.Dispose();
                    ApiErrorHandler(response);
                }                       
            }
            catch (ApiNotAuthorizedException)
            {
                await MakeLoginRequest(userName, password);

            }
            catch (Exception)
            {
                apiClient.Dispose();
            }
            finally
            {
                
            }
            return null;
        }

        private async void RefreshToken(string refreshToken)
        {
            var apiClient = GetApiClient();
            var request = new RestRequest(UrlLogin, Method.POST);
            request.AddParameter("refresh_token", refreshToken);
            request.AddParameter("grant_type", GrandTypePassword);
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            request.AddHeader("Authorization", LoginAuthorizationHeader);
            JsonDeserializer deserial = new JsonDeserializer();

            try
            {
                IRestResponse response = await apiClient.Execute(request);
                if (response.IsSuccess) {
                    var content = response.Content;
                    Session session = deserial.Deserialize<Session>(response);
                    SaveTokens(session);
                } else { 
                    ApiErrorHandler(response);
                }

            } catch (ApiNotAuthorizedException)
            {
                throw new ApiException("Not Authorized");
            }                
            
             finally
            {

            }
        }

        private void ApiErrorHandler(IRestResponse response)
        {
            switch (response.StatusCode)
            {
                case HttpStatusCode.Unauthorized:
                    throw new ApiNotAuthorizedException("Not Authorized");
                default:
                    throw new ApiException("general");
            }
        } 

        private string GetOAuthAuthorizationHeader()
        {
            return $"Bearer {Settings.AccessToken}";
        }

        private void SaveTokens(Session session)
        {
            Settings.AccessToken = session.AccessToken;
            Settings.RefreshToken = session.RefreshToken;
        }
    }

}
