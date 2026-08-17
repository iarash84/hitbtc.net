using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Hitbtc.Tests
{
    public class HitBtcSocketApiTests
    {
        [Fact]
        public void Constructor_InitializesSocketCategories()
        {
            using (var api = new HitBtcSocketApi())
            {
                Assert.NotNull(api.MarketData);
                Assert.NotNull(api.Trading);
                Assert.False(api.IsAuthorized);
            }
        }

        [Fact]
        public void Authorize_MissingCredential_ThrowsAndKeepsUnauthorized()
        {
            using (var api = new HitBtcSocketApi())
            {
                Assert.Throws<ArgumentException>(() => api.Authorize("", "secret"));
                Assert.Throws<ArgumentException>(() => api.Authorize("key", null));
                Assert.False(api.IsAuthorized);
            }
        }

        [Fact]
        public async Task Execute_WithoutAuthorization_ThrowsWithoutConnecting()
        {
            var socket = new FakeWebSocketClient();
            using (var api = new HitBtcSocketApi(socket))
            {
                await Assert.ThrowsAsync<InvalidOperationException>(() => api.Execute("{}", true));
                Assert.Equal(0, socket.ConnectCount);
            }
        }

        [Fact]
        public async Task Execute_FragmentedUtf8Message_AssemblesExactReceivedBytes()
        {
            var socket = new FakeWebSocketClient();
            socket.EnqueueTextFragments("{\"result\":\"", "سلام", "\"}");
            using (var api = new HitBtcSocketApi(socket))
            {
                var response = await api.Execute("{}", false);
                Assert.Equal("{\"result\":\"سلام\"}", response.Content);
                Assert.DoesNotContain("\0", response.Content);
            }
        }

        [Fact]
        public async Task Execute_RepeatedRequests_ReusesOpenConnection()
        {
            var socket = new FakeWebSocketClient();
            socket.EnqueueText("{\"id\":1}");
            socket.EnqueueText("{\"id\":2}");
            using (var api = new HitBtcSocketApi(socket))
            {
                await api.Execute("{\"id\":1}", false);
                await api.Execute("{\"id\":2}", false);
                Assert.Equal(1, socket.ConnectCount);
                Assert.Equal(2, socket.SentMessages.Count);
                Assert.Equal("wss://api.hitbtc.com/api/3/ws/public", socket.ConnectedUri.ToString());
            }
        }

        [Fact]
        public async Task SubscribeTicker_UsesV3ChannelProtocol()
        {
            var socket = new FakeWebSocketClient();
            socket.EnqueueText("{\"result\":\"ok\",\"id\":7}");
            using (var api = new HitBtcSocketApi(socket))
            {
                await api.MarketData.SubscribeTicker("BTCUSDT", 7);

                var request = socket.SentMessages.Single();
                Assert.Contains("\"method\":\"subscribe\"", request);
                Assert.Contains("\"ch\":\"ticker/1s\"", request);
                Assert.Contains("\"symbols\":[\"BTCUSDT\"]", request);
            }
        }

        [Fact]
        public async Task TradingCommand_UsesTradingEndpointAndSnakeCaseMethod()
        {
            var publicSocket = new FakeWebSocketClient();
            var tradingSocket = new FakeWebSocketClient();
            tradingSocket.EnqueueText("{\"result\":true}");
            tradingSocket.EnqueueText("{\"result\":{},\"id\":9}");
            using (var api = new HitBtcSocketApi(publicSocket, tradingSocket))
            {
                api.Authorize("key", "secret");
                await api.Trading.NewOrder("BTCUSDT", "client-9", "0.01", "100", 9);

                Assert.Equal("wss://api.hitbtc.com/api/3/ws/trading", tradingSocket.ConnectedUri.ToString());
                Assert.Contains("\"method\":\"spot_new_order\"", tradingSocket.SentMessages.Last());
                Assert.Contains("\"client_order_id\":\"client-9\"", tradingSocket.SentMessages.Last());
                Assert.Equal(0, publicSocket.ConnectCount);
            }
        }

        [Fact]
        public async Task Execute_AuthenticatedRequests_AuthenticateOnlyOnce()
        {
            var socket = new FakeWebSocketClient();
            socket.EnqueueText("{\"result\":true}");
            socket.EnqueueText("{\"result\":[]}");
            socket.EnqueueText("{\"result\":[]}");
            using (var api = new HitBtcSocketApi(socket))
            {
                api.Authorize("api-key", "secret-key");
                await api.Execute("{\"method\":\"first\"}");
                await api.Execute("{\"method\":\"second\"}");
                Assert.Equal(1, socket.SentMessages.Count(x => x.Contains("\"method\":\"login\"")));
            }
        }

        [Fact]
        public async Task Execute_AuthenticationError_ThrowsMeaningfulException()
        {
            var socket = new FakeWebSocketClient();
            socket.EnqueueText("{\"error\":{\"message\":\"invalid\"}}");
            using (var api = new HitBtcSocketApi(socket))
            {
                api.Authorize("key", "secret");
                var error = await Assert.ThrowsAsync<InvalidOperationException>(() => api.Execute("{}"));
                Assert.Contains("authentication failed", error.Message);
            }
        }

        [Fact]
        public async Task Execute_ServerClose_ThrowsWebSocketException()
        {
            var socket = new FakeWebSocketClient();
            socket.EnqueueClose();
            using (var api = new HitBtcSocketApi(socket))
                await Assert.ThrowsAsync<WebSocketException>(() => api.Execute("{}", false));
        }

        [Fact]
        public async Task Execute_CancelledToken_CancelsBeforeNetworkOperation()
        {
            var socket = new FakeWebSocketClient();
            using (var api = new HitBtcSocketApi(socket))
            using (var source = new CancellationTokenSource())
            {
                source.Cancel();
                await Assert.ThrowsAnyAsync<OperationCanceledException>(() => api.Execute("{}", false, source.Token));
                Assert.Equal(0, socket.ConnectCount);
            }
        }

        private sealed class FakeWebSocketClient : IWebSocketClient
        {
            private readonly Queue<Fragment> _fragments = new Queue<Fragment>();
            public WebSocketState State { get; private set; } = WebSocketState.None;
            public int ConnectCount { get; private set; }
            public Uri ConnectedUri { get; private set; }
            public List<string> SentMessages { get; } = new List<string>();

            public void EnqueueText(string text) => Enqueue(text, WebSocketMessageType.Text, true);
            public void EnqueueTextFragments(params string[] parts)
            {
                for (var i = 0; i < parts.Length; i++) Enqueue(parts[i], WebSocketMessageType.Text, i == parts.Length - 1);
            }
            public void EnqueueClose() => Enqueue("", WebSocketMessageType.Close, true);
            private void Enqueue(string text, WebSocketMessageType type, bool end) =>
                _fragments.Enqueue(new Fragment(Encoding.UTF8.GetBytes(text), type, end));

            public Task ConnectAsync(Uri uri, CancellationToken token)
            {
                token.ThrowIfCancellationRequested(); ConnectCount++; ConnectedUri = uri; State = WebSocketState.Open; return Task.CompletedTask;
            }
            public Task SendAsync(ArraySegment<byte> buffer, WebSocketMessageType type, bool end, CancellationToken token)
            {
                token.ThrowIfCancellationRequested();
                SentMessages.Add(Encoding.UTF8.GetString(buffer.Array, buffer.Offset, buffer.Count));
                return Task.CompletedTask;
            }
            public Task<WebSocketReceiveResult> ReceiveAsync(ArraySegment<byte> buffer, CancellationToken token)
            {
                token.ThrowIfCancellationRequested();
                var item = _fragments.Dequeue();
                Array.Copy(item.Bytes, 0, buffer.Array, buffer.Offset, item.Bytes.Length);
                return Task.FromResult(new WebSocketReceiveResult(item.Bytes.Length, item.Type, item.End));
            }
            public void Dispose() => State = WebSocketState.Closed;

            private sealed class Fragment
            {
                public Fragment(byte[] bytes, WebSocketMessageType type, bool end) { Bytes = bytes; Type = type; End = end; }
                public byte[] Bytes { get; }
                public WebSocketMessageType Type { get; }
                public bool End { get; }
            }
        }
    }
}
