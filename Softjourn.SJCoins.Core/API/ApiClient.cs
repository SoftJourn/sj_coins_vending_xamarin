﻿using RestSharp.Portable;
using RestSharp.Portable.Deserializers;
using Softjourn.SJCoins.Core.API;
using Softjourn.SJCoins.Core.API.Model;
using Softjourn.SJCoins.Core.API.Model.Machines;
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

        public const string UrlGetMachinesList = UrlVendingService + "machines";

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

               if (response.IsSuccess) { 
                    var content = response.Content;
                    Session session = deserial.Deserialize<Session>(response);
                    SaveTokens(session);
                return session;
            } else {
                    ApiErrorHandler(response);
                }                       
            }
            finally
            {
                apiClient.Dispose(); 
            }
            return null;
        }

        private async Task<Session> RefreshToken()
        {
            var apiClient = GetApiClient();
            var request = new RestRequest(UrlLogin, Method.POST);
            request.AddParameter("refresh_token", Settings.RefreshToken);
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

            } catch (ApiNotAuthorizedException)
            {
                throw new ApiException("Not Authorized");
            }                
            
             finally
            {
                apiClient.Dispose();
            }
            return null;
        }

        public async Task<List<Machines>> GetMachinesList()
        {
            var apiClient = GetApiClient();
            var request = new RestRequest(UrlGetMachinesList, Method.GET);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Authorization", GetOAuthAuthorizationHeader());
            JsonDeserializer deserial = new JsonDeserializer();

            try
            {
                IRestResponse response = await apiClient.Execute(request);

                if (response.IsSuccess)
                {
                    var content = response.Content;
                    List<Machines> machinesList = deserial.Deserialize<List<Machines>>(response);
                    return machinesList;
                }
                else
                {
                    ApiErrorHandler(response);
                }
            }
            catch (ApiNotAuthorizedException)
            {
                await RefreshToken();
                return await GetMachinesList();
            }
            finally
            {
                apiClient.Dispose();
            }
            return null;
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
