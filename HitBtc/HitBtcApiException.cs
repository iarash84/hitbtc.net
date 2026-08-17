using System;
using System.Net;

namespace Hitbtc
{
    /// <summary>Represents an error returned by HitBTC or its transport.</summary>
    public sealed class HitBtcApiException : Exception
    {
        public HitBtcApiException(string message, HttpStatusCode statusCode = 0,
            string apiErrorCode = null, Exception innerException = null)
            : base(message, innerException)
        {
            StatusCode = statusCode;
            ApiErrorCode = apiErrorCode;
        }

        /// <summary>Gets the HTTP status code, or zero when no HTTP response was received.</summary>
        public HttpStatusCode StatusCode { get; }

        /// <summary>Gets the exchange-specific error code when the response supplied one.</summary>
        public string ApiErrorCode { get; }
    }

    /// <summary>Represents a malformed or error response received over WebSocket.</summary>
    public sealed class HitBtcWebSocketException : Exception
    {
        public HitBtcWebSocketException(string message, string apiErrorCode = null,
            Exception innerException = null) : base(message, innerException)
        {
            ApiErrorCode = apiErrorCode;
        }

        public string ApiErrorCode { get; }
    }
}
