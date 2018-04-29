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
                request.Parameters.Add(new Parameter
                {
                    Name = "symbol",
                    Value = symoblName,
                    Type = ParameterType.GetOrPost
                });

            request.Parameters.Add(new Parameter
            {
                Name = "sort",
                Value = sort.ToString(),
                Type = ParameterType.GetOrPost
            });

            request.Parameters.Add(new Parameter
            {
                Name = "by",
                Value = by.ToString(),
                Type = ParameterType.GetOrPost
            });

            if (!string.IsNullOrEmpty(from))
                request.Parameters.Add(new Parameter {Name = "from", Value = from, Type = ParameterType.GetOrPost});
            if (!string.IsNullOrEmpty(till))
                request.Parameters.Add(new Parameter {Name = "till", Value = till, Type = ParameterType.GetOrPost});
            if (offset > 0)
                request.Parameters.Add(new Parameter {Name = "offset", Value = offset, Type = ParameterType.GetOrPost});
            if (limit > 0)
                request.Parameters.Add(new Parameter {Name = "limit", Value = limit, Type = ParameterType.GetOrPost});

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
                request.Parameters.Add(new Parameter
                {
                    Name = "symbol",
                    Value = symoblName,
                    Type = ParameterType.GetOrPost
                });
            if (!string.IsNullOrEmpty(clientOrderId))
                request.Parameters.Add(new Parameter
                {
                    Name = "clientOrderId",
                    Value = clientOrderId,
                    Type = ParameterType.GetOrPost
                });
            if (!string.IsNullOrEmpty(from))
                request.Parameters.Add(new Parameter {Name = "from", Value = from, Type = ParameterType.GetOrPost});
            if (!string.IsNullOrEmpty(till))
                request.Parameters.Add(new Parameter {Name = "till", Value = till, Type = ParameterType.GetOrPost});
            if (offset > 0)
                request.Parameters.Add(new Parameter {Name = "offset", Value = offset, Type = ParameterType.GetOrPost});
            if (limit > 0)
                request.Parameters.Add(new Parameter {Name = "limit", Value = limit, Type = ParameterType.GetOrPost});



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
