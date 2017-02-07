using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Softjourn.SJCoins.Core.Utils;

namespace Softjourn.SJCoins.Core.API.Model.Products
{
    public class Product 
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("price")]
        public float Price { get; set; }

        public int IntPrice { get { return int.Parse(Price.ToString()); } }

        public string ImageFullUrl => Const.BaseUrl + Const.UrlVendingService + ImageUrl;

        public bool IsProductFavorite { get; set; }

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
