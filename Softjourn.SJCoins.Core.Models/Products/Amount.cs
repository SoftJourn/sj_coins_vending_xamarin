using Newtonsoft.Json;

namespace Softjourn.SJCoins.Core.Models.Products
{
    public sealed class Amount
    {
        [JsonProperty("amount")]
        public string Balance { get; set; }
    }
}
