using Newtonsoft.Json;

namespace Softjourn.SJCoins.Core.API.Model.Machines
{
    public sealed class Size
    {
        [JsonProperty("rows")]
        public int Rows { get; set; }

        [JsonProperty("columns")]
        public int Columns { get; set; }
    }
}