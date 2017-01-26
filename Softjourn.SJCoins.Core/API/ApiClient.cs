using RestSharp.Portable;
using RestSharp.Portable.Deserializers;
using RestSharp.Portable.HttpClient;
using Softjourn.SJCoins.Core.API;
using Softjourn.SJCoins.Core.API.Model;
using Softjourn.SJCoins.Core.API.Model.AccountInfo;
using Softjourn.SJCoins.Core.API.Model.Machines;
using Softjourn.SJCoins.Core.API.Model.Products;
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

        public async Task<EmptyResponse> RevokeToken()
        {
            var apiClient = GetApiClient();
            string url = UrlAuthService + "oauth/token/revoke";
            var request = new RestRequest(url, Method.POST);
            request.AddParameter("token_value", Settings.RefreshToken);
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            request.AddHeader("Authorization", LoginAuthorizationHeader);
            JsonDeserializer deserial = new JsonDeserializer();
            try
            {
                IRestResponse response = await apiClient.Execute(request);
                if (response.IsSuccess)
                {
                    var content = response.Content;
                    EmptyResponse emptyResponce = deserial.Deserialize<EmptyResponse>(response);
                    return emptyResponce;
                }
                else
                {
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

        public async Task<Featured> GetFeaturedProducts(string machineId)
        {
            string url = UrlVendingService + $"machines/{machineId}/features";
            Featured featuredProducts = await MakeRequest<Featured>(url, Method.GET);
            return featuredProducts;
        }

        public async Task<List<Product>> GetProductsList(string machineId)
        {
            string url = UrlVendingService + $"machines/{machineId}/products";
            List<Product> productsList = await MakeRequest<List<Product>>(url, Method.GET);
            return productsList;
        }

        public async Task<Amount> BuyProductById(string machineId, string productId)
        {
            string url = UrlVendingService + $"machines/{machineId}/products/{productId}";
            Amount productAmount = await MakeRequest<Amount>(url, Method.POST);
            return productAmount;
        }

        public async Task<List<Favorites>> GetFavoritesList()
        {
            string url = UrlVendingService + $"favorites";
            List<Favorites> favoritesList = await MakeRequest<List<Favorites>>(url, Method.GET);
            return favoritesList;
        }

        public async Task<EmptyResponse> AddProductToFavorites(string productId)
        {
            string url = UrlVendingService + $"favorites/{productId}";
            EmptyResponse response = await MakeRequest<EmptyResponse>(url, Method.POST);
            return response;
        }

        public async Task<EmptyResponse> RemoveProductFromFavorites(string productId)
        {
            string url = UrlVendingService + $"favorites/{productId}";
            EmptyResponse response = await MakeRequest<EmptyResponse>(url, Method.DELETE);
            return response;
        }

        public async Task<List<History>> GetPurchaseHistory()
        {
            string url = UrlVendingService + "machines/last";
            List<History> historyList = await MakeRequest<List<History>>(url, Method.GET);
            return historyList;
        }

        #endregion

        #region Coins server endpoints

        public async Task<Account> GetUserAccountAsync()
        {
            string url = UrlCoinService + "account";
            Account account = await MakeRequest<Account>(url, Method.GET);
            return account;
        }

        public async Task<Balance> GetBalanceAsync()
        {
            string url = UrlCoinService + "amount";
            Balance balance = await MakeRequest<Balance>(url, Method.GET);
            return balance;
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
