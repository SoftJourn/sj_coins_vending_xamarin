using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Softjourn.SJCoins.Core.API.Model
{
    public class Cash
    {
        [JsonProperty("tokenContractAddress")]
        public string TokenContractAddress { get; set; }

        [JsonProperty("offlineContractAddress")]
        public string OfflineContractAddress { get; set; }

        [JsonProperty("chequeHash")]
        public string ChequeHash { get; set; }

        [JsonProperty("amount")]
        public int Amount { get; set; }
    }
}
