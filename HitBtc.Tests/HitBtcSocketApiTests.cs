using System.Threading.Tasks;
using Xunit;

namespace Hitbtc.Tests
{
    public class HitBtcSocketApiTests
    {
        [Fact]
        public void Constructor_InitializesSocketCategories()
        {
            var api = new HitBtcSocketApi();

            Assert.NotNull(api.MarketData);
            Assert.NotNull(api.Trading);
            Assert.False(api.IsAuthorized);
        }

        [Fact]
        public void Authorize_MarksSocketClientAsAuthorized()
        {
            var api = new HitBtcSocketApi();

            api.Authorize("api-key", "secret-key");

            Assert.True(api.IsAuthorized);
        }

        [Fact]
        public async Task Execute_AuthenticatedRequestWithoutAuthorization_ReturnsNullWithoutConnecting()
        {
            var api = new HitBtcSocketApi();

            var response = await api.Execute("{}", true);

            Assert.Null(response);
        }
    }
}
