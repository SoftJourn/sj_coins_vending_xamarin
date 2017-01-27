using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Softjourn.SJCoins.Core.API.Model.Products
{
    public class Product 
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("price")]
        public float Price { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("imageUrl")]
        public string ImageUrl { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("category")]
        public Category Category { get; set; }
    }
}
