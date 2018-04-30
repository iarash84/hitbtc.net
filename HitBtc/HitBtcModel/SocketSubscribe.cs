using Newtonsoft.Json;

namespace Hitbtc.HitBtcModel
{
    public class SocketSubscribe
    {
        [JsonProperty("jsonrpc")]
        public string Jsonrpc { get; set; }
        [JsonProperty("result")]
        public string Result { get; set; }
        [JsonProperty("id")]
        public string Id { get; set; }
    }
}
