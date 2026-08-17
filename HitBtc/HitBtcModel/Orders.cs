using Newtonsoft.Json;

namespace Hitbtc.HitBtcModel
{
    public class Order
    {
        /// <summary>
        /// Unique identifier for Order as assigned by exchange
        /// </summary>
        [JsonProperty("id")]
        public long Id { get; set; }

        /// <summary>
        /// String 	Unique identifier for Order as assigned by trader. Uniqueness must be guaranteed within a single trading day, including all active orders.
        /// </summary>
        [JsonProperty("client_order_id")]
        public string ClientOrderId { get; set; }

        /// <summary>
        /// Trading symbol
        /// </summary>
        [JsonProperty("symbol")]
        public string Symbol { get; set; }

        /// <summary>
        /// sell buy
        /// </summary>
        [JsonProperty("side")]
        public string Side { get; set; }

        /// <summary>
        /// new, suspended, partiallyFilled, filled, canceled, expired
        /// </summary>
        [JsonProperty("status")]
        public string Status { get; set; }

        /// <summary>
        /// limit, market, stopLimit, stopMarket
        /// </summary>
        [JsonProperty("type")]
        public string Type { get; set; }

        /// <summary>
        /// Time in force
        /// GTC - Good-Til-Canceled, IOC - Immediate-Or-Cancel, OK - Fill-Or-Kill, DAY - day
        /// </summary>
        [JsonProperty("time_in_force")]
        public string TimeInForce { get; set; }

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
        [JsonProperty("quantity_cumulative")]
        public string CumQuantity { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("created_at")]
        public string CreatedAt { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("updated_at")]
        public string UpdatedAt { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("stop_price")]
        public string StopPrice { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("expire_time")]
        public string ExpireTime { get; set; }

        [JsonProperty("price_average")]
        public string PriceAverage { get; set; }

        [JsonProperty("post_only")]
        public bool PostOnly { get; set; }
    }
}
