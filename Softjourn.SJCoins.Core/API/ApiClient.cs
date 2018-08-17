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
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Softjourn.SJCoins.Core.API.Model.TransactionReports;

namespace Softjourn.SJCoins.Core.API
{
    public class ApiClient
    {

        public const string GrandTypePassword = "password";
        public const string GrandTypeRefreshToken = "refresh_token";

        public const string BaseUrl = Const.BaseUrl;
        public const string LoginAuthorizationHeader = Const.HeaderAuthorizationValue;

        public const string UrlAuthService = Const.UrlAuthService;
        public const string UrlVendingService = Const.UrlVendingService;
        public const string UrlCoinService = Const.UrlCoinService;


        public ApiClient()
        {

        }

        #region OAuth Server Calls
        public async Task<Session> MakeLoginRequestAsync(string userName, string password)
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

                if (response.IsSuccess)
                {
                    Session session = deserial.Deserialize<Session>(response);
                    SaveTokens(session);
                    return session;
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

        private async Task<Session> RefreshTokenAsync()
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
                if (response.IsSuccess)
                {
                    Session session = deserial.Deserialize<Session>(response);
                    SaveTokens(session);
                    System.Diagnostics.Debug.WriteLine(String.Format(session == null ? "Tokens no saved !" : "Tokens saved !"));
                    return session;
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine(String.Format("Refresh token Failure!"));
                    throw new ApiNotAuthorizedException(Resources.StringResources.server_error_401);
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
        public async Task<List<Machines>> GetMachinesListAsync()
        {
            string url = UrlVendingService + "machines";
            List<Machines> list = await MakeRequestAsync<List<Machines>>(url, Method.GET);
            return list;
        }

        public async Task<Machines> GetMachineByIdAsync(string machineId)
        {
            string url = UrlVendingService + $"machines/{machineId}";
            Machines machine = await MakeRequestAsync<Machines>(url, Method.GET);
            return machine;
        }

        public async Task<Featured> GetFeaturedProductsAsync(string machineId)
        {
            string url = UrlVendingService + $"machines/{machineId}/features";
            Featured featuredProducts = await MakeRequestAsync<Featured>(url, Method.GET);
            return featuredProducts;
        }

        public async Task<List<Product>> GetProductsListAsync(string machineId)
        {
            string url = UrlVendingService + $"machines/{machineId}/products";
            List<Product> productsList = await MakeRequestAsync<List<Product>>(url, Method.GET);
            return productsList;
        }

        public async Task<Amount> BuyProductByIdAsync(string machineId, string productId)
        {
            string url = UrlVendingService + $"machines/{machineId}/products/{productId}";
            Amount productAmount = await MakeRequestAsync<Amount>(url, Method.POST);
            return productAmount;
        }

        public async Task<List<Product>> GetFavoritesListAsync()
        {
            string url = UrlVendingService + $"favorites";
            List<Product> favoritesList = await MakeRequestAsync<List<Product>>(url, Method.GET);
            return favoritesList;
        }

        public async Task<EmptyResponse> AddProductToFavoritesAsync(string productId)
        {
            string url = UrlVendingService + $"favorites/{productId}";
            EmptyResponse response = await MakeRequestAsync<EmptyResponse>(url, Method.POST);
            return response;
        }

        public async Task<EmptyResponse> RemoveProductFromFavoritesAsync(string productId)
        {
            string url = UrlVendingService + $"favorites/{productId}";
            EmptyResponse response = await MakeRequestAsync<EmptyResponse>(url, Method.DELETE);
            return response;
        }

        public async Task<List<History>> GetPurchaseHistoryAsync()
        {
            string url = UrlVendingService + "machines/last";
            List<History> historyList = await MakeRequestAsync<List<History>>(url, Method.GET);
            return historyList;
        }

        #endregion

        #region Coins server endpoints

        public async Task<Account> GetUserAccountAsync()
        {
            string url = UrlCoinService + "account";
            Account account = await MakeRequestAsync<Account>(url, Method.GET);
            return account;
        }

        public async Task<Balance> GetBalanceAsync()
        {
            string url = UrlCoinService + "amount";
            Balance balance = await MakeRequestAsync<Balance>(url, Method.GET);
            return balance;
        }

        public async Task<DepositeTransaction> GetOfflineMoney(Cash scannedCode)
        {
            var url = UrlCoinService + "deposit";
            var deposit = await MakeRequestWithBodyAsync<DepositeTransaction>(url, Method.POST, scannedCode);
            return deposit;
        }

        public async Task<Cash> WithdrawMoney(Amount amount)
        {
            var url = UrlCoinService + "withdraw";
            var cash = await MakeRequestWithBodyAsync<Cash>(url, Method.POST, amount);
            return cash;
        }

        public async Task<Report> GetTransactionReport(TransactionRequest transactionRequest)
        {
            var url = UrlCoinService + "transactions/my/";
            var transactionReport = await MakeRequestWithQueryParametersAsync<Report>(url, Method.GET, transactionRequest);
            return transactionReport;
        }

        public async Task<byte[]> GetAvatarImage(string endpoint)
        {
            var url = UrlCoinService + endpoint;
            var result = await MakeRequestForFile(url, Method.GET);
            return result;
        }

        public async Task<EmptyResponse> SetAvatarImage(byte[] image)
        {
            var url = UrlCoinService + "account/image";
            var response = await MakeRequestPostFile<EmptyResponse>(url, Method.POST, image);
            return response;
        }

        #endregion

        private async Task<TResult> MakeRequestAsync<TResult>(string url, Method httpMethod)
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
                    System.Diagnostics.Debug.WriteLine(String.Format("Responce status code: {0}", response.StatusCode));
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
                await RefreshTokenAsync();
                return await MakeRequestAsync<TResult>(url, httpMethod);
            }
            // all another exceptions should be caught on Presenter side
            finally
            {
                apiClient.Dispose();
            }

            return default(TResult);
        }

