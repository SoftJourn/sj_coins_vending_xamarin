using Newtonsoft.Json;

namespace Softjourn.SJCoins.Core.API.Model.AccountInfo
{
    public sealed class Balance
    {
        [JsonProperty("amount")]
        public string Amount { get; set; }
    }
}