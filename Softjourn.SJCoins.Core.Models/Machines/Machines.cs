using Newtonsoft.Json;

namespace Softjourn.SJCoins.Core.Models.Machines
{
    public sealed class Machines
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("size")]
        public Size Size { get; set; }
    }
}