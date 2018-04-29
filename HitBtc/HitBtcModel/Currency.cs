namespace Hitbtc.HitBtcModel
{
    public class Currency
    {
        public string id { get; set; }

        public string fullName { get; set; }

        public bool crypto { get; set; }
        //True for cryptocurrencies, false for fiat, ICO and others.

        public bool payinEnabled { get; set; }
        //True if cryptocurrency support generate adress or paymentId for deposits

        public bool payinPaymentId { get; set; }
        // True if cryptocurrency requred use paymentId for deposits

        public int payinConfirmations { get; set; }
        //Confirmations count for cryptocurrency deposits

        public bool payoutEnabled { get; set; }

        public string payoutFee { get; set; }

        public bool payoutIsPaymentId { get; set; }

        public bool delisted { get; set; }

        public bool transferEnabled { get; set; }
    }
}
