using System.Collections.Generic;
using Newtonsoft.Json;

namespace Hitbtc.HitBtcModel
{

    public class SocketSymbol
    {
        [JsonProperty("jsonrpc")]
        public string Jsonrpc { get; set; }
        [JsonProperty("result")]
        public Symbol Result { get; set; }
        [JsonProperty("id")]
        public string Id { get; set; }
    }

    public class SocketSymbols
    {
        [JsonProperty("jsonrpc")]
        public string Jsonrpc { get; set; }
        [JsonProperty("result")]
        public List<Symbol> Result { get; set; }
        [JsonProperty("id")]
        public string Id { get; set; }
    }
}

