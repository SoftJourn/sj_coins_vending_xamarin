using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Softjourn.SJCoins.Core.API.Model
{
    public class BadResponse
    {
        [JsonProperty("title")]
        public int Title { get; set; }

        [JsonProperty("detail")]
        public int Detail { get; set; }

        [JsonProperty("code")]
        public int Code { get; set; }

        [JsonProperty("developerMessage")]
        public int DeveloperMessage { get; set; }
    }
}
