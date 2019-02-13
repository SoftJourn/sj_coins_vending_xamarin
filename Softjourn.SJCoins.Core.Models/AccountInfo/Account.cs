using Newtonsoft.Json;

namespace Softjourn.SJCoins.Core.Models.AccountInfo
{
    public sealed class Account
    {
        [JsonProperty("amount")]
        public int Amount { get; set; }

        [JsonProperty("image")]
        public string Image { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("surname")]
        public string Surname { get; set; }
    }
}