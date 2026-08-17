using Newtonsoft.Json;

namespace Hitbtc.HitBtcModel
{
    public class Trade
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("order_id")]
        public long OrderId { get; set; }
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
        [JsonProperty("client_order_id")]
        public string ClientOrderId { get; set; }
        [JsonProperty("order_id")]
        public long OrderId { get; set; }

        [JsonProperty("taker")]
        public bool Taker { get; set; }
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

