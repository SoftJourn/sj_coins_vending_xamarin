using System;

using Newtonsoft.Json;

namespace Softjourn.SJCoins.Core.API.Model.AccountInfo
{
    public class Account
    {
        [JsonProperty("amount")]
        public int Amount { get; set; }

        [JsonProperty("image")]
        public string Image { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("surname")]
        public string Surname { get; set; }
    }
}