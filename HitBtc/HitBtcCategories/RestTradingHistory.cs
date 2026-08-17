using System.Collections.Generic;
using System.Threading.Tasks;
using Hitbtc.HitBtcModel;
using RestSharp;

namespace Hitbtc.HitBtcCategories
{
    /// <summary>Spot order and trade history for HitBTC API v3.</summary>
    public class RestTradingHistory
    {
        private readonly HitBtcRestApi _api;
        public RestTradingHistory(HitBtcRestApi api) { _api = api; }

        public async Task<List<TradeHistory>> GetTraders(string symoblName, string from,
            string till, int offset, int limit = 100,
            PublicEnum.EnSort sort = PublicEnum.EnSort.Desc,
            PublicEnum.EnBy by = PublicEnum.EnBy.timestamp)
        {
            var request = HistoryRequest("/api/3/spot/history/trade", symoblName, from, till,
                offset, limit, sort, by);
            return await _api.Execute(request);
        }

        public async Task<List<Order>> GetOrder(string symoblName, string clientOrderId,
            string from, string till, int offset, int limit = 100)
        {
            var request = HistoryRequest("/api/3/spot/history/order", symoblName, from, till,
                offset, limit, PublicEnum.EnSort.Desc, PublicEnum.EnBy.timestamp);
            AddOptional(request, "client_order_id", clientOrderId);
            return await _api.Execute(request);
        }

        public async Task<List<TradeHistory>> GetTradersByOrder(string orderId)
        {
            var request = new RestRequest("/api/3/spot/history/trade");
            request.AddQueryParameter("order_id", orderId);
            return await _api.Execute(request);
        }

        private static RestRequest HistoryRequest(string resource, string symbol, string from,
            string till, int offset, int limit, PublicEnum.EnSort sort, PublicEnum.EnBy by)
        {
            var request = new RestRequest(resource);
            AddOptional(request, "symbol", symbol);
            AddOptional(request, "from", from);
            AddOptional(request, "till", till);
            request.AddQueryParameter("sort", sort.ToString().ToUpperInvariant());
            request.AddQueryParameter("by", by.ToString());
            if (offset > 0) request.AddQueryParameter("offset", offset.ToString());
            if (limit > 0) request.AddQueryParameter("limit", limit.ToString());
            return request;
        }

        private static void AddOptional(RestRequest request, string name, string value)
        {
            if (!string.IsNullOrWhiteSpace(value)) request.AddQueryParameter(name, value);
        }
    }
}
