using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Softjourn.SJCoins.Core.API.Coins;
using Softjourn.SJCoins.Core.API.OAuth;
using Softjourn.SJCoins.Core.API.Vending;

namespace Softjourn.SJCoins.Core.API
{

    public class ApiManager
    {
        private static ApiManager _apiManager;
        private static OAuthApiClient _authApiClient;
        private static CoinsApiClient _coinsApiClient;
        private static VendingApiClient _vendingApiClient;

        public static ApiManager GetInstance()
        {
            if (_apiManager == null)
            {
                _apiManager = new ApiManager();
                _authApiClient = new OAuthApiClient();
                _coinsApiClient = new CoinsApiClient();
                _vendingApiClient = new VendingApiClient();
            }
            return _apiManager;
        }

        public IOAuthApiProvider GetOauthApiProvider()
        {
            return _authApiClient;
        }

        public IVendingApiProvider GetVendingProcessApiProvider()
        {
            return _vendingApiClient;
        }

        public ICoinsApiProvider GetCoinsApiProvider()
        {
            return _coinsApiClient;
        }
    }
}
