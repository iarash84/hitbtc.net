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
        public bool IsAuthorized { get; set; }

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
        /// Sends one JSON-RPC request. Operations are serialized because receives on one socket cannot safely run concurrently.
        /// </summary>
        public async Task<ApiResponse> Execute(string request, bool requireAuthentication,
            CancellationToken cancellationToken)
        {
            ThrowIfDisposed();
            if (string.IsNullOrWhiteSpace(request))
                throw new ArgumentException("The JSON-RPC request cannot be empty.", nameof(request));
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
                return new ApiResponse { Content = await Receive(socket, cancellationToken).ConfigureAwait(false) };
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
            var response = JObject.Parse(await Receive(socket, cancellationToken).ConfigureAwait(false));
            if (response["error"] != null || response["result"] == null)
                throw new InvalidOperationException("HitBTC WebSocket authentication failed.");
            _connectionAuthenticated = true;
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
