namespace Hitbtc.HitBtcModel
{
    public class Symbol
    {
        public string id { get; set; }
        public string baseCurrency { get; set; }
        public string quoteCurrency { get; set; }
        public string quantityIncrement { get; set; }
        public string tickSize { get; set; }
        public string takeLiquidityRate { get; set; }
        public string provideLiquidityRate { get; set; }
        public string feeCurrency { get; set; }
    }
}
