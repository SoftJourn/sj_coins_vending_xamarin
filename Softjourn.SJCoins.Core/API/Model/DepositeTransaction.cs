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

        [JsonProperty("account")]
        public string Account { get; set; }

        [JsonProperty("destination")]
        public Object Destination { get; set; }

        [JsonProperty("amount")]
        public Object Amount { get; set; }

        [JsonProperty("comment")]
        public string Comment { get; set; }

        [JsonProperty("created")]
        public string Created { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("remain")]
        public int Remain { get; set; }

        [JsonProperty("error")]
        public Object Error { get; set; }
    }
}
