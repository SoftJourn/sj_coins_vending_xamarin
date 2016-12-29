using System.Collections.Generic;

using Newtonsoft.Json;

namespace Softjourn.SJCoins.Core.API.Model.Products
{
    class Featured
    {
        [JsonProperty("lastAdded")]
        public List<int> LastAdded { get; set;}

        [JsonProperty("bestSellers")]
        public List<int> BestSellers { get; set; }

        [JsonProperty("categories")]
        public List<Categories> Categories { get; set; }
    }
}