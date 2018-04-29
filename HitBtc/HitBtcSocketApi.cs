using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Hitbtc.HitBtcCategories;
using Newtonsoft.Json;

namespace Hitbtc
{
    /// <summary>
    /// https://api.hitbtc.com/
    /// Use JSON-RPC 2.0 over WebSocket connection as transport.
    /// </summary>
    public class HitBtcSocketApi
    {
        private const string Uri = "wss://api.hitbtc.com/api/2/ws";
        private readonly ClientWebSocket _clientWebSocket;
        private string _apiKey;
        private string _secretKey;

        public SocketMarketData MarketData { get; set; }
        public SocketTrading Trading { set; get; }
        public bool IsAuthorized { get; set; }

        public HitBtcSocketApi()
        {
            MarketData = new SocketMarketData(this);
            Trading = new SocketTrading(this);
            _clientWebSocket = new ClientWebSocket();
        }

        public async Task<ApiResponse> Execute(string request, bool requireAuthentication = true)
        {
            try
            {
                if (requireAuthentication && !IsAuthorized)
                    throw new Exception("AccessTokenInvalid");

                await ConnectToServer();

                if (requireAuthentication)
                {
                    var loginRequest =
                        string.Format("{{ \"method\": \"login\", \"params\": {{ \"algo\": \"BASIC\", \"pKey\": \"{0}\", \"sKey\": \"{1}\"}} }}", _apiKey, _secretKey);
                    await SendCommand(loginRequest);
                    var reciveLoginData = await Receive();
                }

                await SendCommand(request);
                var reciveData = await Receive();
                return new ApiResponse { Content = reciveData };
            }
            catch (Exception e)
            {
                return null;
            }
        }


        private async Task ConnectToServer()
        {
            await _clientWebSocket.ConnectAsync(new Uri(Uri), CancellationToken.None);
        }

        private async Task SendCommand(string jsonCmd)
        {
            ArraySegment<byte> outputBuffer = new ArraySegment<byte>(Encoding.UTF8.GetBytes(jsonCmd));
            await _clientWebSocket.SendAsync(outputBuffer, WebSocketMessageType.Text, true, CancellationToken.None);
        }

        private async Task<string> Receive()
        {
            const int bufferSize = 8192;
            var temporaryBuffer = new byte[bufferSize];
            var buffer = new byte[bufferSize * 20];
            int offset = 0;
            while (true)
            {
                var webSocketReceiveResult = await _clientWebSocket.ReceiveAsync(
                    new ArraySegment<byte>(temporaryBuffer),
                    CancellationToken.None);
                temporaryBuffer.CopyTo(buffer, offset);
                offset += webSocketReceiveResult.Count;
                temporaryBuffer = new byte[bufferSize];
                if (webSocketReceiveResult.EndOfMessage)
                {
                    break;
                }
            }
            var resultJson = (new UTF8Encoding()).GetString(buffer);
            return resultJson;
        }

        public void Authorize(string apiKey, string secretKey)
        {
            _apiKey = apiKey;
            _secretKey = secretKey;
            IsAuthorized = true;
        }
    }
}
