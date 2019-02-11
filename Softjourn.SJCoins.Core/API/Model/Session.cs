using Newtonsoft.Json;

namespace Softjourn.SJCoins.Core.API.Model
{
    public sealed class Session
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }

        [JsonProperty("expires_in")]
        public string ExpreIn { get; set; }
    }
}
