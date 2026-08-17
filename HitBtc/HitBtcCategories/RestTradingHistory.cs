using System.Collections.Generic;
using System.Threading.Tasks;
using Hitbtc.HitBtcModel;
using RestSharp;

namespace Hitbtc.HitBtcCategories
{
    public class RestTradingHistory
    {
        private readonly HitBtcRestApi _hitBtcRestApi;

        public RestTradingHistory(HitBtcRestApi hitBtcRestApi)
        {
            _hitBtcRestApi = hitBtcRestApi;
        }

        /// <summary>
        /// Get historical trades
        /// </summary>
        /// <param name="symoblName"></param>
        /// <param name="sort">Sort direction</param>
        /// <param name="by">Filter field</param>
        /// <param name="from">If filter by timestamp, then datetime in iso format or timestamp in millisecond otherwise trade id</param>
        /// <param name="till">If filter by timestamp, then datetime in iso format or timestamp in millisecond otherwise trade id</param>
        /// <param name="offset"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public async Task<List<TradeHistory>> GetTraders(string symoblName, string from, string till, int offset, int limit = 100,
            PublicEnum.EnSort sort = PublicEnum.EnSort.Desc, PublicEnum.EnBy by = PublicEnum.EnBy.timestamp)
        {
            var request = new RestRequest("/api/2/history/trades");
            if (!string.IsNullOrEmpty(symoblName))
                request.AddQueryParameter("symbol", symoblName);

            request.AddQueryParameter("sort", sort.ToString());

            request.AddQueryParameter("by", by.ToString());

            if (!string.IsNullOrEmpty(from))
                request.AddQueryParameter("from", from);
            if (!string.IsNullOrEmpty(till))
                request.AddQueryParameter("till", till);
            if (offset > 0)
                request.AddQueryParameter("offset", offset.ToString());
            if (limit > 0)
                request.AddQueryParameter("limit", limit.ToString());

            return await _hitBtcRestApi.Execute(request);
        }

        /// <summary>
        /// Get historical orders
        /// </summary>
        /// <returns></returns>
        public async Task<List<Order>> GetOrder(string symoblName, string clientOrderId, string from, string till,
            int offset, int limit = 100)
        {

            var request = new RestRequest("/api/2/history/order");
            if (!string.IsNullOrEmpty(symoblName))
                request.AddQueryParameter("symbol", symoblName);
            if (!string.IsNullOrEmpty(clientOrderId))
                request.AddQueryParameter("clientOrderId", clientOrderId);
            if (!string.IsNullOrEmpty(from))
                request.AddQueryParameter("from", from);
            if (!string.IsNullOrEmpty(till))
                request.AddQueryParameter("till", till);
            if (offset > 0)
                request.AddQueryParameter("offset", offset.ToString());
            if (limit > 0)
                request.AddQueryParameter("limit", limit.ToString());

            return await _hitBtcRestApi.Execute(request);
        }

        /// <summary>
        /// Get historical trades by specified order
        /// </summary>
        /// <returns></returns>
        public async Task<List<TradeHistory>> GetTradersByOrder(string orderId)
        {
            var request = new RestRequest("/api/2/history/order/{orderId}/trades");
            request.AddParameter("orderId", orderId, ParameterType.UrlSegment);
            return await _hitBtcRestApi.Execute(request);
        }
    }
}
