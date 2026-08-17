using Newtonsoft.Json;

namespace Hitbtc.HitBtcModel
{
    public class AddressModel

    {
        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("payment_id")]
        public string PaymentId { get; set; }
        [JsonProperty("currency")]
        public string Currency { get; set; }
        [JsonProperty("network_code")]
        public string NetworkCode { get; set; }
        [JsonProperty("public_key")]
        public string PublicKey { get; set; }
    }
}
