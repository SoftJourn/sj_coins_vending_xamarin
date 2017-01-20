using RestSharp.Portable;
using RestSharp.Portable.Deserializers;
using RestSharp.Portable.HttpClient;
using Softjourn.SJCoins.Core.API;
using Softjourn.SJCoins.Core.API.Model;
using Softjourn.SJCoins.Core.API.Model.Machines;
using Softjourn.SJCoins.Core.Exceptions;
using Softjourn.SJCoins.Core.Helpers;
using Softjourn.SJCoins.Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Softjourn.SJCoins.Core.API
{
    public class ApiClient
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


        public ApiClient() {

            }

        #region OAuth Server Calls
        public async Task<Session> MakeLoginRequest(string userName, string password)
        {
            var apiClient = GetApiClient();
            string url = UrlAuthService + "oauth/token";
            var request = new RestRequest(url, Method.POST);
            request.AddParameter("username", userName);
            request.AddParameter("password", password);
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
                return session;
            } else {
                    ApiErrorHandler(response);
                }                       
            }
            // catch is missing because all exceptions should be caught on Presenter side
            finally
            {
                apiClient.Dispose(); 
            }
            return null;
        }

        private async Task<Session> RefreshToken()
        {
            var apiClient = GetApiClient();
            string url = UrlAuthService + "oauth/token";
            var request = new RestRequest(url, Method.POST);
            request.AddParameter("refresh_token", Settings.RefreshToken);
            request.AddParameter("grant_type", GrandTypeRefreshToken);
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
                    return session;
                } else { 
                    ApiErrorHandler(response);
                }
            }
            // catch is missing because all exceptions should be caught on Presenter side
            finally
            {
                apiClient.Dispose();
            }
            return null;
        }

        public async void RevokeToken()
        {
            var apiClient = GetApiClient();
            string url = UrlAuthService + "oauth/token/revoke";
            var request = new RestRequest(url, Method.POST);
            request.AddParameter("token_value", Settings.RefreshToken);
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            request.AddHeader("Authorization", LoginAuthorizationHeader);

            try
            {
                IRestResponse response = await apiClient.Execute(request);
                if (!response.IsSuccess)
                {
                    ApiErrorHandler(response);
                }
            }
            // catch is missing because all exceptions should be caught on Presenter side
            finally
            {
                apiClient.Dispose();
            }
        }
        #endregion

        #region Vending machines calls
        public async Task<List<Machines>> GetMachinesList()
        {
            string url = UrlVendingService + "machines";
            List<Machines> list = await MakeRequest<List<Machines>>(url, Method.GET);
            return list;
        }

        public async Task<Machines> GetMachineById(string machineId)
        {
            string url = UrlVendingService + $"machines/{machineId}";
            Machines machine = await MakeRequest<Machines>(url, Method.GET);
            return machine;
        }
        #endregion

        private async Task<TResult> MakeRequest<TResult>(string url, Method httpMethod)
        {
            var apiClient = GetApiClient();
            var request = new RestRequest(url, httpMethod);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Authorization", GetOAuthAuthorizationHeader());
            JsonDeserializer deserial = new JsonDeserializer();

            try
            {
                IRestResponse response = await apiClient.Execute(request);

                if (response.IsSuccess)
                {
                    var content = response.Content;
                    TResult data = deserial.Deserialize<TResult>(response);
                    return data;
                }
                else
                {
                    ApiErrorHandler(response);
                }
            }
            catch (ApiNotAuthorizedException)
            {
                await RefreshToken();
                return await MakeRequest<TResult>(url, httpMethod);
            }
            // all another exceptions should be caught on Presenter side
            finally
            {
                apiClient.Dispose();
            }

            return default(TResult);
        }

        private void ApiErrorHandler(IRestResponse response)
        {
            string errorDescription = response.StatusDescription;
            switch (response.StatusCode)
            {
                case HttpStatusCode.BadRequest:
                    throw new ApiBadRequestException(errorDescription);
                case HttpStatusCode.Unauthorized:
                    throw new ApiNotAuthorizedException(errorDescription);
                default:
                    throw new ApiException(errorDescription);
            }
        }

        private IRestClient GetApiClient()
        {
            IRestClient apiClient = new RestClient();
            apiClient.BaseUrl = new Uri(Const.BaseUrl);
            apiClient.IgnoreResponseStatusCode = true;
            return apiClient;
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
