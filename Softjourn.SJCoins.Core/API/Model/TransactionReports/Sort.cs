using Newtonsoft.Json;

namespace Softjourn.SJCoins.Core.API.Model.TransactionReports
{
    public class Sort
    {
        [JsonProperty("direction")]
        public string Direction { get; set; }

        [JsonProperty("property")]
        public string Property { get; set; }

        [JsonProperty("ignoreCase")]
        public bool IgnoreCase { get; set; }

        [JsonProperty("nullHandling")]
        public string NullHandling { get; set; }

        [JsonProperty("ascending")]
        public bool Ascending { get; set; }
    }
}
