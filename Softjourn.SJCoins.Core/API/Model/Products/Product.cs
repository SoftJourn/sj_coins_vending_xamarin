using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Softjourn.SJCoins.Core.Utils;

namespace Softjourn.SJCoins.Core.API.Model.Products
{
    public sealed class Product : IEquatable<Product>
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("price")]
        public float Price { get; set; }

        [JsonIgnore]
        public int IntPrice => int.Parse(Price.ToString());

        [JsonIgnore]
        public string ImageFullUrl => Const.BaseUrl + Const.UrlVendingService + ImageUrl;

        [JsonIgnore]
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

        [JsonProperty("nutritionFacts")]
        public Dictionary<string,string> NutritionFacts { get; set; } 

        public bool Equals(Product other)
        {
            return Id.Equals(other.Id);
        }
    }
}
