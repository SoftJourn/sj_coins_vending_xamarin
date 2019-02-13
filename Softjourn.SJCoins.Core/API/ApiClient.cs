using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RestSharp.Portable;
using RestSharp.Portable.Deserializers;
using RestSharp.Portable.HttpClient;
using Softjourn.SJCoins.Core.Common;
using Softjourn.SJCoins.Core.Common.Exceptions;
using Softjourn.SJCoins.Core.Common.Utils;
using Softjourn.SJCoins.Core.Models;
using Softjourn.SJCoins.Core.Models.AccountInfo;
using Softjourn.SJCoins.Core.Models.Machines;
using Softjourn.SJCoins.Core.Models.Products;
using Softjourn.SJCoins.Core.Models.TransactionReports;

namespace Softjourn.SJCoins.Core.Managers.Api
{
    public sealed class ApiClient
    {
        public const string UrlAuthService = Constant.UrlAuthService;
        public const string UrlVendingService = Constant.UrlVendingService;
        public const string UrlCoinService = Constant.UrlCoinService;

        #region OAuth Server Calls

        public async Task<Session> MakeLoginRequestAsync(string userName, string password)
        {
            var url = $"{UrlAuthService}/oauth/token";
            var request = new RestRequest(url, Method.POST);

            request.AddParameter(Constant.HttpParameter.UserNameKey, userName);
            request.AddParameter(Constant.HttpParameter.PasswordKey, password);
            request.AddParameter(Constant.HttpParameter.GrantTypeKey, Constant.HttpParameter.PasswordValue);
            request.AddHeader(Constant.HttpHeader.ContentTypeKey, Constant.HttpHeader.ApplicationFormValue);
            request.AddHeader(Constant.HttpHeader.AuthorizationKey, Constant.HttpHeader.BasicAuthorizationValue);

            using (var apiClient = GetApiClient())
            {
                var response = await apiClient.Execute(request);

                if (response.IsSuccess)
                {
                    var deserializer = new JsonDeserializer();
                    var session = deserializer.Deserialize<Session>(response);
                    SaveTokens(session);

                    return session;
                }

                ApiErrorHandler(response);
            }

            return null;
        }

        private static async Task<Session> RefreshTokenAsync()
        {
            var url = $"{UrlAuthService}/oauth/token";
            var request = new RestRequest(url, Method.POST);

            request.AddParameter(Constant.HttpParameter.RefreshTokenValue, Settings.RefreshToken);
            request.AddParameter(Constant.HttpParameter.GrantTypeKey, Constant.HttpParameter.RefreshTokenValue);
            request.AddHeader(Constant.HttpHeader.ContentTypeKey, Constant.HttpHeader.ApplicationFormValue);
            request.AddHeader(Constant.HttpHeader.AuthorizationKey, Constant.HttpHeader.BasicAuthorizationValue);

            using (var apiClient = GetApiClient())
            {
                var response = await apiClient.Execute(request);
                if (response.IsSuccess)
                {
                    var deserializer = new JsonDeserializer();
                    var session = deserializer.Deserialize<Session>(response);
                    SaveTokens(session);

                    var debugMessage = session == null
                        ? "Tokens no saved !"
                        : "Tokens saved !";
                    Debug.WriteLine(debugMessage);

                    return session;
                }

                Debug.WriteLine("Refresh token Failure!");
                throw new ApiNotAuthorizedException(Resources.ServerErrorResources.server_error_401);
            }
        }

        public async Task RevokeToken()
        {
            var url = $"{UrlAuthService}/oauth/token/revoke";
            var request = new RestRequest(url, Method.POST);

            request.AddParameter(Constant.HttpParameter.TokenValueKey, Settings.RefreshToken);
            request.AddHeader(Constant.HttpHeader.ContentTypeKey, Constant.HttpHeader.ApplicationFormValue);
            request.AddHeader(Constant.HttpHeader.AuthorizationKey, Constant.HttpHeader.BasicAuthorizationValue);

            using (var apiClient = GetApiClient())
            {
                var response = await apiClient.Execute(request);
                if (!response.IsSuccess)
                    ApiErrorHandler(response);
            }
        }

