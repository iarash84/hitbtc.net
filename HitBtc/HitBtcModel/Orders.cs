namespace Hitbtc.HitBtcModel
{
    public class Order
    {
        /// <summary>
        /// Unique identifier for Order as assigned by exchange
        /// </summary>
        public int id { get; set; }

        /// <summary>
        /// String 	Unique identifier for Order as assigned by trader. Uniqueness must be guaranteed within a single trading day, including all active orders.
        /// </summary>
        public string clientOrderId { get; set; }

        /// <summary>
        /// Trading symbol
        /// </summary>
        public long symbol { get; set; }

        /// <summary>
        /// sell buy
        /// </summary>
        public string side { get; set; }

        /// <summary>
        /// new, suspended, partiallyFilled, filled, canceled, expired
        /// </summary>
        public int status { get; set; }

        /// <summary>
        /// limit, market, stopLimit, stopMarket
        /// </summary>
        public string type { get; set; }

        /// <summary>
        /// Time in force
        /// GTC - Good-Til-Canceled, IOC - Immediate-Or-Cancel, OK - Fill-Or-Kill, DAY - day
        /// </summary>
        public int timeInForce { get; set; }

        /// <summary>
        /// Number 	Order quantity
        /// </summary>
        public string quantity { get; set; }

        /// <summary>
        /// Order price
        /// </summary>
        public string price { get; set; }

        /// <summary>
        /// Cumulative executed quantity
        /// </summary>
        public int cumQuantity { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string createdAt { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string updatedAt { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string stopPrice { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int expireTime { get; set; }
    }
}
