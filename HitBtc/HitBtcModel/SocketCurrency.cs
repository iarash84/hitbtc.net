using System.Collections.Generic;

namespace Hitbtc.HitBtcModel
{
    public class SocketCurrency
    {
        public string jsonrpc { get; set; }
        public Currency result { get; set; }
        public string id { get; set; }
    }
    public class SocketCurrencies
    {
        public string jsonrpc { get; set; }
        public List<Currency> result { get; set; }
        public string id { get; set; }
    }
    
}
