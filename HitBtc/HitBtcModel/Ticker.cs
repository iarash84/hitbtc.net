using System.Text;
using Newtonsoft.Json;

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
        [JsonProperty("volumeQuote")]
        public string VolumeQuote { get; set; }

        /// <summary>
        /// Server time in UNIX timestamp format
        /// </summary>
        [JsonProperty("timestamp")]
        public string Timestamp { get; set; }


        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(string.Format("ask:{0}", Ask));
            sb.AppendLine(string.Format("bid:{0}", Bid));
            sb.AppendLine(string.Format("last:{0}",Last));
            sb.AppendLine(string.Format("open:{0}", Open));
            sb.AppendLine(string.Format("low:{0}", Low));
            sb.AppendLine(string.Format("high:{0}", High));
            sb.AppendLine(string.Format("volume:{0}", Volume));
            sb.AppendLine(string.Format("volume_quote:{0}", VolumeQuote));
            sb.AppendLine(string.Format("timestamp:{0}", Timestamp));
            sb.AppendLine(string.Format("symbol:{0}", Symbol));

            return sb.ToString();
        }
    }
}
