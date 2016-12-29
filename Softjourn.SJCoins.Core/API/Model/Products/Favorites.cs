
using Newtonsoft.Json;

namespace Softjourn.SJCoins.Core.API.Model.Products
{
    class Favorites
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("price")]
        private int Price { get; set; }

        [JsonProperty("name")]
        private string Name { get; set; }

        [JsonProperty("imageUrl")]
        private string ImageUrl { get; set; }

        [JsonProperty("description")]
        private string Description { get; set; }
    }
}