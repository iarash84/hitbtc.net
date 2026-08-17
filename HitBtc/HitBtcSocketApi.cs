using System;
using System.IO;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Hitbtc.HitBtcCategories;
using Newtonsoft.Json.Linq;

namespace Hitbtc
{
    /// <summary>Provides JSON-RPC access to the HitBTC WebSocket API.</summary>
    public class HitBtcSocketApi : IDisposable
    {
        private const string PublicEndpoint = "wss://api.hitbtc.com/api/3/ws/public";
        private const string TradingEndpoint = "wss://api.hitbtc.com/api/3/ws/trading";
        private const int ReceiveBufferSize = 8192;
        private const int MaximumMessageSize = 1024 * 1024;
        private readonly IWebSocketClient _publicSocket;
        private readonly IWebSocketClient _tradingSocket;
        private readonly SemaphoreSlim _publicLock = new SemaphoreSlim(1, 1);
        private readonly SemaphoreSlim _tradingLock = new SemaphoreSlim(1, 1);
        private string _apiKey;
        private string _secretKey;
        private bool _connectionAuthenticated;
        private bool _disposed;

        public SocketMarketData MarketData { get; set; }
        public SocketTrading Trading { get; set; }
        public bool IsAuthorized { get; private set; }

        /// <summary>Raised for every message read by <see cref="ListenForNotificationsAsync"/>.</summary>
        public event EventHandler<HitBtcNotificationEventArgs> NotificationReceived;

        public HitBtcSocketApi() : this(new WebSocketClientAdapter(), new WebSocketClientAdapter()) { }

        internal HitBtcSocketApi(IWebSocketClient clientWebSocket) : this(clientWebSocket, clientWebSocket) { }

        internal HitBtcSocketApi(IWebSocketClient publicSocket, IWebSocketClient tradingSocket)
        {
            _publicSocket = publicSocket ?? throw new ArgumentNullException(nameof(publicSocket));
            _tradingSocket = tradingSocket ?? throw new ArgumentNullException(nameof(tradingSocket));
            MarketData = new SocketMarketData(this);
            Trading = new SocketTrading(this);
        }

        public Task<ApiResponse> Execute(string request, bool requireAuthentication = true) =>
            Execute(request, requireAuthentication, CancellationToken.None);

        /// <summary>
        /// Continuously receives notifications on an already subscribed connection until cancellation.
        /// Commands and this listener are serialized per connection to prevent concurrent receives.
        /// </summary>
        /// <param name="requireAuthentication">Use the authenticated trading connection when true.</param>
        /// <param name="cancellationToken">Stops the receive loop without closing the client.</param>
        public async Task ListenForNotificationsAsync(bool requireAuthentication,
            CancellationToken cancellationToken)
        {
            ThrowIfDisposed();
            if (requireAuthentication && !IsAuthorized)
                throw new InvalidOperationException("The listener requires authorization. Call Authorize first.");

            var socket = requireAuthentication ? _tradingSocket : _publicSocket;
            var operationLock = requireAuthentication ? _tradingLock : _publicLock;
            await operationLock.WaitAsync(cancellationToken).ConfigureAwait(false);
            try
            {
                await EnsureConnected(socket, requireAuthentication, cancellationToken).ConfigureAwait(false);
                if (requireAuthentication && !_connectionAuthenticated)
                    await Authenticate(socket, cancellationToken).ConfigureAwait(false);

                while (true)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    var content = await Receive(socket, cancellationToken).ConfigureAwait(false);
                    JObject message;
                    try
                    {
                        message = JObject.Parse(content);
                    }
                    catch (Newtonsoft.Json.JsonException exception)
                    {
                        throw new HitBtcWebSocketException(
                            "HitBTC returned malformed notification JSON.", null, exception);
                    }

                    if (message["error"] != null)
                        throw CreateWebSocketError(message, "HitBTC WebSocket notification failed.");

                    OnNotificationReceived(new HitBtcNotificationEventArgs(content, message));
                }
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                // Cancellation is the normal way to stop a long-running notification listener.
            }
            finally
            {
                operationLock.Release();
            }
        }

        private void OnNotificationReceived(HitBtcNotificationEventArgs eventArgs)
        {
            var handler = NotificationReceived;
            if (handler != null) handler(this, eventArgs);
        }

        /// <summary>
        /// Sends one JSON-RPC request. Operations are serialized because receives on one socket cannot safely run concurrently.
        /// </summary>
        public async Task<ApiResponse> Execute(string request, bool requireAuthentication,
            CancellationToken cancellationToken)
        {
            ThrowIfDisposed();
            if (string.IsNullOrWhiteSpace(request))
                throw new ArgumentException("The JSON-RPC request cannot be empty.", nameof(request));
            EnsureValidRequest(request);
            if (requireAuthentication && !IsAuthorized)
                throw new InvalidOperationException("The request requires authorization. Call Authorize first.");

            var socket = requireAuthentication ? _tradingSocket : _publicSocket;
            var operationLock = requireAuthentication ? _tradingLock : _publicLock;
            await operationLock.WaitAsync(cancellationToken).ConfigureAwait(false);
            try
            {
                await EnsureConnected(socket, requireAuthentication, cancellationToken).ConfigureAwait(false);
                if (requireAuthentication && !_connectionAuthenticated)
                    await Authenticate(socket, cancellationToken).ConfigureAwait(false);

                await SendCommand(socket, request, cancellationToken).ConfigureAwait(false);
                var content = await Receive(socket, cancellationToken).ConfigureAwait(false);
                ValidateResponse(request, content);
                return new ApiResponse { Content = content };
            }
            finally
            {
                operationLock.Release();
            }
        }