        #endregion

        #region Vending machines calls

        public async Task<List<Machines>> GetMachinesListAsync()
        {
            var url = $"{UrlVendingService}/machines";
            var list = await MakeRequestAsync<List<Machines>>(url, Method.GET);

            return list;
        }

        public async Task<Machines> GetMachineByIdAsync(string machineId)
        {
            var url = $"{UrlVendingService}/machines/{machineId}";
            var machine = await MakeRequestAsync<Machines>(url, Method.GET);

            return machine;
        }

        public async Task<Featured> GetFeaturedProductsAsync(string machineId)
        {
            var url = $"{UrlVendingService}/machines/{machineId}/features";
            var featuredProducts = await MakeRequestAsync<Featured>(url, Method.GET);

            return featuredProducts;
        }

        public async Task<List<Product>> GetProductsListAsync(string machineId)
        {
            var url = $"{UrlVendingService}/machines/{machineId}/products";
            var productsList = await MakeRequestAsync<List<Product>>(url, Method.GET);

            return productsList;
        }

        public async Task<Amount> BuyProductByIdAsync(string machineId, string productId)
        {
            var url = $"{UrlVendingService}/machines/{machineId}/products/{productId}";
            var productAmount = await MakeRequestAsync<Amount>(url, Method.POST);

            return productAmount;
        }

        public async Task<List<Product>> GetFavoritesListAsync()
        {
            var url = $"{UrlVendingService}/favorites";
            var favoritesList = await MakeRequestAsync<List<Product>>(url, Method.GET);

            return favoritesList;
        }

        public async Task AddProductToFavoritesAsync(string productId)
        {
            var url = $"{UrlVendingService}/favorites/{productId}";
            await MakeRequestAsync(url, Method.POST);
        }

        public async Task RemoveProductFromFavoritesAsync(string productId)
        {
            var url = $"{UrlVendingService}/favorites/{productId}";
            await MakeRequestAsync(url, Method.DELETE);
        }

        public async Task<List<History>> GetPurchaseHistoryAsync()
        {
            var url = $"{UrlVendingService}/machines/last";
            var historyList = await MakeRequestAsync<List<History>>(url, Method.GET);

            return historyList;
        }

        #endregion

        #region Coins server endpoints

        public async Task<Account> GetUserAccountAsync()
        {
            var url = $"{UrlCoinService}/account";
            var account = await MakeRequestAsync<Account>(url, Method.GET);

            return account;
        }

        public async Task<Balance> GetBalanceAsync()
        {
            var url = $"{UrlCoinService}/amount";
            var balance = await MakeRequestAsync<Balance>(url, Method.GET);

            return balance;
        }

        public async Task<DepositeTransaction> GetOfflineMoney(Cash scannedCode)
        {
            var url = $"{UrlCoinService}/deposit";
            var deposit = await MakeRequestWithBodyAsync<DepositeTransaction>(url, Method.POST, scannedCode);

            return deposit;
        }

        public async Task<Cash> WithdrawMoney(Amount amount)
        {
            var url = $"{UrlCoinService}/withdraw";
            var cash = await MakeRequestWithBodyAsync<Cash>(url, Method.POST, amount);

            return cash;
        }

        public async Task<Report> GetTransactionReport(TransactionRequest transactionRequest)
        {
            var url = $"{UrlCoinService}/transactions/my";
            var transactionReport = await MakeRequestWithQueryParametersAsync<Report>(url, Method.GET, transactionRequest);

            return transactionReport;
        }

        public async Task<byte[]> GetAvatarImage(string endpoint)
        {
            var url = $"{UrlCoinService}/{endpoint}";
            var result = await MakeRequestForFile(url, Method.GET);

            return result;
        }

        public async Task SetAvatarImage(byte[] image)
        {
            var url = $"{UrlCoinService}/account/image";
            await MakeRequestPostFile(url, image);
        }

        #endregion

