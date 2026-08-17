using Newtonsoft.Json;

#nullable disable
namespace Hitbtc.HitBtcModel
{
    public class Fee
    {
        [JsonProperty("take_rate")]
        public string TakeLiquidityRate { get; set; }

        [JsonProperty("make_rate")]
        public string ProvideLiquidityRate { get; set; }

    }
}
