using Newtonsoft.Json;

namespace Softjourn.SJCoins.Core.API.Model.Machines
{
    public class Size
    {
        [JsonProperty("rows")]
        public int Rows { get; set; }

        [JsonProperty("columns")]
        public int Columns { get; set; }
    }
}