using Newtonsoft.Json;

namespace Hitbtc.HitBtcModel
{
    public class Transaction
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("created_at")]
        public string CreatedAt { get; set; }
        [JsonProperty("updated_at")]
        public string UpdatedAt { get; set; }
        [JsonProperty("last_activity_at")]
        public string LastActivityAt { get; set; }
        [JsonProperty("status")]
        public string Status { get; set; }
        [JsonProperty("type")]
        public string Type { get; set; }
        [JsonProperty("subtype")]
        public string Subtype { get; set; }
        [JsonProperty("native")]
        public TransactionNative Native { get; set; }
    }

    public class TransactionNative
    {
        [JsonProperty("tx_id")]
        public string TransactionId { get; set; }
        [JsonProperty("index")]
        public long? Index { get; set; }
        [JsonProperty("currency")]
        public string Currency { get; set; }
        [JsonProperty("amount")]
        public string Amount { get; set; }
        [JsonProperty("fee")]
        public string Fee { get; set; }
        [JsonProperty("address")]
        public string Address { get; set; }
        [JsonProperty("payment_id")]
        public string PaymentId { get; set; }
        [JsonProperty("hash")]
        public string Hash { get; set; }
        [JsonProperty("network_code")]
        public string NetworkCode { get; set; }
        [JsonProperty("protocol_code")]
        public string ProtocolCode { get; set; }
    }
}
