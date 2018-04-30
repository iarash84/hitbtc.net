using Newtonsoft.Json;

namespace Hitbtc.HitBtcModel
{
    public class Balance
    {
        [JsonProperty("currency")]
        public string Currency { get; set; }

        [JsonProperty("available")]
        public string Available { get; set; }

        [JsonProperty("reserved")]
        public string Reserved { get; set; }

    }
}
