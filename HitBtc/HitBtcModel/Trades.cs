namespace Hitbtc.HitBtcModel
{
    public class Trade
    {
        public int id { get; set; }
        public int orderId { get; set; }
        public string symbol { get; set; }
        public string side { get; set; }
        public string quantity { get; set; }
        public string price { get; set; }
        public string fee { get; set; }
        public string timestamp { get; set; }
    }

    public class TradeHistory
    {
        public int id { get; set; }
        public int clientOrderId { get; set; }
        public int orderId { get; set; }
        public string symbol { get; set; }
        public PublicEnum.EnTradingSide side { get; set; }
        public string quantity { get; set; }
        public string price { get; set; }
        public string fee { get; set; }
        public string timestamp { get; set; }
    }
}

