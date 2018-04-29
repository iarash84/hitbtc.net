using System.Collections.Generic;
using System.Threading.Tasks;
using Hitbtc.HitBtcModel;
using RestSharp;

namespace Hitbtc.HitBtcCategories
{
    /// <summary>
    /// Trading RESTful API
    /// </summary>
    public class RestTrading
    {
        private readonly HitBtcRestApi _hitBtcRestApi;

        public RestTrading(HitBtcRestApi hitBtcApi)
        {
            _hitBtcRestApi = hitBtcApi;
        }

        /// <summary>
        /// Get trading balance
        /// </summary>
        /// <returns></returns>
        public async Task<List<Balance>> GetBalance()
        {
            return await _hitBtcRestApi.Execute(new RestRequest("/api/2/trading/balance", Method.GET));
        }

        /// <summary>
        /// Get trading fee rate
        /// </summary>
        /// <param name="symbolName"></param>
        /// <returns></returns>
        public async Task<Fee> GetFee(string symbolName)
        {
            var request = new RestRequest("/api/2/trading/fee/{symbol}", Method.GET);
            request.AddParameter("symbol", symbolName, ParameterType.UrlSegment);
            return await _hitBtcRestApi.Execute(request);
        }

        /// <summary>
        /// List your current open orders
        /// </summary>
        /// <param name="symbolName"></param>
        /// <returns></returns>
        //public async Task<List<Order>> GetOrders(string symbolName)
        public async Task<List<Order>> GetOrders(string symbolName)
        {
            var request = new RestRequest("/api/2/order", Method.GET);
            request.AddParameter("symbol", symbolName, ParameterType.UrlSegment);
            return await _hitBtcRestApi.Execute(request);
        }

        /// <summary>
        /// Create new order
        /// </summary>
        /// <param name="symbolName"> Trading symbol</param>
        /// <param name="quantity"> Order quantity</param>
        /// <param name="side">sell buy</param>
        /// <param name="type"></param>
        /// <param name="timeInForce">Time in force is a special instruction used when placing a trade to indicate how long an order will remain active before it is executed or expires </param>
        /// <param name="price"></param>
        /// <param name="stopPrice"></param>
        /// <param name="expireTime"></param>
        /// <param name="clientOrderId">Unique identifier for Order as assigned by trader. Uniqueness must be guaranteed within a single trading day, including all active orders.</param>
        /// <param name="strictValidate"></param>
        /// <returns></returns>
        public async Task<Order> PostOrders(string symbolName, string quantity,
            PublicEnum.EnTradingSide side = PublicEnum.EnTradingSide.buy,
            PublicEnum.EnTradingType type = PublicEnum.EnTradingType.limit,
            PublicEnum.EnTradingTimeInForce timeInForce = PublicEnum.EnTradingTimeInForce.GTC,
            string price = null, string stopPrice = null, string expireTime = null,
            string clientOrderId = null, bool strictValidate = false)
        {
            var request = new RestRequest("/api/2/order", Method.POST);
            request.AddParameter("symbol", symbolName, ParameterType.UrlSegment);
            request.AddParameter("quantity", quantity, ParameterType.UrlSegment);
            request.AddParameter("side", side, ParameterType.UrlSegment);
            request.AddParameter("type", type, ParameterType.UrlSegment);
            request.AddParameter("timeInForce", timeInForce, ParameterType.UrlSegment);
            if (!string.IsNullOrEmpty(price))
                request.AddParameter("price", price, ParameterType.UrlSegment);
            if (!string.IsNullOrEmpty(stopPrice))
                request.AddParameter("stopPrice", stopPrice, ParameterType.UrlSegment);
            if (!string.IsNullOrEmpty(expireTime))
                request.AddParameter("expireTime", expireTime, ParameterType.UrlSegment);
            if (!string.IsNullOrEmpty(clientOrderId))
                request.AddParameter("clientOrderId", clientOrderId, ParameterType.UrlSegment);
            request.AddParameter("strictValidate", strictValidate, ParameterType.UrlSegment);
            return await _hitBtcRestApi.Execute(request);
        }

        /// <summary>
        /// Cancel all open orders
        /// </summary>
        /// <param name="symbolName"></param>
        /// <returns></returns>
        public async Task<List<Order>> DeleteOrders(string symbolName)
        {
            var request = new RestRequest("/api/2/order", Method.DELETE);
            request.AddParameter("symbol", symbolName, ParameterType.UrlSegment);
            return await _hitBtcRestApi.Execute(request);
        }


