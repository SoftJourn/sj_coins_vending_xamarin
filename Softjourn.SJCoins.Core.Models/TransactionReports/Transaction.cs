using Newtonsoft.Json;

namespace Softjourn.SJCoins.Core.Models.TransactionReports
{
    public sealed class Transaction
    {
        [JsonProperty ("id")]
        public int Id { get; set; }

        [JsonProperty("account")]
        public string Account { get; set; }

        [JsonProperty("destination")]
        public string Destination { get; set; }

        [JsonProperty("amount")]
        public int? Amount { get; set ; }

        [JsonProperty("comment")]
        public string Comment { get; set; }

        [JsonProperty("created")]
        public string Created { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("error")]
        public object Error { get; set; }

        public string PrettyTime { get; set; }
    }
}
