using System.Text;
using Newtonsoft.Json;

#nullable disable
namespace Hitbtc.HitBtcModel
{
    public class Ticker
    {
        [JsonProperty("symbol")]
        public string Symbol { get; set; }
        /// <summary>
        /// Last price
        /// </summary>
        [JsonProperty("last")]
        public string Last { get; set; }

        /// <summary>
        /// Highest buy order
        /// </summary>
        [JsonProperty("bid")]
        public string Bid { get; set; }

        /// <summary>
        /// Lowest sell order
        /// </summary>
        [JsonProperty("ask")]
        public string Ask { get; set; }

        /// <summary>
        /// Highest trade price per last 24h + last incomplete minute
        /// </summary>
        [JsonProperty("high")]
        public string High { get; set; }

        /// <summary>
        /// Lowest trade price per last 24h + last incomplete minute
        /// </summary>
        [JsonProperty("low")]
        public string Low { get; set; }

        /// <summary>
        /// Volume per last 24h + last incomplete minute
        /// </summary>
        [JsonProperty("volume")]
        public string Volume { get; set; }

        /// <summary>
        /// Price in which instrument open
        /// </summary>
        [JsonProperty("open")]
        public string Open { get; set; }

        /// <summary>
        /// Volume in second currency per last 24h + last incomplete minute
        /// </summary>
        [JsonProperty("volume_quote")]
        public string VolumeQuote { get; set; }

        /// <summary>
        /// Server time in UNIX timestamp format
        /// </summary>
        [JsonProperty("timestamp")]
        public string Timestamp { get; set; }


        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("ask:").AppendLine(Ask);
            sb.Append("bid:").AppendLine(Bid);
            sb.Append("last:").AppendLine(Last);
            sb.Append("open:").AppendLine(Open);
            sb.Append("low:").AppendLine(Low);
            sb.Append("high:").AppendLine(High);
            sb.Append("volume:").AppendLine(Volume);
            sb.Append("volume_quote:").AppendLine(VolumeQuote);
            sb.Append("timestamp:").AppendLine(Timestamp);
            sb.Append("symbol:").AppendLine(Symbol);

            return sb.ToString();
        }
    }
}
