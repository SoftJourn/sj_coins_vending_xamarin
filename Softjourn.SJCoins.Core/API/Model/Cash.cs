using Newtonsoft.Json;

namespace Softjourn.SJCoins.Core.API.Model
{
    public sealed class Cash
    {
        [JsonProperty("tokenContractAddress")]
        public string TokenContractAddress { get; set; }

        [JsonProperty("offlineContractAddress")]
        public string OfflineContractAddress { get; set; }

        [JsonProperty("chequeHash")]
        public string ChequeHash { get; set; }

        [JsonProperty("amount")]
        public long Amount { get; set; }
    }
}
