using System.Collections.Generic;
using Newtonsoft.Json;

namespace Hitbtc.HitBtcModel
{
    public class SocketOrder
    {
        [JsonProperty("jsonrpc")]
        public string Jsonrpc { get; set; }
        [JsonProperty("result")]
        public SocketOrederResult Result { get; set; }
        [JsonProperty("id")]
        public string Id { get; set; }
    }

    public class SocketOrderReplace
    {
        [JsonProperty("jsonrpc")]
        public string Jsonrpc { get; set; }
        [JsonProperty("result")]
        public SocketOrderReplaceResult Result { get; set; }
        [JsonProperty("id")]
        public string Id { get; set; }
    }

    public class SocketOrederResult
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("clientOrderId")]
        public string ClientOrderId { get; set; }
        [JsonProperty("symbol")]
        public string Symbol { get; set; }
        [JsonProperty("side")]
        public string Side { get; set; }
        [JsonProperty("status")]
        public string Status { get; set; }
        [JsonProperty("type")]
        public string Type { get; set; }
        [JsonProperty("timeInForce")]
        public string TimeInForce { get; set; }
        [JsonProperty("quantity")]
        public string Quantity { get; set; }
        [JsonProperty("price")]
        public string Price { get; set; }
        [JsonProperty("cumQuantity")]
        public string CumQuantity { get; set; }
        [JsonProperty("createdAt")]
        public string CreatedAt { get; set; }
        [JsonProperty("updatedAt")]
        public string UpdatedAt { get; set; }
        [JsonProperty("reportType")]
        public string ReportType { get; set; }
    }

    public class SocketOrderReplaceResult
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("clientOrderId")]
        public string ClientOrderId { get; set; }
        [JsonProperty("symbol")]
        public string Symbol { get; set; }
        [JsonProperty("side")]
        public string Side { get; set; }
        [JsonProperty("status")]
        public string Status { get; set; }
        [JsonProperty("type")]
        public string Type { get; set; }
        [JsonProperty("timeInForce")]
        public string TimeInForce { get; set; }
        [JsonProperty("quantity")]
        public string Quantity { get; set; }
        [JsonProperty("price")]
        public string Price { get; set; }
        [JsonProperty("cumQuantity")]
        public string CumQuantity { get; set; }
        [JsonProperty("createdAt")]
        public string CreatedAt { get; set; }
        [JsonProperty("updatedAt")]
        public string UpdatedAt { get; set; }
        [JsonProperty("reportType")]
        public string ReportType { get; set; }
        [JsonProperty("originalRequestClientOrderId")]
        public string OriginalRequestClientOrderId { get; set; }
    }
}
