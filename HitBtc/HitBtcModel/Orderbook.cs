using System.Collections.Generic;
using Newtonsoft.Json;

namespace Hitbtc.HitBtcModel
{
    public class Orderbook
    {
        [JsonProperty("ask")]
        public List<OrderBookParamter> Ask { get; set; }
        [JsonProperty("bid")]
        public List<OrderBookParamter> Bid { get; set; }
        [JsonProperty("timestamp")]
        public string Timestamp { get; set; }
    }

    public class OrderBookParamter
    {
        [JsonProperty("price")]
        public string Price { set; get; }
        [JsonProperty("size")]
        public string Size { set; get; }
    }
}
