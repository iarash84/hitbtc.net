using System.Text;

namespace Hitbtc.HitBtcModel
{
    public class Ticker
    {
        public string symbol { get; set; }
        /// <summary>
        /// Last price
        /// </summary>
        public string last { get; set; }

        /// <summary>
        /// Highest buy order
        /// </summary>
        public string bid { get; set; }

        /// <summary>
        /// Lowest sell order
        /// </summary>
        public string ask { get; set; }

        /// <summary>
        /// Highest trade price per last 24h + last incomplete minute
        /// </summary>
        public string high { get; set; }

        /// <summary>
        /// Lowest trade price per last 24h + last incomplete minute
        /// </summary>
        public string low { get; set; }

        /// <summary>
        /// Volume per last 24h + last incomplete minute
        /// </summary>
        public string volume { get; set; }

        /// <summary>
        /// Price in which instrument open
        /// </summary>
        public string open { get; set; }

        /// <summary>
        /// Volume in second currency per last 24h + last incomplete minute
        /// </summary>
        public string volumeQuoute { get; set; }

        /// <summary>
        /// Server time in UNIX timestamp format
        /// </summary>
        public string timestamp { get; set; }


        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(string.Format("ask:{0}", ask));
            sb.AppendLine(string.Format("bid:{0}", bid));
            sb.AppendLine(string.Format("last:{0}",last));
            sb.AppendLine(string.Format("open:{0}", open));
            sb.AppendLine(string.Format("low:{0}", low));
            sb.AppendLine(string.Format("high:{0}", high));
            sb.AppendLine(string.Format("volume:{0}", volume));
            sb.AppendLine(string.Format("volume_quote:{0}", volumeQuoute));
            sb.AppendLine(string.Format("timestamp:{0}", timestamp));
            sb.AppendLine(string.Format("symbol:{0}", symbol));

            return sb.ToString();
        }
    }
}