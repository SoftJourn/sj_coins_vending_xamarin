
using Newtonsoft.Json;

namespace Softjourn.SJCoins.Core.API.Model.AccountInfo
{
    class Balance
    {
        [JsonProperty("amount")]
        public string Amount { get; set; }
    }
}