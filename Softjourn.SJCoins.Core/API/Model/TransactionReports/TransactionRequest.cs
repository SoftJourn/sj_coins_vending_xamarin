﻿
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Softjourn.SJCoins.Core.API.Model.TransactionReports
{
    public class TransactionRequest
    {
        [JsonProperty("size")]
        public int Size { get; set; }

        [JsonProperty("page")]
        public int Page { get; set; }

        [JsonProperty("sort")]
        public List<Sort> Sort { get; set; }
    }
}
