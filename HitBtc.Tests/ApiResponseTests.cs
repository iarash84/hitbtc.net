using System.Collections.Generic;
using Hitbtc.HitBtcModel;
using Newtonsoft.Json;
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
                Content = "{\"id\":\"BTCUSD\",\"base_currency\":\"BTC\",\"quote_currency\":\"USD\"}"
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
        public void InvalidObjectJson_ThrowsJsonException()
        {
            var response = new ApiResponse { Content = "not-json" };

            Assert.Throws<JsonReaderException>(() =>
            {
                Symbol symbol = response;
            });
        }

        [Fact]
        public void InvalidListJson_ThrowsJsonException()
        {
            var response = new ApiResponse { Content = "not-json" };

            Assert.Throws<JsonReaderException>(() =>
            {
                List<Balance> balances = response;
            });
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData("null")]
        public void EmptyObjectResponse_ThrowsJsonSerializationException(string content)
        {
            var response = new ApiResponse { Content = content };

            Assert.Throws<JsonSerializationException>(() =>
            {
                Symbol symbol = response;
            });
        }

        [Fact]
        public void TickerDictionaryConversion_DeserializesKeysAndNestedValues()
        {
            var response = new ApiResponse
            {
                Content = "{\"BTCUSD\":{\"last\":\"100.5\"},\"ETHUSD\":{\"last\":\"20.1\"}}"
            };

            Dictionary<string, Ticker> tickers = response;

            Assert.Equal("100.5", tickers["BTCUSD"].Last);
            Assert.Equal("20.1", tickers["ETHUSD"].Last);
        }

        [Fact]
        public void OrderBookConversion_DeserializesV3PriceSizeArrays()
        {
            var response = new ApiResponse
            {
                Content = "{\"ask\":[[\"101\",\"0.5\"]],\"bid\":[[\"100\",\"1.25\"]],\"timestamp\":\"2026-01-01T00:00:00.000Z\"}"
            };

            Orderbook orderbook = response;

            Assert.Equal("101", orderbook.Ask[0].Price);
            Assert.Equal("0.5", orderbook.Ask[0].Size);
            Assert.Equal("100", orderbook.Bid[0].Price);
        }

        [Fact]
        public void V3SnakeCaseFields_AreMappedToModels()
        {
            var response = new ApiResponse
            {
                Content = "{\"client_order_id\":\"client-1\",\"time_in_force\":\"GTC\",\"quantity_cumulative\":\"0.2\",\"created_at\":\"now\"}"
            };

            Order order = response;

            Assert.Equal("client-1", order.ClientOrderId);
            Assert.Equal("GTC", order.TimeInForce);
            Assert.Equal("0.2", order.CumQuantity);
            Assert.Equal("now", order.CreatedAt);
        }

        [Fact]
        public void CurrencyConversion_MapsV3NetworkAndDecimalPrecision()
        {
            var response = new ApiResponse
            {
                Content = "{\"full_name\":\"Bitcoin\",\"precision_transfer\":\"0.00000001\",\"networks\":[{\"code\":\"BTC\",\"network_name\":\"Bitcoin\",\"precision_payout\":\"0.00000001\"}]}"
            };

            Currency currency = response;

            Assert.Equal("Bitcoin", currency.FullName);
            Assert.Equal("0.00000001", currency.PrecisionTransfer);
            Assert.Equal("BTC", currency.Networks[0].Code);
            Assert.Equal("0.00000001", currency.Networks[0].PrecisionPayout);
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
