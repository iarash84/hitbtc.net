using Newtonsoft.Json;

namespace Hitbtc.HitBtcModel
{
    public class AddressModel

    {
        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("paymentId")]
        public string PaymentId { get; set; }
    }
}
