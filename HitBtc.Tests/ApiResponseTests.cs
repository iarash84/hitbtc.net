using System.Collections.Generic;
using Hitbtc.HitBtcModel;
using Xunit;

namespace Hitbtc.Tests
{
    public class ApiResponseTests
    {
        [Fact]
        public void SymbolConversion_DeserializesJsonProperties()
        {
            var response = new ApiResponse
            {
                Content = "{\"id\":\"BTCUSD\",\"baseCurrency\":\"BTC\",\"quoteCurrency\":\"USD\"}"
            };

            Symbol symbol = response;

            Assert.Equal("BTCUSD", symbol.Id);
            Assert.Equal("BTC", symbol.BaseCurrency);
            Assert.Equal("USD", symbol.QuoteCurrency);
        }

        [Fact]
        public void BalanceListConversion_DeserializesAllItems()
        {
            var response = new ApiResponse
            {
                Content = "[{\"currency\":\"BTC\",\"available\":\"1.25\",\"reserved\":\"0.10\"},{\"currency\":\"ETH\",\"available\":\"2\",\"reserved\":\"0\"}]"
            };

            List<Balance> balances = response;

            Assert.Collection(
                balances,
                balance =>
                {
                    Assert.Equal("BTC", balance.Currency);
                    Assert.Equal("1.25", balance.Available);
                    Assert.Equal("0.10", balance.Reserved);
                },
                balance => Assert.Equal("ETH", balance.Currency));
        }

        [Fact]
        public void InvalidObjectJson_ReturnsEmptyModelInsteadOfThrowing()
        {
            var response = new ApiResponse { Content = "not-json" };

            Symbol symbol = response;

            Assert.NotNull(symbol);
            Assert.Null(symbol.Id);
        }

        [Fact]
        public void InvalidListJson_ReturnsEmptyListInsteadOfThrowing()
        {
            var response = new ApiResponse { Content = "not-json" };

            List<Balance> balances = response;

            Assert.NotNull(balances);
            Assert.Empty(balances);
        }

        [Fact]
        public void NullResponse_ConvertsToNullModel()
        {
            ApiResponse response = null;

            Symbol symbol = response;

            Assert.Null(symbol);
        }

        [Fact]
        public void TickerToString_ContainsImportantMarketFields()
        {
            var ticker = new Ticker
            {
                Symbol = "BTCUSD",
                Ask = "101",
                Bid = "100",
                Last = "100.5"
            };

            var text = ticker.ToString();

            Assert.Contains("symbol:BTCUSD", text);
            Assert.Contains("ask:101", text);
            Assert.Contains("bid:100", text);
            Assert.Contains("last:100.5", text);
        }
    }
}