        private async Task<byte[]> MakeRequestForFile(string url, Method httpMethod)
        {
            var apiClient = GetApiClient();
            var request = new RestRequest(url, httpMethod);
            request.AddHeader("Content-Type", "multipart/form-data");
            request.AddHeader("Authorization", GetOAuthAuthorizationHeader());

            try
            {
                var response = await apiClient.Execute(request);

                if (response.IsSuccess)
                {
                    var data = response.RawBytes;
                    return data;
                }
                else
                {
                    ApiErrorHandler(response);
                }
            }
            catch (ApiNotAuthorizedException)
            {
                await RefreshTokenAsync();
                return await MakeRequestForFile(url, httpMethod);
            }
            // all another exceptions should be caught on Presenter side
            finally
            {
                apiClient.Dispose();
            }

            return new byte[0];
        }

        private async Task<TResult> MakeRequestPostFile<TResult>(string url, Method httpMethod, byte[] image)
        {

            var fileContent = new ByteArrayContent(image);

            fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");
            fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
            {
                Name = "file",
                FileName = "avatar.jpg"
            };

            var boundary = "---8d0f01e6b3b5dafaaadaad";
            var multipartContent = new MultipartFormDataContent(boundary);
            multipartContent.Add(fileContent);

            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Authorization", GetOAuthAuthorizationHeader());
            var response = await httpClient.PostAsync(BaseUrl + url, multipartContent);
            try
            {
                if (response.IsSuccessStatusCode)
                {
                    return default(TResult);
                }
                else
                {
                    throw new ApiNotFoundException(NetworkErrorUtils.GetErrorMessage(409));
                }
            }
            catch (ApiNotAuthorizedException)
            {
                await RefreshTokenAsync();
                return await MakeRequestPostFile<TResult>(url, httpMethod, image);
            }

            // all another exceptions should be caught on Presenter side
            finally
            {
                httpClient.Dispose();
            }
        }


