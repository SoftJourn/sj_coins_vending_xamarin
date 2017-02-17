using System;

using Newtonsoft.Json;

namespace Softjourn.SJCoins.Core.API.Model
{
    public class History
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("time")]
        public string Time { get; set; }

        [JsonProperty("price")]
        public int Price { get; set; }
    }
}