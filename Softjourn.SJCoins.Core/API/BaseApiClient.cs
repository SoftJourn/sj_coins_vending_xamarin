using RestSharp.Portable;
using RestSharp.Portable.HttpClient;
using Softjourn.SJCoins.Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Softjourn.SJCoins.Core.API
{
    public class BaseApiClient
    {
        public IRestClient ApiClient
        {
            get; set;
        }

        public BaseApiClient()
        {

        }

        public IRestClient GetApiClient()
        {
            IRestClient apiClient = new RestClient();
            apiClient.BaseUrl = new Uri(Const.BaseUrl);
            apiClient.IgnoreResponseStatusCode = true;
            return apiClient;
        }

    }
}
