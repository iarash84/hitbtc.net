using Newtonsoft.Json;

namespace Hitbtc.HitBtcModel
{
    public class Trade
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("orderId")]
        public int OrderId { get; set; }
        [JsonProperty("symbol")]
        public string Symbol { get; set; }
        [JsonProperty("side")]
        public string Side { get; set; }
        [JsonProperty("quantity")]
        public string Quantity { get; set; }
        [JsonProperty("price")]
        public string Price { get; set; }
        [JsonProperty("fee")]
        public string Fee { get; set; }
        [JsonProperty("timestamp")]
        public string Timestamp { get; set; }
    }

    public class TradeHistory
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("clientOrderId")]
        public int ClientOrderId { get; set; }
        [JsonProperty("orderId")]
        public int OrderId { get; set; }
        [JsonProperty("symbol")]
        public string Symbol { get; set; }
        [JsonProperty("side")]
        public PublicEnum.EnTradingSide Side { get; set; }
        [JsonProperty("quantity")]
        public string Quantity { get; set; }
        [JsonProperty("price")]
        public string Price { get; set; }
        [JsonProperty("fee")]
        public string Fee { get; set; }
        [JsonProperty("timestamp")]
        public string Timestamp { get; set; }
    }
}

