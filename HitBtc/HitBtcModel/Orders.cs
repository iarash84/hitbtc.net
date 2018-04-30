using Newtonsoft.Json;

namespace Hitbtc.HitBtcModel
{
    public class Order
    {
        /// <summary>
        /// Unique identifier for Order as assigned by exchange
        /// </summary>
        [JsonProperty("id")]
        public int Id { get; set; }

        /// <summary>
        /// String 	Unique identifier for Order as assigned by trader. Uniqueness must be guaranteed within a single trading day, including all active orders.
        /// </summary>
        [JsonProperty("clientOrderId")]
        public string ClientOrderId { get; set; }

        /// <summary>
        /// Trading symbol
        /// </summary>
        [JsonProperty("symbol")]
        public long Symbol { get; set; }

        /// <summary>
        /// sell buy
        /// </summary>
        [JsonProperty("side")]
        public string Side { get; set; }

        /// <summary>
        /// new, suspended, partiallyFilled, filled, canceled, expired
        /// </summary>
        [JsonProperty("status")]
        public int Status { get; set; }

        /// <summary>
        /// limit, market, stopLimit, stopMarket
        /// </summary>
        [JsonProperty("type")]
        public string Type { get; set; }

        /// <summary>
        /// Time in force
        /// GTC - Good-Til-Canceled, IOC - Immediate-Or-Cancel, OK - Fill-Or-Kill, DAY - day
        /// </summary>
        [JsonProperty("timeInForce")]
        public int TimeInForce { get; set; }

        /// <summary>
        /// Number 	Order quantity
        /// </summary>
        [JsonProperty("quantity")]
        public string Quantity { get; set; }

        /// <summary>
        /// Order price
        /// </summary>
        [JsonProperty("price")]
        public string Price { get; set; }

        /// <summary>
        /// Cumulative executed quantity
        /// </summary>
        [JsonProperty("cumQuantity")]
        public int CumQuantity { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("createdAt")]
        public string CreatedAt { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("updatedAt")]
        public string UpdatedAt { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("stopPrice")]
        public string StopPrice { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("expireTime")]
        public int ExpireTime { get; set; }
    }
}
