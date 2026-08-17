using System;
using System.Threading.Tasks;
using Hitbtc.HitBtcModel;
using Newtonsoft.Json.Linq;

namespace Hitbtc.HitBtcCategories
{
    /// <summary>Public streaming subscriptions for HitBTC API v3.</summary>
    public class SocketMarketData
    {
        private readonly HitBtcSocketApi _api;
        public SocketMarketData(HitBtcSocketApi api) { _api = api; }

        [Obsolete("API v3 exposes currencies through RestApi.PublicData.")]
        public Task<SocketCurrencies> GetCurrencies(int id = 123) { return Unsupported<SocketCurrencies>(); }

        [Obsolete("API v3 exposes currencies through RestApi.PublicData.")]
        public Task<SocketCurrency> GetCurrency(string currencyName, int id = 123) { return Unsupported<SocketCurrency>(); }

        [Obsolete("API v3 exposes symbols through RestApi.PublicData.")]
        public Task<SocketSymbols> GetSymbols(int id = 123) { return Unsupported<SocketSymbols>(); }

        [Obsolete("API v3 exposes symbols through RestApi.PublicData.")]
        public Task<SocketSymbol> GetSymbol(string symbol, int id = 123) { return Unsupported<SocketSymbol>(); }

        [Obsolete("API v3 exposes historical trades through RestApi.TradingHistory.")]
        public Task<SocketTrades> GetTrades(string symoblName, string from, string till, int offset,
            int id = 123, int limit = 100, PublicEnum.EnSort sort = PublicEnum.EnSort.Desc,
            PublicEnum.EnBy by = PublicEnum.EnBy.timestamp) { return Unsupported<SocketTrades>(); }

        public Task<SocketSubscribe> SubscribeTicker(string symbol, int id = 123)
        {
            return Subscription("subscribe", "ticker/1s", symbol, id);
        }

        public Task<SocketSubscribe> UnsubscribeTicker(string symbol, int id = 123)
        {
            return Subscription("unsubscribe", "ticker/1s", symbol, id);
        }

        public Task<SocketSubscribe> SubscribeOrderbook(string symbol, int id = 123)
        {
            return Subscription("subscribe", "orderbook/full", symbol, id);
        }

        public Task<SocketSubscribe> UnsubscribeOrderbook(string symbol, int id = 123)
        {
            return Subscription("unsubscribe", "orderbook/full", symbol, id);
        }

        public Task<SocketSubscribe> SubscribeTrades(string symbol, int id = 123)
        {
            return Subscription("subscribe", "trades", symbol, id);
        }

        public Task<SocketSubscribe> UnsubscribeTrades(string symbol, int id = 123)
        {
            return Subscription("unsubscribe", "trades", symbol, id);
        }

        public Task<SocketSubscribe> SubscribeCandles(string symbol,
            PublicEnum.EnPeriod enPeriod = PublicEnum.EnPeriod.M30, int id = 123)
        {
            return Subscription("subscribe", "candles/" + Period(enPeriod), symbol, id);
        }

        public Task<SocketSubscribe> UnsubscribeCandles(string symbol,
            PublicEnum.EnPeriod enPeriod = PublicEnum.EnPeriod.M30, int id = 123)
        {
            return Subscription("unsubscribe", "candles/" + Period(enPeriod), symbol, id);
        }

        private async Task<SocketSubscribe> Subscription(string method, string channel,
            string symbol, int id)
        {
            var request = new JObject
            {
                ["method"] = method,
                ["ch"] = channel,
                ["params"] = new JObject { ["symbols"] = new JArray(symbol) },
                ["id"] = id
            }.ToString(Newtonsoft.Json.Formatting.None);
            return await _api.Execute(request, false);
        }

        private static string Period(PublicEnum.EnPeriod period)
        {
            return period == PublicEnum.EnPeriod.Month ? "1M" : period.ToString();
        }

        private static Task<T> Unsupported<T>()
        {
            var source = new TaskCompletionSource<T>();
            source.SetException(new NotSupportedException("This request was removed from HitBTC WebSocket API v3."));
            return source.Task;
        }
    }
}
