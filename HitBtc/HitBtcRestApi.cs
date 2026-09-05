using System;
using System.Threading.Tasks;
using System.Threading;
using Hitbtc.HitBtcCategories;
using RestSharp;
using RestSharp.Authenticators;
using Newtonsoft.Json.Linq;
using System.Globalization;

namespace Hitbtc
{
    
//    200 OK Successful request
//    400 Bad Request. Returns JSON with the error message
//    401 Unauthorized. Authorisation required or failed
//    403 Forbidden. Action is forbidden for API key
//    429 Too Many Requests. Your connection is being rate limited
//    500 Internal Server. Internal Server Error
//    503 Service Unavailable. Service is down for maintenance
//    504 Gateway Timeout. Request timeout expired

    /// <summary>
    /// HitBTC API v3 client. See https://api.hitbtc.com/api/3/explore/.
    /// </summary>
    public class HitBtcRestApi : IDisposable
    {
        private const string Url = "https://api.hitbtc.com";
        private string? _apiKey;
        private string? _secretKey;
        private readonly IRestTransport _transport;
        private bool _disposed;

        public RestTrading Trading { get; set; }
        public RestAccount Account { get; set; }
        public RestPublicData PublicData { get; set; }
        public RestTradingHistory TradingHistory { get; set; }

        public HitBtcRestApi() : this(new RestSharpTransport())
        {
        }

        internal HitBtcRestApi(IRestTransport transport)
        {
            _transport = transport ?? throw new ArgumentNullException(nameof(transport));
            PublicData = new RestPublicData(this);
            TradingHistory = new RestTradingHistory(this);
            Trading = new RestTrading(this);
            Account = new RestAccount(this);
        }

        public virtual Task<ApiResponse> Execute(RestRequest request, bool requireAuthentication = true)
        {
            return ExecuteCore(request, requireAuthentication, CancellationToken.None);
        }

        /// <summary>Executes a REST request with cancellation support.</summary>
        public virtual Task<ApiResponse> Execute(RestRequest request, bool requireAuthentication,
            CancellationToken cancellationToken)
        {
            return ExecuteCore(request, requireAuthentication, cancellationToken);
        }

        private async Task<ApiResponse> ExecuteCore(RestRequest request, bool requireAuthentication,
            CancellationToken cancellationToken)
        {
            ThrowIfDisposed();
#if NET8_0_OR_GREATER
            ArgumentNullException.ThrowIfNull(request);
#else
            if (request == null)
                throw new ArgumentNullException(nameof(request));
#endif
            if (requireAuthentication && !IsAuthorized)
                throw new InvalidOperationException("The request requires authorization. Call Authorize first.");

            var options = new RestClientOptions(Url);

            if (requireAuthentication)
                options.Authenticator = new HttpBasicAuthenticator(_apiKey!, _secretKey!);

            var response = await _transport.ExecuteAsync(request, options, cancellationToken)
                .ConfigureAwait(false);

            if (response.ErrorException != null || !response.IsSuccessful)
            {
                var apiError = TryReadApiError(response.Content);
                var message = apiError == null
                    ? string.Format(CultureInfo.InvariantCulture,
                        "HitBTC request failed with HTTP status {0} ({1}).",
                        (int)response.StatusCode, response.StatusDescription)
                    : string.Format(CultureInfo.InvariantCulture, "HitBTC request failed: {0}",
                        apiError.Value.Message);
                throw new HitBtcApiException(message, response.StatusCode,
                    apiError?.Code, response.ErrorException);
            }

            return new ApiResponse { Content = response.Content };
        }

        /// <summary>
        /// Flag shows that user is authorized
        /// </summary>
        public bool IsAuthorized { get; private set; }

        private static (string? Code, string Message)? TryReadApiError(string? content)
        {
            if (content == null || string.IsNullOrWhiteSpace(content)) return null;
            try
            {
                var root = JObject.Parse(content);
                var error = root["error"] as JObject ?? root;
                var message = error.Value<string>("message") ?? error.Value<string>("description");
                if (message == null || string.IsNullOrWhiteSpace(message)) return null;
                return (error.Value<string>("code"), message);
            }
            catch (Newtonsoft.Json.JsonException)
            {
                return null;
            }
        }

        /// <summary>
        /// Method for authorization 
        /// </summary>
        /// <param name="apiKey">API key from the Settings page.</param>
        /// <param name="secretKey">Secret key from the Settings page.</param>
        public void Authorize(string apiKey, string secretKey)
        {
            ThrowIfDisposed();
            if (string.IsNullOrWhiteSpace(apiKey))
                throw new ArgumentException("API key cannot be empty.", nameof(apiKey));
            if (string.IsNullOrWhiteSpace(secretKey))
                throw new ArgumentException("Secret key cannot be empty.", nameof(secretKey));
            _apiKey = apiKey;
            _secretKey = secretKey;
            _transport.ResetAuthenticatedClient();
            IsAuthorized = true;
        }

        public void Dispose()
        {
            if (_disposed) return;
            _transport.Dispose();
            _apiKey = null;
            _secretKey = null;
            IsAuthorized = false;
            _disposed = true;
            GC.SuppressFinalize(this);
        }

        private void ThrowIfDisposed()
        {
#if NET8_0_OR_GREATER
            ObjectDisposedException.ThrowIf(_disposed, this);
#else
            if (_disposed) throw new ObjectDisposedException(nameof(HitBtcRestApi));
#endif
        }

    }
}
