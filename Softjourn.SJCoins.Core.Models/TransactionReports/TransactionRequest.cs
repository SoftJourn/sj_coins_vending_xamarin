using System.Collections.Generic;
using Newtonsoft.Json;

namespace Softjourn.SJCoins.Core.Models.TransactionReports
{
    public sealed class TransactionRequest
    {
        [JsonProperty("size")]
        public int Size { get; set; }

        [JsonProperty("page")]
        public int Page { get; set; }

        [JsonProperty("direction")]
        public string Direction { get; set; }

        [JsonProperty("sort")]
        public List<Sort> Sort { get; set; }
    }
}
