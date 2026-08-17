using System.Linq;
using System.Threading.Tasks;
using RestSharp;
using Xunit;

namespace Hitbtc.Tests
{
    public class RestRequestConstructionTests
    {
        [Fact]
        public async Task PostWithdraw_ValidInput_UsesPostAndFormParameters()
        {
            var api = new CapturingRestApi("{}");

            await api.Account.PostWithraw("BTC", 2, "wallet-address");

            Assert.Equal(Method.Post, api.Request.Method);
            Assert.Equal("/api/2/account/crypto/withdraw", api.Request.Resource);
            AssertParameter(api.Request, "currency", "BTC", ParameterType.GetOrPost);
            AssertParameter(api.Request, "amount", "2", ParameterType.GetOrPost);
            AssertParameter(api.Request, "address", "wallet-address", ParameterType.GetOrPost);
        }

        [Fact]
        public async Task PutWithdraw_ValidId_UsesPutAndPathParameter()
        {
            var api = new CapturingRestApi("{}");

            await api.Account.PutWithraw("withdraw-1");

            Assert.Equal(Method.Put, api.Request.Method);
            AssertParameter(api.Request, "id", "withdraw-1", ParameterType.UrlSegment);
        }

        [Fact]
        public async Task PostOrder_ValidInput_UsesPostAndFormParameters()
        {
            var api = new CapturingRestApi("{}");

            await api.Trading.PostOrders("BTCUSD", "1.5", price: "100");

            Assert.Equal(Method.Post, api.Request.Method);
            AssertParameter(api.Request, "symbol", "BTCUSD", ParameterType.GetOrPost);
            AssertParameter(api.Request, "quantity", "1.5", ParameterType.GetOrPost);
            AssertParameter(api.Request, "price", "100", ParameterType.GetOrPost);
        }

        [Fact]
        public async Task DeleteOrder_ValidId_UsesIdInResourcePath()
        {
            var api = new CapturingRestApi("{}");

            await api.Trading.DeleteOrder("client-1");

            Assert.Equal(Method.Delete, api.Request.Method);
            Assert.Equal("/api/2/order/{clientOrderId}", api.Request.Resource);
            AssertParameter(api.Request, "clientOrderId", "client-1", ParameterType.UrlSegment);
        }

        [Fact]
        public async Task GetOrders_WithSymbol_UsesQueryParameter()
        {
            var api = new CapturingRestApi("[]");

            await api.Trading.GetOrders("BTCUSD");

            Assert.Equal(Method.Get, api.Request.Method);
            AssertParameter(api.Request, "symbol", "BTCUSD", ParameterType.QueryString);
        }

        [Fact]
        public async Task GetCandles_WithPeriod_UsesPeriodQueryParameter()
        {
            var api = new CapturingRestApi("[]");

            await api.PublicData.GetCandles("BTCUSD", PublicEnum.EnPeriod.H4);

            AssertParameter(api.Request, "symbol", "BTCUSD", ParameterType.UrlSegment);
            AssertParameter(api.Request, "period", "H4", ParameterType.QueryString);
        }

        private static void AssertParameter(RestRequest request, string name, string value, ParameterType type)
        {
            var parameter = request.Parameters.Single(item => item.Name == name);
            Assert.Equal(type, parameter.Type);
            Assert.Equal(value, parameter.Value.ToString());
        }

        private sealed class CapturingRestApi : HitBtcRestApi
        {
            private readonly string _content;

            public CapturingRestApi(string content)
            {
                _content = content;
            }

            public RestRequest Request { get; private set; }

            public override Task<ApiResponse> Execute(RestRequest request, bool requireAuthentication = true)
            {
                Request = request;
                return Task.FromResult(new ApiResponse { Content = _content });
            }
        }
    }
}
