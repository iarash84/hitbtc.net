using Newtonsoft.Json;

namespace Hitbtc.HitBtcModel
{
    public class Candle
    {
        [JsonProperty("open")]
        public string Open { get; set; }
        [JsonProperty("close")]
        public string Close { get; set; }
        [JsonProperty("min")]
        public string Min { get; set; }
        [JsonProperty("max")]
        public string Max { get; set; }
        [JsonProperty("volume")]
        public string Volume { get; set; }
        [JsonProperty("volumeQuote")]
        public string VolumeQuote { get; set; }
        [JsonProperty("timestamp")]
        public string Timestamp { get; set; }
    }
}
