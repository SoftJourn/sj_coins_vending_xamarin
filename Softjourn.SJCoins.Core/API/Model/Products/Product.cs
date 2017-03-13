using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Softjourn.SJCoins.Core.Utils;

namespace Softjourn.SJCoins.Core.API.Model.Products
{
    public class Product : IEquatable<Product>
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("price")]
        public float Price { get; set; }

        public int IntPrice { get { return int.Parse(Price.ToString()); } }

        public string ImageFullUrl => Const.BaseUrl + Const.UrlVendingService + ImageUrl;

        public List<string> ImagesFullUrls => ImageUrls.Select(url => Const.BaseUrl + Const.UrlVendingService + url).ToList();

        public bool IsProductFavorite { get; set; }

        public bool IsProductInCurrentMachine { get; set; }

        public bool IsHeartAnimationRunning { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("imageUrl")]
        public string ImageUrl { get; set; }

        [JsonProperty("imageUrls")]
        public List<string> ImageUrls { get; set;}

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("category")]
        public Category Category { get; set; }

        public bool Equals(Product other)
        {
            return this.Id.Equals(other.Id);
        }
    }
}