        private async Task<TResult> MakeRequestWithQueryParametersAsync<TResult>(string url, Method httpMethod, TransactionRequest transactionRequest)
        {
            var apiClient = GetApiClient();
            var request = new RestRequest(url, httpMethod);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Authorization", GetOAuthAuthorizationHeader());
            request.AddQueryParameter("size", transactionRequest.Size);
            request.AddQueryParameter("page", transactionRequest.Page);
            request.AddQueryParameter("sort", transactionRequest.Sort[0].Property + "," + transactionRequest.Sort[0].Direction);
            request.AddQueryParameter("direction", transactionRequest.Direction);
            JsonDeserializer deserial = new JsonDeserializer();

            try
            {
                IRestResponse response = await apiClient.Execute(request);

                if (response.IsSuccess)
                {
                    var data = deserial.Deserialize<TResult>(response);
                    return data;
                }
                else
                {
                    ApiErrorHandler(response);
                }
            }
            catch (ApiNotAuthorizedException)
            {
                await RefreshTokenAsync();
                return await MakeRequestWithQueryParametersAsync<TResult>(url, httpMethod, transactionRequest);
            }
            // all another exceptions should be caught on Presenter side
            finally
            {
                apiClient.Dispose();
            }

            return default(TResult);
        }

        private async Task<TResult> MakeRequestWithBodyAsync<TResult>(string url, Method httpMethod, object body)
        {
            var apiClient = GetApiClient();
            var request = new RestRequest(url, httpMethod);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Authorization", GetOAuthAuthorizationHeader());
            request.AddBody(body);
            var deserial = new JsonDeserializer();

            try
            {
                var response = await apiClient.Execute(request);

                if (response.IsSuccess)
                {
                    var data = deserial.Deserialize<TResult>(response);
                    return data;
                }
                else
                {
                    ApiErrorHandler(response);
                }
            }
            catch (ApiNotAuthorizedException)
            {
                await RefreshTokenAsync();
                return await MakeRequestWithBodyAsync<TResult>(url, httpMethod, body);
            }
            // all another exceptions should be caught on Presenter side
            finally
            {
                apiClient.Dispose();
            }

            return default(TResult);
        }

        // get error code from response and generate Exception with needed message
        private void ApiErrorHandler(IRestResponse response)
        {

            string errorDescription = response.StatusDescription;
            switch (response.StatusCode)
            {
                case HttpStatusCode.BadRequest: // code 400
                    throw new ApiBadRequestException(Resources.StringResources.server_error_400);

                case HttpStatusCode.Unauthorized: // code 401 
                    throw new ApiNotAuthorizedException(Resources.StringResources.server_error_401);

                case HttpStatusCode.NotFound: // code 404
                    var deserial = new JsonDeserializer();
                    try
                    {
                        var badResponse = deserial.Deserialize<BadResponse>(response);
                        if (badResponse != null)
                        {
                            throw new ApiNotFoundException(NetworkErrorUtils.GetErrorMessage(badResponse.Code));
                        }
                        else
                        {
                            throw new ApiNotFoundException(NetworkErrorUtils.GetErrorMessage(404));
                        }
                    }
                    catch (Exception e)
                    {
                        throw new ApiNotFoundException(NetworkErrorUtils.GetErrorMessage(404));
                    }
                case HttpStatusCode.Conflict: // code 409
                    var deser = new JsonDeserializer();
                    try
                    {
                        var badResponse = deser.Deserialize<BadResponse>(response);
                        if (badResponse != null)
                        {
                            throw new ApiNotFoundException(NetworkErrorUtils.GetErrorMessage(badResponse.Code));
                        }
                        else
                        {
                            throw new ApiNotFoundException(NetworkErrorUtils.GetErrorMessage(409));
                        }
                    }
                    catch (JsonSerializationException e)
                    {
                        throw new ApiNotFoundException(NetworkErrorUtils.GetErrorMessage(409));
                    }
                default: // for all rest codes
                    throw new ApiException(NetworkErrorUtils.GetErrorMessage(500));
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
