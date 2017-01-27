using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Softjourn.SJCoins.Core.API.Model.Products
{
    public class Amount
    {
        [JsonProperty("amount")]
        public string Balance { get; set; }
    }
}
