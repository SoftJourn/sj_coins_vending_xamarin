using RestSharp.Portable.HttpClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Softjourn.SJCoins.Core.API
{
    public class BaseApiClient
    {
        public RestClient _apiClient;

        public BaseApiClient()
        {
            _apiClient = new RestClient();
        }

    }
}
