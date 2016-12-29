
using Newtonsoft.Json;

namespace Softjourn.SJCoins.Core.API.Model.Machines
{
    public class Machines
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("size")]
        public Size Size { get; set; }
    }
}