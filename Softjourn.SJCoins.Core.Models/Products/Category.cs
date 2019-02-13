using Newtonsoft.Json;

namespace Softjourn.SJCoins.Core.Models.Products
{
    public sealed class Category
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}