using System.Collections.Generic;

namespace Hitbtc.HitBtcModel
{
    public class SocketOrder
    {
        public string jsonrpc { get; set; }
        public SocketOrederResult result { get; set; }
        public string id { get; set; }
    }

    public class SocketOrderReplace
    {
        public string jsonrpc { get; set; }
        public SocketOrderReplaceResult result { get; set; }
        public string id { get; set; }
    }

    public class SocketOrederResult
    {
        public string id { get; set; }
        public string clientOrderId { get; set; }
        public string symbol { get; set; }
        public string side { get; set; }
        public string status { get; set; }
        public string type { get; set; }
        public string timeInForce { get; set; }
        public string quantity { get; set; }
        public string price { get; set; }
        public string cumQuantity { get; set; }
        public string createdAt { get; set; }
        public string updatedAt { get; set; }
        public string reportType { get; set; }
    }

    public class SocketOrderReplaceResult
    {
        public string id { get; set; }
        public string clientOrderId { get; set; }
        public string symbol { get; set; }
        public string side { get; set; }
        public string status { get; set; }
        public string type { get; set; }
        public string timeInForce { get; set; }
        public string quantity { get; set; }
        public string price { get; set; }
        public string cumQuantity { get; set; }
        public string createdAt { get; set; }
        public string updatedAt { get; set; }
        public string reportType { get; set; }
        public string originalRequestClientOrderId { get; set; }
    }
}
