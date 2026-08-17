using Newtonsoft.Json;

namespace Hitbtc.HitBtcModel
{
    public class Symbol
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("base_currency")]
        public string BaseCurrency { get; set; }
        [JsonProperty("quote_currency")]
        public string QuoteCurrency { get; set; }
        [JsonProperty("quantity_increment")]
        public string QuantityIncrement { get; set; }
        [JsonProperty("tick_size")]
        public string TickSize { get; set; }
        [JsonProperty("take_rate")]
        public string TakeLiquidityRate { get; set; }
        [JsonProperty("make_rate")]
        public string ProvideLiquidityRate { get; set; }
        [JsonProperty("fee_currency")]
        public string FeeCurrency { get; set; }
        [JsonProperty("type")]
        public string Type { get; set; }
        [JsonProperty("status")]
        public string Status { get; set; }
        [JsonProperty("margin_trading")]
        public bool MarginTrading { get; set; }
        [JsonProperty("max_initial_leverage")]
        public string MaxInitialLeverage { get; set; }
    }
}
