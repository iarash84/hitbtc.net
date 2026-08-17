using System;
using Newtonsoft.Json.Linq;

namespace Hitbtc
{
    /// <summary>Contains one notification received from a HitBTC WebSocket channel.</summary>
    public sealed class HitBtcNotificationEventArgs : EventArgs
    {
        internal HitBtcNotificationEventArgs(string rawJson, JObject message)
        {
            RawJson = rawJson;
            Channel = message.Value<string>("ch");
            Method = message.Value<string>("method");
            Data = message["data"] ?? message["params"];
        }

        /// <summary>Gets the channel name supplied in the <c>ch</c> property.</summary>
        public string Channel { get; }

        /// <summary>Gets the protocol method when the message supplies one.</summary>
        public string Method { get; }

        /// <summary>Gets the notification payload without forcing a domain model.</summary>
        public JToken Data { get; }

        /// <summary>Gets the complete JSON message exactly as received.</summary>
        public string RawJson { get; }
    }
}
