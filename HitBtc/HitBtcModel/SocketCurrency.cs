using System.Collections.Generic;
using Newtonsoft.Json;

namespace Hitbtc.HitBtcModel
{
    public class SocketCurrency
    {
        [JsonProperty("jsonrpc")]
        public string Jsonrpc { get; set; }
        [JsonProperty("result")]
        public Currency Result { get; set; }
        [JsonProperty("id")]
        public string Id { get; set; }
    }
    public class SocketCurrencies
    {
        [JsonProperty("jsonrpc")]
        public string Jsonrpc { get; set; }
        [JsonProperty("result")]
        public List<Currency> Result { get; set; }
        [JsonProperty("id")]
        public string Id { get; set; }
    }
    
}
