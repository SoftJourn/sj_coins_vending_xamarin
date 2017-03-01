using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Softjourn.SJCoins.Core.API.Model.TransactionReports
{
    public class Transaction
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

        public int IntAmount { get { return Amount != null ? int.Parse(Amount.ToString()) : 0; } }

        public string PrettyTime { get; set; }
    }
}
