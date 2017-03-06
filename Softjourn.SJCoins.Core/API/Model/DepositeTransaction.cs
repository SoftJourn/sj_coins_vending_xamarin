using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Softjourn.SJCoins.Core.API.Model
{
    public class DepositeTransaction
    {

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("account", NullValueHandling = NullValueHandling.Ignore)]
        public string Account { get; set; }

        [JsonProperty("destination")]
        public string Destination { get; set; }

        [JsonProperty("amount", NullValueHandling = NullValueHandling.Ignore)]
        public object Amount { get; set; }

        [JsonProperty("comment")]
        public string Comment { get; set; }

        [JsonProperty("created")]
        public string Created { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("remain", NullValueHandling = NullValueHandling.Ignore)]
        public int Remain { get; set; }

        [JsonProperty("error", NullValueHandling = NullValueHandling.Ignore)]
        public object Error { get; set; }

        [JsonProperty("transactionStoring", NullValueHandling = NullValueHandling.Ignore)]
        public object TransactionStoring { get; set; }
    }
}
