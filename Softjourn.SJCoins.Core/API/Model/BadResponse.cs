using Newtonsoft.Json;

namespace Softjourn.SJCoins.Core.API.Model
{
    public sealed class BadResponse
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
