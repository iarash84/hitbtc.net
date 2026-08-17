using System.Collections.Generic;
using System.Threading.Tasks;
using Hitbtc.HitBtcModel;
using RestSharp;

namespace Hitbtc.HitBtcCategories
{
    /// <summary>Spot trading operations for HitBTC API v3.</summary>
    public class RestTrading
    {
        private readonly HitBtcRestApi _api;
        public RestTrading(HitBtcRestApi api) { _api = api; }

        public async Task<List<Balance>> GetBalance()
        {
            return await _api.Execute(new RestRequest("/api/3/spot/balance", Method.Get));
        }

        public async Task<Fee> GetFee(string symbolName)
        {
            var request = new RestRequest("/api/3/spot/fee/{symbol}", Method.Get);
            request.AddParameter("symbol", symbolName, ParameterType.UrlSegment);
            return await _api.Execute(request);
        }

        public async Task<List<Order>> GetOrders(string? symbolName = null)
        {
            var request = new RestRequest("/api/3/spot/order", Method.Get);
            AddOptionalQuery(request, "symbol", symbolName);
            return await _api.Execute(request);
        }

        public async Task<Order> PostOrders(string symbolName, string quantity,
            PublicEnum.EnTradingSide side = PublicEnum.EnTradingSide.buy,
            PublicEnum.EnTradingType type = PublicEnum.EnTradingType.limit,
            PublicEnum.EnTradingTimeInForce timeInForce = PublicEnum.EnTradingTimeInForce.GTC,
            string? price = null, string? stopPrice = null, string? expireTime = null,
            string? clientOrderId = null, bool strictValidate = false)
        {
            var request = CreateOrderRequest("/api/3/spot/order", Method.Post, symbolName, quantity,
                side, type, timeInForce, price, stopPrice, expireTime, strictValidate);
            AddOptionalBody(request, "client_order_id", clientOrderId);
            return await _api.Execute(request);
        }

        public async Task<List<Order>> DeleteOrders(string? symbolName = null)
        {
            var request = new RestRequest("/api/3/spot/order", Method.Delete);
            AddOptionalQuery(request, "symbol", symbolName);
            return await _api.Execute(request);
        }

        public async Task<Order> GetOrder(string clientOrderId, int wait = 0)
        {
            return await _api.Execute(OrderRequest(clientOrderId, Method.Get));
        }

        public async Task<Order> PutOrder(string clientOrderId, string symbolName, string quantity,
            PublicEnum.EnTradingSide side = PublicEnum.EnTradingSide.buy,
            PublicEnum.EnTradingType type = PublicEnum.EnTradingType.limit,
            PublicEnum.EnTradingTimeInForce timeInForce = PublicEnum.EnTradingTimeInForce.GTC,
            string? price = null, string? stopPrice = null, string? expireTime = null,
            bool strictValidate = false)
        {
            var request = CreateOrderRequest("/api/3/spot/order/{clientOrderId}", Method.Put, symbolName,
                quantity, side, type, timeInForce, price, stopPrice, expireTime, strictValidate);
            request.AddParameter("clientOrderId", clientOrderId, ParameterType.UrlSegment);
            return await _api.Execute(request);
        }

        public async Task<Order> DeleteOrder(string clientOrderId)
        {
            return await _api.Execute(OrderRequest(clientOrderId, Method.Delete));
        }

        public async Task<Order> PatchOrder(string clientOrderId, string quantity,
            string requestClientId, string? price = null)
        {
            var request = OrderRequest(clientOrderId, Method.Patch);
            AddOptionalBody(request, "quantity", quantity);
            AddOptionalBody(request, "new_client_order_id", requestClientId);
            AddOptionalBody(request, "price", price);
            return await _api.Execute(request);
        }

        private static RestRequest CreateOrderRequest(string resource, Method method, string symbol,
            string quantity, PublicEnum.EnTradingSide side, PublicEnum.EnTradingType type,
            PublicEnum.EnTradingTimeInForce timeInForce, string? price, string? stopPrice,
            string? expireTime, bool strictValidate)
        {
            var request = new RestRequest(resource, method);
            request.AddParameter("symbol", symbol);
            request.AddParameter("quantity", quantity);
            request.AddParameter("side", side.ToString());
            request.AddParameter("type", ToApiValue(type));
            request.AddParameter("time_in_force", timeInForce.ToString());
            AddOptionalBody(request, "price", price);
            AddOptionalBody(request, "stop_price", stopPrice);
            AddOptionalBody(request, "expire_time", expireTime);
            request.AddParameter("strict_validate", strictValidate.ToString().ToLowerInvariant());
            return request;
        }

        private static RestRequest OrderRequest(string clientOrderId, Method method)
        {
            var request = new RestRequest("/api/3/spot/order/{clientOrderId}", method);
            request.AddParameter("clientOrderId", clientOrderId, ParameterType.UrlSegment);
            return request;
        }

        private static string ToApiValue(PublicEnum.EnTradingType type)
        {
            switch (type)
            {
                case PublicEnum.EnTradingType.stopLimit: return "stop_limit";
                case PublicEnum.EnTradingType.stopMarket: return "stop_market";
                default: return type.ToString();
            }
        }

        private static void AddOptionalBody(RestRequest request, string name, string? value)
        {
            if (!string.IsNullOrWhiteSpace(value)) request.AddParameter(name, value);
        }

        private static void AddOptionalQuery(RestRequest request, string name, string? value)
        {
            if (!string.IsNullOrWhiteSpace(value)) request.AddQueryParameter(name, value);
        }
    }
}
