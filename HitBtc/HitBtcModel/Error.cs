namespace Hitbtc.HitBtcModel
{
    public class Error
    {
        public string code { get; set; }
        public int message { get; set; }
        public string description { get; set; }
        public override string ToString()
        {
            return string.Format("{0} - {1} - {2}", code, message, description);
        }
    }
}
