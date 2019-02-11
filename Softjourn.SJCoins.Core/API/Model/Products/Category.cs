using Newtonsoft.Json;

namespace Softjourn.SJCoins.Core.API.Model.Products
{
    public sealed class Category
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}