using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hitbtc.HitBtcModel;
using RestSharp;
using System.Globalization;

namespace Hitbtc.HitBtcCategories
{
    /// <summary>Public market-data operations for HitBTC API v3.</summary>
    public class RestPublicData
    {
        private readonly HitBtcRestApi _api;
        public RestPublicData(HitBtcRestApi api) { _api = api; }

        public async Task<List<Symbol>> GetSymbol()
        {
            var response = await _api.Execute(new RestRequest("/api/3/public/symbol"), false);
            var values = Utilities.ConvertDictionaryFromJson<Symbol>(response);
            foreach (var item in values) item.Value.Id = item.Key;
            return values.Values.ToList();
        }

        public async Task<Symbol> GetSymbol(string symbolName)
        {
            var request = new RestRequest("/api/3/public/symbol/{symbol}");
            request.AddParameter("symbol", symbolName, ParameterType.UrlSegment);
            Symbol value = await _api.Execute(request, false);
            value.Id = symbolName;
            return value;
        }

        public async Task<List<Currency>> GetCurrency()
        {
            var response = await _api.Execute(new RestRequest("/api/3/public/currency"), false);
            var values = Utilities.ConvertDictionaryFromJson<Currency>(response);
            foreach (var item in values) item.Value.Id = item.Key;
            return values.Values.ToList();
        }

        public async Task<Currency> GetCurrency(string currencyName)
        {
            var request = new RestRequest("/api/3/public/currency/{currency}");
            request.AddParameter("currency", currencyName, ParameterType.UrlSegment);
            Currency value = await _api.Execute(request, false);
            value.Id = currencyName;
            return value;
        }

        public async Task<List<Ticker>> GetTicker()
        {
            var response = await _api.Execute(new RestRequest("/api/3/public/ticker"), false);
            var values = Utilities.ConvertTickerDictionaryFromJson(response);
            foreach (var item in values) item.Value.Symbol = item.Key;
            return values.Values.ToList();
        }

        public async Task<Ticker> GetTicker(string symbolName, string? period = null, int limit = 0)
        {
            var request = new RestRequest("/api/3/public/ticker/{symbol}");
            request.AddParameter("symbol", symbolName, ParameterType.UrlSegment);
            Ticker value = await _api.Execute(request, false);
            value.Symbol = symbolName;
            return value;
        }

        public async Task<Orderbook> GetOrderbook(string symbolName, int limit = 0)
        {
            var request = new RestRequest("/api/3/public/orderbook/{symbol}");
            request.AddParameter("symbol", symbolName, ParameterType.UrlSegment);
            if (limit > 0) request.AddQueryParameter("depth", limit.ToString(CultureInfo.InvariantCulture));
            return await _api.Execute(request, false);
        }

        public async Task<List<Candle>> GetCandles(string symbolName,
            PublicEnum.EnPeriod enPeriod = PublicEnum.EnPeriod.M30)
        {
            var period = enPeriod == PublicEnum.EnPeriod.Month ? "1M" : enPeriod.ToString();
            var request = new RestRequest("/api/3/public/candles/{symbol}");
            request.AddParameter("symbol", symbolName, ParameterType.UrlSegment);
            request.AddQueryParameter("period", period);
            return await _api.Execute(request, false);
        }
    }
}
