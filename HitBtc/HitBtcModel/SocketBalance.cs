using System.Collections.Generic;

namespace Hitbtc.HitBtcModel
{
    public class SocketBalance
    {
        public string jsonrpc { get; set; }
        public List<Balance> result { get; set; }
        public string id { get; set; }
    }
}