        private static async Task<TResult> MakeRequestAsync<TResult>(string url, Method httpMethod)
        {
            var request = new RestRequest(url, httpMethod);

            request.AddHeader(Constant.HttpHeader.ContentTypeKey, Constant.HttpHeader.ApplicationJsonValue);
            request.AddHeader(Constant.HttpHeader.AuthorizationKey, GetOAuthAuthorizationHeader());

            try
            {
                using (var apiClient = GetApiClient())
                {
                    var response = await apiClient.Execute(request);
                    if (response.IsSuccess)
                    {
                        Debug.WriteLine($"Response status code: {response.StatusCode}");

                        var deserializer = new JsonDeserializer();
                        var data = deserializer.Deserialize<TResult>(response);

                        return data;
                    }

                    ApiErrorHandler(response);
                }
            }
            catch (ApiNotAuthorizedException)
            {
                await RefreshTokenAsync();

                return await MakeRequestAsync<TResult>(url, httpMethod);
            }

            return default(TResult);
        }

        private static async Task MakeRequestAsync(string url, Method httpMethod)
        {
            var request = new RestRequest(url, httpMethod);

            request.AddHeader(Constant.HttpHeader.ContentTypeKey, Constant.HttpHeader.ApplicationJsonValue);
            request.AddHeader(Constant.HttpHeader.AuthorizationKey, GetOAuthAuthorizationHeader());

            try
            {
                using (var apiClient = GetApiClient())
                {
                    var response = await apiClient.Execute(request);
                    if (!response.IsSuccess)
                        ApiErrorHandler(response);
                }
            }
            catch (ApiNotAuthorizedException)
            {
                await RefreshTokenAsync();
                await MakeRequestAsync(url, httpMethod);
            }
        }

        private static async Task<byte[]> MakeRequestForFile(string url, Method httpMethod)
        {
            var request = new RestRequest(url, httpMethod);

            request.AddHeader(Constant.HttpHeader.ContentTypeKey, Constant.HttpHeader.MultipartFormDataValue);
            request.AddHeader(Constant.HttpHeader.AuthorizationKey, GetOAuthAuthorizationHeader());

            try
            {
                using (var apiClient = GetApiClient())
                {
                    var response = await apiClient.Execute(request);

                    if (response.IsSuccess)
                    {
                        var data = response.RawBytes;

                        return data;
                    }

                    ApiErrorHandler(response);
                }
            }
            catch (ApiNotAuthorizedException)
            {
                await RefreshTokenAsync();

                return await MakeRequestForFile(url, httpMethod);
            }

            return new byte[0];
        }

        private static async Task MakeRequestPostFile(string url, byte[] image)
        {
            const string boundary = "---8d0f01e6b3b5dafaaadaad";

            var fileContent = new ByteArrayContent(image);

            fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse(Constant.HttpHeader.MultipartFormDataValue);
            fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue(Constant.HttpHeader.FormDataValue)
            {
                Name = "file",
                FileName = "avatar.jpg"
            };

            var multipartContent = new MultipartFormDataContent(boundary) { fileContent };
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add(Constant.HttpHeader.AuthorizationKey, GetOAuthAuthorizationHeader());
            var response = await httpClient.PostAsync($"{Constant.BaseUrl}/{url}", multipartContent);

            try
            {
                if (!response.IsSuccessStatusCode)
                    throw new ApiNotFoundException(NetworkErrorUtils.GetErrorMessage(409));
            }
            catch (ApiNotAuthorizedException)
            {
                await RefreshTokenAsync();
                await MakeRequestPostFile(url, image);
            }
            finally
            {
                httpClient.Dispose();
            }
        }

