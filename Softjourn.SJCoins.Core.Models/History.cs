using Newtonsoft.Json;

namespace Softjourn.SJCoins.Core.Models
{
    public sealed class History
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("time")]
        public string Time { get; set; }

        [JsonProperty("price")]
        public float Price { get; set; }

        public string PrettyTime { get; set; }
    }
}