using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Newtonsoft.Json;
using Softjourn.SJCoins.Core.Common;

namespace Softjourn.SJCoins.Core.Models.Products
{
    public sealed class Product : IEquatable<Product>
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("price")]
        public float Price { get; set; }

        [JsonIgnore]
        public int IntPrice => int.Parse(Price.ToString(CultureInfo.InvariantCulture));

        [JsonIgnore]
        public string ImageFullUrl => $"{Constant.BaseUrl}{Constant.UrlVendingService}/{ImageUrl}";

        [JsonIgnore]
        public List<string> ImagesFullUrls => ImageUrls.Select(url => $"{Constant.BaseUrl}{Constant.UrlVendingService}/{url}").ToList();

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

        public bool Equals(Product other) => other != null && Id.Equals(other.Id);
    }
}
