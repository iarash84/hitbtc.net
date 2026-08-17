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
        private const string Endpoint = "wss://api.hitbtc.com/api/2/ws";
        private const int ReceiveBufferSize = 8192;
        private const int MaximumMessageSize = 1024 * 1024;
        private readonly IWebSocketClient _clientWebSocket;
        private readonly SemaphoreSlim _operationLock = new SemaphoreSlim(1, 1);
        private string _apiKey;
        private string _secretKey;
        private bool _connectionAuthenticated;
        private bool _disposed;

        public SocketMarketData MarketData { get; set; }
        public SocketTrading Trading { get; set; }
        public bool IsAuthorized { get; set; }

        public HitBtcSocketApi() : this(new WebSocketClientAdapter()) { }

        internal HitBtcSocketApi(IWebSocketClient clientWebSocket)
        {
            _clientWebSocket = clientWebSocket ?? throw new ArgumentNullException(nameof(clientWebSocket));
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

            await _operationLock.WaitAsync(cancellationToken).ConfigureAwait(false);
            try
            {
                await EnsureConnected(cancellationToken).ConfigureAwait(false);
                if (requireAuthentication && !_connectionAuthenticated)
                    await Authenticate(cancellationToken).ConfigureAwait(false);

                await SendCommand(request, cancellationToken).ConfigureAwait(false);
                return new ApiResponse { Content = await Receive(cancellationToken).ConfigureAwait(false) };
            }
            finally
            {
                _operationLock.Release();
            }
        }

        private async Task EnsureConnected(CancellationToken cancellationToken)
        {
            if (_clientWebSocket.State == WebSocketState.Open)
                return;
            if (_clientWebSocket.State != WebSocketState.None)
                throw new WebSocketException("The WebSocket is not connectable: " + _clientWebSocket.State);
            await _clientWebSocket.ConnectAsync(new Uri(Endpoint), cancellationToken).ConfigureAwait(false);
        }

        private async Task Authenticate(CancellationToken cancellationToken)
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

            await SendCommand(loginRequest, cancellationToken).ConfigureAwait(false);
            var response = JObject.Parse(await Receive(cancellationToken).ConfigureAwait(false));
            if (response["error"] != null || response["result"] == null)
                throw new InvalidOperationException("HitBTC WebSocket authentication failed.");
            _connectionAuthenticated = true;
        }

        private Task SendCommand(string jsonCommand, CancellationToken cancellationToken)
        {
            var bytes = Encoding.UTF8.GetBytes(jsonCommand);
            return _clientWebSocket.SendAsync(new ArraySegment<byte>(bytes), WebSocketMessageType.Text, true,
                cancellationToken);
        }

        private async Task<string> Receive(CancellationToken cancellationToken)
        {
            var buffer = new byte[ReceiveBufferSize];
            using (var message = new MemoryStream())
            {
                while (true)
                {
                    var result = await _clientWebSocket.ReceiveAsync(new ArraySegment<byte>(buffer), cancellationToken)
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
            _clientWebSocket.Dispose();
            _operationLock.Dispose();
            _disposed = true;
        }

        private void ThrowIfDisposed()
        {
            if (_disposed) throw new ObjectDisposedException(nameof(HitBtcSocketApi));
        }
    }
}
