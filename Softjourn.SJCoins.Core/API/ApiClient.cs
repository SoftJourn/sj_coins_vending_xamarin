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

        public ApiClient() : base(){

            }



    }

}
