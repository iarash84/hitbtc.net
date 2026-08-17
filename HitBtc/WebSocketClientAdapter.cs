using System;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

namespace Hitbtc
{
    internal interface IWebSocketClient : IDisposable
    {
        WebSocketState State { get; }
        Task ConnectAsync(Uri uri, CancellationToken cancellationToken);
        Task SendAsync(ArraySegment<byte> buffer, WebSocketMessageType messageType, bool endOfMessage,
            CancellationToken cancellationToken);
        Task<WebSocketReceiveResult> ReceiveAsync(ArraySegment<byte> buffer, CancellationToken cancellationToken);
    }

    internal sealed class WebSocketClientAdapter : IWebSocketClient
    {
        private readonly ClientWebSocket _client = new ClientWebSocket();
        public WebSocketState State => _client.State;

        public Task ConnectAsync(Uri uri, CancellationToken cancellationToken) =>
            _client.ConnectAsync(uri, cancellationToken);

        public Task SendAsync(ArraySegment<byte> buffer, WebSocketMessageType messageType, bool endOfMessage,
            CancellationToken cancellationToken) =>
            _client.SendAsync(buffer, messageType, endOfMessage, cancellationToken);

        public Task<WebSocketReceiveResult> ReceiveAsync(ArraySegment<byte> buffer,
            CancellationToken cancellationToken) => _client.ReceiveAsync(buffer, cancellationToken);

        public void Dispose() => _client.Dispose();
    }
}
