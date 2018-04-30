using Newtonsoft.Json;

namespace Hitbtc.HitBtcModel
{
    public class Symbol
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("baseCurrency")]
        public string BaseCurrency { get; set; }
        [JsonProperty("quoteCurrency")]
        public string QuoteCurrency { get; set; }
        [JsonProperty("quantityIncrement")]
        public string QuantityIncrement { get; set; }
        [JsonProperty("tickSize")]
        public string TickSize { get; set; }
        [JsonProperty("takeLiquidityRate")]
        public string TakeLiquidityRate { get; set; }
        [JsonProperty("provideLiquidityRate")]
        public string ProvideLiquidityRate { get; set; }
        [JsonProperty("feeCurrency")]
        public string FeeCurrency { get; set; }
    }
}
