using System;
using System.Reflection;
using System.Threading.Tasks;
using RestSharp;
using Xunit;

namespace Hitbtc.Tests
{
    public class HitBtcRestApiTests
    {
        [Fact]
        public void Constructor_InitializesAllApiCategories()
        {
            var api = new HitBtcRestApi();

            Assert.NotNull(api.PublicData);
            Assert.NotNull(api.Trading);
            Assert.NotNull(api.Account);
            Assert.NotNull(api.TradingHistory);
            Assert.False(api.IsAuthorized);
        }

        [Fact]
        public void Authorize_StoresCredentialsAndMarksClientAsAuthorized()
        {
            var api = new HitBtcRestApi();

            api.Authorize("api-key", "secret-key");

            Assert.True(api.IsAuthorized);
            Assert.Equal("api-key", ReadPrivateField(api, "_apiKey"));
            Assert.Equal("secret-key", ReadPrivateField(api, "_secretKey"));
        }

        [Fact]
        public async Task Execute_AuthenticatedRequestWithoutAuthorization_ThrowsBeforeNetworkCall()
        {
            var api = new HitBtcRestApi();
            var request = new RestRequest("/api/2/trading/balance", Method.Get);

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => api.Execute(request));

            Assert.Contains("requires authorization", exception.Message);
        }

        [Theory]
        [InlineData(null, "secret")]
        [InlineData("", "secret")]
        [InlineData("key", null)]
        [InlineData("key", "  ")]
        public void Authorize_MissingCredential_ThrowsAndKeepsUnauthorized(string apiKey, string secretKey)
        {
            var api = new HitBtcRestApi();

            Assert.Throws<ArgumentException>(() => api.Authorize(apiKey, secretKey));
            Assert.False(api.IsAuthorized);
        }

        private static string ReadPrivateField(HitBtcRestApi api, string fieldName)
        {
            var field = typeof(HitBtcRestApi).GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic);
            Assert.NotNull(field);
            return (string)field.GetValue(api);
        }
    }
}