        private static async Task<TResult> MakeRequestWithQueryParametersAsync<TResult>(string url, Method httpMethod, TransactionRequest transactionRequest)
        {
            var request = new RestRequest(url, httpMethod);

            request.AddHeader(Constant.HttpHeader.ContentTypeKey, Constant.HttpHeader.ApplicationJsonValue);
            request.AddHeader(Constant.HttpHeader.AuthorizationKey, GetOAuthAuthorizationHeader());
            request.AddQueryParameter("size", transactionRequest.Size);
            request.AddQueryParameter("page", transactionRequest.Page);
            request.AddQueryParameter("sort",  $"{transactionRequest.Sort[0].Property},{transactionRequest.Sort[0].Direction}");
            request.AddQueryParameter("direction", transactionRequest.Direction);

            try
            {
                using (var apiClient = GetApiClient())
                {
                    var response = await apiClient.Execute(request);

                    if (response.IsSuccess)
                    {
                        var deserializer = new JsonDeserializer();
                        var data = deserializer.Deserialize<TResult>(response);

                        return data;
                    }

                    ApiErrorHandler(response);
                }
            }
            catch (ApiNotAuthorizedException)
            {
                await RefreshTokenAsync();

                return await MakeRequestWithQueryParametersAsync<TResult>(url, httpMethod, transactionRequest);
            }

            return default(TResult);
        }

        private static async Task<TResult> MakeRequestWithBodyAsync<TResult>(string url, Method httpMethod, object body)
        {
            var request = new RestRequest(url, httpMethod);

            request.AddHeader(Constant.HttpHeader.ContentTypeKey, Constant.HttpHeader.ApplicationJsonValue);
            request.AddHeader(Constant.HttpHeader.AuthorizationKey, GetOAuthAuthorizationHeader());
            request.AddBody(body);

            try
            {
                using (var apiClient = GetApiClient())
                {
                    var response = await apiClient.Execute(request);

                    if (response.IsSuccess)
                    {
                        var deserializer = new JsonDeserializer();
                        var data = deserializer.Deserialize<TResult>(response);

                        return data;
                    }

                    ApiErrorHandler(response);
                }
            }
            catch (ApiNotAuthorizedException)
            {
                await RefreshTokenAsync();

                return await MakeRequestWithBodyAsync<TResult>(url, httpMethod, body);
            }

            return default(TResult);
        }

        // get error code from response and generate Exception with needed message
        private static void ApiErrorHandler(IRestResponse response)
        {
            switch (response.StatusCode)
            {
                case HttpStatusCode.BadRequest: // code 400
                    throw new ApiBadRequestException(Resources.ServerErrorResources.server_error_400);
                case HttpStatusCode.Unauthorized: // code 401 
                    throw new ApiNotAuthorizedException(Resources.ServerErrorResources.server_error_401);
                case HttpStatusCode.NotFound: // code 404               
                    try
                    {
                        var deserializer = new JsonDeserializer();
                        var badResponse = deserializer.Deserialize<BadResponse>(response);
                        if (badResponse != null)
                            throw new ApiNotFoundException(NetworkErrorUtils.GetErrorMessage(badResponse.Code));

                        throw new ApiNotFoundException(NetworkErrorUtils.GetErrorMessage(404));
                    }
                    catch (Exception)
                    {
                        throw new ApiNotFoundException(NetworkErrorUtils.GetErrorMessage(404));
                    }
                case HttpStatusCode.Conflict: // code 409                  
                    try
                    {
                        var deserializer = new JsonDeserializer();
                        var badResponse = deserializer.Deserialize<BadResponse>(response);
                        if (badResponse != null)
                            throw new ApiNotFoundException(NetworkErrorUtils.GetErrorMessage(badResponse.Code));

                        throw new ApiNotFoundException(NetworkErrorUtils.GetErrorMessage(409));
                    }
                    catch (JsonSerializationException)
                    {
                        throw new ApiNotFoundException(NetworkErrorUtils.GetErrorMessage(409));
                    }
                default: // for all rest codes
                    throw new ApiException(NetworkErrorUtils.GetErrorMessage(500));
            }
        }

        private static IRestClient GetApiClient()
        {
            IRestClient apiClient = new RestClient();
            apiClient.BaseUrl = new Uri(Constant.BaseUrl);
            apiClient.IgnoreResponseStatusCode = true;

            return apiClient;
        }

        private static string GetOAuthAuthorizationHeader()
        {
            return $"Bearer {Settings.AccessToken}";
        }

        private static void SaveTokens(Session session)
        {
            Settings.AccessToken = session.AccessToken;
            Settings.RefreshToken = session.RefreshToken;
        }
    }
}
