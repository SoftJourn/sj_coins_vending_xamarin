using System;

using Newtonsoft.Json;

namespace Softjourn.SJCoins.Core.API.Model.AccountInfo
{
    class Account
    {
        [JsonProperty("amount")]
        private int Amount { get; set; }

        [JsonProperty("image")]
        public string Image { get; set; }

        [JsonProperty("name")]
        private string Name { get; set; }

        [JsonProperty("surname")]
        public String Surname { get; set; }
    }
}