using Newtonsoft.Json;

namespace Softjourn.SJCoins.Core.API.Model
{
    public sealed class History
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("time")]
        public string Time { get; set; }

        [JsonProperty("price")]
        public float Price { get; set; }

        public int IntPrice => int.Parse(Price.ToString());

        public string PrettyTime { get; set; }
    }
}