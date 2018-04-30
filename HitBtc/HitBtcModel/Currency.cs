using Newtonsoft.Json;

namespace Hitbtc.HitBtcModel
{
    public class Currency
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("fullName")]
        public string FullName { get; set; }

        [JsonProperty("crypto")]
        public bool Crypto { get; set; }
        //True for cryptocurrencies, false for fiat, ICO and others.

        [JsonProperty("payinEnabled")]
        public bool PayinEnabled { get; set; }
        //True if cryptocurrency support generate adress or paymentId for deposits

        [JsonProperty("payinPaymentId")]
        public bool PayinPaymentId { get; set; }
        // True if cryptocurrency requred use paymentId for deposits

        [JsonProperty("payinConfirmations")]
        public int PayinConfirmations { get; set; }
        //Confirmations count for cryptocurrency deposits

        [JsonProperty("payoutEnabled")]
        public bool PayoutEnabled { get; set; }

        [JsonProperty("payoutFee")]
        public string PayoutFee { get; set; }

        [JsonProperty("payoutIsPaymentId")]
        public bool PayoutIsPaymentId { get; set; }

        [JsonProperty("delisted")]
        public bool Delisted { get; set; }

        [JsonProperty("transferEnabled")]
        public bool TransferEnabled { get; set; }
    }
}
