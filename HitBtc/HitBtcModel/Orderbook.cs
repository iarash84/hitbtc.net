using System.Collections.Generic;

namespace Hitbtc.HitBtcModel
{
    public class Orderbook
    {
        public List<OrderBookParamter> ask { get; set; }
        public List<OrderBookParamter> bid { get; set; }
        public string timestamp { get; set; }
    }

    public class OrderBookParamter
    {
        public string price { set; get; }
        public string size { set; get; }
    }
}
