using Newtonsoft.Json;

namespace Hitbtc.HitBtcModel
{
    public class Fee
    {
        [JsonProperty("takeLiquidityRate")]
        public string TakeLiquidityRate { get; set; }

        [JsonProperty("provideLiquidityRate")]
        public string ProvideLiquidityRate { get; set; }

    }
}
