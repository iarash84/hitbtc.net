using System.Collections.Generic;
using System.Threading.Tasks;
using Hitbtc.HitBtcModel;
using RestSharp;

namespace Hitbtc.HitBtcCategories
{
    /// <summary>
    /// Market data RESTful API
    /// </summary>
    public class RestPublicData
    {
        private readonly HitBtcRestApi _hitBtcRestApi;

        public RestPublicData(HitBtcRestApi hitBtcRestApi)
        {
            _hitBtcRestApi = hitBtcRestApi;
        }

        public async Task<List<Symbol>> GetSymbol()
        {
            return await _hitBtcRestApi.Execute(new RestRequest("/api/2/public/symbol"), false);
        }

        public async Task<Symbol> GetSymbol(string symbolName)
        {
            var request = new RestRequest("/api/2/public/symbol/{symbol}");
            request.AddParameter("symbol", symbolName, ParameterType.UrlSegment);
            return await _hitBtcRestApi.Execute(request, false);
        }


        public async Task<List<Currency>> GetCurrency()
        {
            return await _hitBtcRestApi.Execute(new RestRequest("/api/2/public/currency"), false);
        }


        public async Task<Currency> GetCurrency(string currencyName)
        {
            var request = new RestRequest("/api/2/public/currency/{currency}");
            request.AddParameter("currency", currencyName, ParameterType.UrlSegment);
            return await _hitBtcRestApi.Execute(request, false);
        }

        public async Task<List<Ticker>> GetTicker()
        {
            return await _hitBtcRestApi.Execute(new RestRequest("/api/2/public/ticker"), false);
        }

        public async Task<Ticker> GetTicker(string symbolName, string period = null, int limit = 0)
        {
            var request = new RestRequest { Resource = "api/2/public/ticker/{symbol}" };
            request.AddParameter("symbol", symbolName, ParameterType.UrlSegment);
            if (limit > 0)
                request.Parameters.Add(new Parameter("limit", limit, ParameterType.GetOrPost));
            if (!string.IsNullOrEmpty(period))
                request.Parameters.Add(new Parameter("period", period, ParameterType.GetOrPost));
            return await _hitBtcRestApi.Execute(request, false);
        }

        public async Task<Orderbook> GetOrderbook(string symbolName, int limit = 0)
        {
            var request = new RestRequest("api/2/public/orderbook/{symbol}");
            request.AddParameter("symbol", symbolName, ParameterType.UrlSegment);
            request.Parameters.Add(new Parameter("limit", limit, ParameterType.GetOrPost));
            return await _hitBtcRestApi.Execute(request, false);
        }

        public async Task<List<Candle>> GetCandles(string symbolName, PublicEnum.EnPeriod enPeriod = PublicEnum.EnPeriod.M30)
        {
            var period = enPeriod.ToString() == "Month" ? "1M" : enPeriod.ToString();
            var request = new RestRequest("api/2/public/candles/{symbol}");
            request.AddParameter("symbol", symbolName, ParameterType.UrlSegment);
            request.AddParameter("period", period, ParameterType.UrlSegment);
            return await _hitBtcRestApi.Execute(request, false);
        }
    }
}