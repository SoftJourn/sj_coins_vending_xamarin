using Newtonsoft.Json;

namespace Softjourn.SJCoins.Core.Models.AccountInfo
{
    public sealed class Balance
    {
        [JsonProperty("amount")]
        public string Amount { get; set; }
    }
}