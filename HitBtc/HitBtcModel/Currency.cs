using Newtonsoft.Json;

#nullable disable
namespace Hitbtc.HitBtcModel
{
    public class Currency
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("full_name")]
        public string FullName { get; set; }

        [JsonProperty("crypto")]
        public bool Crypto { get; set; }

        [JsonProperty("stable")]
        public bool Stable { get; set; }
        //True for cryptocurrencies, false for fiat, ICO and others.

        [JsonProperty("payin_enabled")]
        public bool PayinEnabled { get; set; }
        //True if cryptocurrency support generate adress or paymentId for deposits

        [JsonProperty("payin_payment_id")]
        public bool PayinPaymentId { get; set; }
        // True if cryptocurrency requred use paymentId for deposits

        [JsonProperty("payin_confirmations")]
        public int PayinConfirmations { get; set; }
        //Confirmations count for cryptocurrency deposits

        [JsonProperty("payout_enabled")]
        public bool PayoutEnabled { get; set; }

        [JsonProperty("payout_fee")]
        public string PayoutFee { get; set; }

        [JsonProperty("payout_is_payment_id")]
        public bool PayoutIsPaymentId { get; set; }

        [JsonProperty("delisted")]
        public bool Delisted { get; set; }

        [JsonProperty("transfer_enabled")]
        public bool TransferEnabled { get; set; }

        [JsonProperty("precision_transfer")]
        public string PrecisionTransfer { get; set; }

        [JsonProperty("transfer_to_wallet_enabled")]
        public bool TransferToWalletEnabled { get; set; }

        [JsonProperty("transfer_to_exchange_enabled")]
        public bool TransferToExchangeEnabled { get; set; }

        [JsonProperty("networks")]
        public System.Collections.Generic.List<CurrencyNetwork> Networks { get; set; }
    }

    public class CurrencyNetwork
    {
        [JsonProperty("code")]
        public string Code { get; set; }
        [JsonProperty("network_name")]
        public string NetworkName { get; set; }
        [JsonProperty("network")]
        public string Network { get; set; }
        [JsonProperty("protocol")]
        public string Protocol { get; set; }
        [JsonProperty("default")]
        public bool Default { get; set; }
        [JsonProperty("payin_enabled")]
        public bool PayinEnabled { get; set; }
        [JsonProperty("payout_enabled")]
        public bool PayoutEnabled { get; set; }
        [JsonProperty("precision_payout")]
        public string PrecisionPayout { get; set; }
        [JsonProperty("payout_fee")]
        public string PayoutFee { get; set; }
        [JsonProperty("payout_is_payment_id")]
        public bool PayoutIsPaymentId { get; set; }
        [JsonProperty("payin_payment_id")]
        public bool PayinPaymentId { get; set; }
        [JsonProperty("payin_confirmations")]
        public int PayinConfirmations { get; set; }
    }
}
