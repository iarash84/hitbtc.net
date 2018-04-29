namespace Hitbtc.HitBtcModel
{
    public class Transaction
    {
        public string id { get; set; }
        public string index { get; set; }
        public string currency { get; set; }
        public int amount { get; set; }
        public int fee { get; set; }
        public double address { get; set; }
        public string paymentId { get; set; }
        public double hash { get; set; }
        public string status { get; set; }
        public object type { get; set; }
        public int createdAt { get; set; }
        public string updatedAt { get; set; }
    }

}
