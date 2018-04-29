using System.Collections.Generic;

namespace Hitbtc.HitBtcModel
{

    public class SocketSymbol
    {
        public string jsonrpc { get; set; }
        public Symbol result { get; set; }
        public string id { get; set; }
    }

    public class SocketSymbols
    {
        public string jsonrpc { get; set; }
        public List<Symbol> result { get; set; }
        public string id { get; set; }
    }
}

