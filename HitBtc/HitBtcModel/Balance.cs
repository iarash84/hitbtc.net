using Newtonsoft.Json;

#nullable disable
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

        [JsonProperty("reserved_margin")]
        public string ReservedMargin { get; set; }

        [JsonProperty("cross_margin_reserved")]
        public string CrossMarginReserved { get; set; }

    }
}
