using System.Collections.Generic;

namespace Hitbtc.HitBtcModel
{

    public class SocketTrades
    {
        public string jsonrpc { get; set; }
        public SocketTradeData result { get; set; }
        public string id { get; set; }
    }

    public class SocketTradeData
    {
        public List<SocketTrade> data { get; set; }
        public string symbol { get; set; }
    }

    public class SocketTrade
    {
        public int id { get; set; }
        public string price { get; set; }
        public string quantity { get; set; }
        public PublicEnum.EnTradingSide side { get; set; }
        public string timestamp { get; set; }
    }
}