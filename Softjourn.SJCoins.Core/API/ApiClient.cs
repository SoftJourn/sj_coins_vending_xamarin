﻿using RestSharp.Portable;
using RestSharp.Portable.Deserializers;
using Softjourn.SJCoins.Core.API.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Softjourn.SJCoins.Core.API
{
    public class ApiClient : BaseApiClient
    {

        public const string BaseUrl = "https://sjcoins-testing.softjourn.if.ua";

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

        public async void MakeLoginRequest(string email, string password, string type, Action<Session> action)
        {
            var request = new RestRequest(UrlLogin, Method.POST);
            request.AddParameter("username", email);
            request.AddParameter("password", password);
            request.AddParameter("grant_type", type);
            JsonDeserializer deserial = new JsonDeserializer();
            IRestResponse response = await ApiClient.Execute(request);
            var content = response.Content;
            Session session = deserial.Deserialize<Session>(response);
            action(session);
        }
    }

}
