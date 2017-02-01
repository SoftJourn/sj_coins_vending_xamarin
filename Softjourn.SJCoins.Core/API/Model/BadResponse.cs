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
        public string Title { get; set; }

        [JsonProperty("detail")]
        public string Detail { get; set; }

        [JsonProperty("code")]
        public int Code { get; set; }

        [JsonProperty("developerMessage")]
        public string DeveloperMessage { get; set; }
    }
}
