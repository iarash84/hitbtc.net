using Newtonsoft.Json;

namespace Hitbtc.HitBtcModel
{
    public class Transaction
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("index")]
        public string Index { get; set; }
        [JsonProperty("currency")]
        public string Currency { get; set; }
        [JsonProperty("amount")]
        public int Amount { get; set; }
        [JsonProperty("fee")]
        public int Fee { get; set; }
        [JsonProperty("address")]
        public double Address { get; set; }
        [JsonProperty("paymentId")]
        public string PaymentId { get; set; }
        [JsonProperty("hash")]
        public double Hash { get; set; }
        [JsonProperty("status")]
        public string Status { get; set; }
        [JsonProperty("type")]
        public object Type { get; set; }
        [JsonProperty("createdAt")]
        public int CreatedAt { get; set; }
        [JsonProperty("updatedAt")]
        public string UpdatedAt { get; set; }
    }

}
