using System.Collections.Generic;
using Newtonsoft.Json;

namespace Softjourn.SJCoins.Core.Models.TransactionReports
{
    public sealed class Report
    {
        [JsonProperty("totalElements")]
        public int TotalElements { get; set; }

        [JsonProperty("last")]
        public bool Last { get; set; }

        [JsonProperty("first")]
        public bool First { get; set; }

        [JsonProperty("totalPages")]
        public int TotalPages { get; set; }

        [JsonProperty("numberOfElements")]
        public int NumberOfElements { get; set; }

        [JsonProperty("size")]
        public int Size { get; set; }

        [JsonProperty("number")]
        public int Number { get; set; }

        [JsonProperty("sort")]
        public List<Sort> Sort { get; set; }

        [JsonProperty("content")]
        public List<Transaction> Content { get; set; }
    }
}