        /// <summary>
        /// Get a single order by clientOrderId
        /// </summary>
        /// <param name="clientOrderId"></param>
        /// <param name="wait">Optional parameter. Time in milliseconds. Max 60000. </param>
        /// <returns></returns>
        public async Task<Order> GetOrder(string clientOrderId, int wait = 0)
        {
            var request = new RestRequest("/api/2/order/{clientOrderId}", Method.GET);
            request.AddParameter("clientOrderId", clientOrderId, ParameterType.UrlSegment);
            if (wait > 0)
                request.AddParameter("wait", wait, ParameterType.UrlSegment);
            return await _hitBtcRestApi.Execute(request);
        }

        /// <summary>
        /// Create new order
        /// </summary>
        /// <param name="symbolName"> Trading symbol</param>
        /// <param name="quantity"> Order quantity</param>
        /// <param name="side">sell buy</param>
        /// <param name="type"></param>
        /// <param name="timeInForce">Time in force is a special instruction used when placing a trade to indicate how long an order will remain active before it is executed or expires </param>
        /// <param name="price"></param>
        /// <param name="stopPrice"></param>
        /// <param name="expireTime"></param>
        /// <param name="clientOrderId">Unique identifier for Order as assigned by trader. Uniqueness must be guaranteed within a single trading day, including all active orders.</param>
        /// <param name="strictValidate"></param>
        /// <returns></returns>
        public async Task<Order> PutOrder(string clientOrderId ,string symbolName, string quantity,
            PublicEnum.EnTradingSide side = PublicEnum.EnTradingSide.buy,
            PublicEnum.EnTradingType type = PublicEnum.EnTradingType.limit,
            PublicEnum.EnTradingTimeInForce timeInForce = PublicEnum.EnTradingTimeInForce.GTC,
            string price = null, string stopPrice = null, string expireTime = null, bool strictValidate = false)
        {
            var request = new RestRequest("/api/2/order/{clientOrderId}", Method.PUT);
            request.AddParameter("clientOrderId", clientOrderId, ParameterType.UrlSegment);
            request.AddParameter("symbol", symbolName, ParameterType.UrlSegment);
            request.AddParameter("quantity", quantity, ParameterType.UrlSegment);
            request.AddParameter("side", side, ParameterType.UrlSegment);
            request.AddParameter("type", type, ParameterType.UrlSegment);
            request.AddParameter("timeInForce", timeInForce, ParameterType.UrlSegment);
            if (!string.IsNullOrEmpty(price))
                request.AddParameter("price", price, ParameterType.UrlSegment);
            if (!string.IsNullOrEmpty(stopPrice))
                request.AddParameter("stopPrice", stopPrice, ParameterType.UrlSegment);
            if (!string.IsNullOrEmpty(expireTime))
                request.AddParameter("expireTime", expireTime, ParameterType.UrlSegment);
            request.AddParameter("strictValidate", strictValidate, ParameterType.UrlSegment);
            return await _hitBtcRestApi.Execute(request);
        }

        /// <summary>
        /// Cancel all open orders
        /// </summary>
        /// <param name="clientOrderId"></param>
        /// <returns></returns>
        public async Task<Order> DeleteOrder(string clientOrderId)
        {
            var request = new RestRequest("/api/2/order", Method.DELETE);
            request.AddParameter("clientOrderId", clientOrderId, ParameterType.UrlSegment);
            return await _hitBtcRestApi.Execute(request);
        }

        /// <summary>
        /// Cancel Replace order
        /// </summary>
        /// <param name="clientOrderId"></param>
        /// <param name="quantity"></param>
        /// <param name="requestClientId"></param>
        /// <param name="price"></param>
        /// <returns></returns>
        public async Task<Order> PatchOrder(string clientOrderId, string quantity, string requestClientId,
            string price = null)
        {
            var request = new RestRequest("/api/2/order/{clientOrderId}", Method.PATCH);
            request.AddParameter("clientOrderId", clientOrderId, ParameterType.UrlSegment);
            request.AddParameter("quantity", quantity, ParameterType.UrlSegment);
            request.AddParameter("requestClientId", requestClientId, ParameterType.UrlSegment);
            if (!string.IsNullOrEmpty(price))
                request.AddParameter("price", price, ParameterType.UrlSegment);
            return await _hitBtcRestApi.Execute(request);
        }
    }
}
