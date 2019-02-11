using Newtonsoft.Json;

namespace Softjourn.SJCoins.Core.API.Model.Products
{
    public sealed class Amount
    {
        [JsonProperty("amount")]
        public string Balance { get; set; }
    }
}
