using System.Collections.Generic;
using Newtonsoft.Json;

namespace Softjourn.SJCoins.Core.API.Model.Products
{
    public sealed class Categories
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("products")]
        public List<Product> Products { get; set; }
    }
}