        private static async Task EnsureConnected(IWebSocketClient socket, bool trading,
            CancellationToken cancellationToken)
        {
            if (socket.State == WebSocketState.Open)
                return;
            if (socket.State != WebSocketState.None)
                throw new WebSocketException("The WebSocket is not connectable: " + socket.State);
            var endpoint = trading ? TradingEndpoint : PublicEndpoint;
            await socket.ConnectAsync(new Uri(endpoint), cancellationToken).ConfigureAwait(false);
        }

        private async Task Authenticate(IWebSocketClient socket, CancellationToken cancellationToken)
        {
            var loginRequest = new JObject
            {
                ["method"] = "login",
                ["params"] = new JObject
                {
                    ["algo"] = "BASIC",
                    ["pKey"] = _apiKey,
                    ["sKey"] = _secretKey
                }
            }.ToString(Newtonsoft.Json.Formatting.None);

            await SendCommand(socket, loginRequest, cancellationToken).ConfigureAwait(false);
            JObject response;
            try
            {
                response = JObject.Parse(await Receive(socket, cancellationToken).ConfigureAwait(false));
            }
            catch (Newtonsoft.Json.JsonException exception)
            {
                throw new HitBtcWebSocketException("HitBTC returned malformed authentication JSON.", null, exception);
            }
            if (response["error"] != null || response["result"] == null ||
                response["result"].Type == JTokenType.Boolean && !response["result"].Value<bool>())
                throw CreateWebSocketError(response, "HitBTC WebSocket authentication failed.");
            _connectionAuthenticated = true;
        }

        private static void ValidateResponse(string requestContent, string responseContent)
        {
            JObject response;
            try
            {
                response = JObject.Parse(responseContent);
            }
            catch (Newtonsoft.Json.JsonException exception)
            {
                throw new HitBtcWebSocketException("HitBTC returned malformed WebSocket JSON.", null, exception);
            }

            if (response["error"] != null)
                throw CreateWebSocketError(response, "HitBTC WebSocket request failed.");

            try
            {
                var request = JObject.Parse(requestContent);
                var requestId = request["id"];
                if (requestId != null && !JToken.DeepEquals(requestId, response["id"]))
                    throw new HitBtcWebSocketException("HitBTC WebSocket response ID did not match the request ID.");
            }
            catch (Newtonsoft.Json.JsonException exception)
            {
                throw new ArgumentException("The WebSocket request must contain valid JSON.",
                    nameof(requestContent), exception);
            }
        }

        private static void EnsureValidRequest(string request)
        {
            try
            {
                JObject.Parse(request);
            }
            catch (Newtonsoft.Json.JsonException exception)
            {
                throw new ArgumentException("The WebSocket request must contain valid JSON.",
                    nameof(request), exception);
            }
        }

        private static HitBtcWebSocketException CreateWebSocketError(JObject response,
            string fallbackMessage)
        {
            var error = response["error"] as JObject;
            var code = error?.Value<string>("code");
            var message = error?.Value<string>("message") ?? error?.Value<string>("description")
                ?? fallbackMessage;
            return new HitBtcWebSocketException(message, code);
        }

        private static Task SendCommand(IWebSocketClient socket, string jsonCommand,
            CancellationToken cancellationToken)
        {
            var bytes = Encoding.UTF8.GetBytes(jsonCommand);
            return socket.SendAsync(new ArraySegment<byte>(bytes), WebSocketMessageType.Text, true,
                cancellationToken);
        }

        private static async Task<string> Receive(IWebSocketClient socket, CancellationToken cancellationToken)
        {
            var buffer = new byte[ReceiveBufferSize];
            using (var message = new MemoryStream())
            {
                while (true)
                {
                    var result = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), cancellationToken)
                        .ConfigureAwait(false);
                    if (result.MessageType == WebSocketMessageType.Close)
                        throw new WebSocketException("The server closed the WebSocket connection.");
                    if (result.MessageType != WebSocketMessageType.Text)
                        throw new WebSocketException("Only text WebSocket messages are supported.");

                    message.Write(buffer, 0, result.Count);
                    if (message.Length > MaximumMessageSize)
                        throw new WebSocketException("The WebSocket message exceeded the 1 MiB safety limit.");
                    if (result.EndOfMessage)
                        break;
                }
                return Encoding.UTF8.GetString(message.ToArray());
            }
        }

        public void Authorize(string apiKey, string secretKey)
        {
            if (string.IsNullOrWhiteSpace(apiKey))
                throw new ArgumentException("API key cannot be empty.", nameof(apiKey));
            if (string.IsNullOrWhiteSpace(secretKey))
                throw new ArgumentException("Secret key cannot be empty.", nameof(secretKey));
            _apiKey = apiKey;
            _secretKey = secretKey;
            IsAuthorized = true;
            _connectionAuthenticated = false;
        }

        public void Dispose()
        {
            if (_disposed) return;
            _publicSocket.Dispose();
            if (!ReferenceEquals(_publicSocket, _tradingSocket)) _tradingSocket.Dispose();
            _publicLock.Dispose();
            _tradingLock.Dispose();
            _disposed = true;
        }

        private void ThrowIfDisposed()
        {
            if (_disposed) throw new ObjectDisposedException(nameof(HitBtcSocketApi));
        }
    }
}
