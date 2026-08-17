using System.Threading.Tasks;
using Hitbtc.HitBtcModel;
using Newtonsoft.Json.Linq;

namespace Hitbtc.HitBtcCategories
{
    /// <summary>Authenticated spot commands for HitBTC WebSocket API v3.</summary>
    public class SocketTrading
    {
        private readonly HitBtcSocketApi _api;
        public SocketTrading(HitBtcSocketApi api) { _api = api; }

        public async Task<SocketSubscribe> SubscribeReports(int id = 123)
        {
            return await Send<SocketSubscribe>("spot_subscribe", new JObject(), id);
        }

        public async Task<SocketOrder> NewOrder(string symbolName, string clientOrderId,
            string quantity, string price, int id = 123,
            PublicEnum.EnTradingSide side = PublicEnum.EnTradingSide.buy)
        {
            var parameters = new JObject
            {
                ["symbol"] = symbolName,
                ["client_order_id"] = clientOrderId,
                ["quantity"] = quantity,
                ["side"] = side.ToString()
            };
            AddOptional(parameters, "price", price);
            return await Send<SocketOrder>("spot_new_order", parameters, id);
        }

        public async Task<SocketOrder> CancelOrder(string clientOrderId, int id = 123)
        {
            return await Send<SocketOrder>("spot_cancel_order",
                new JObject { ["client_order_id"] = clientOrderId }, id);
        }

        public async Task<SocketOrderReplace> CancelReplaceOrder(string clientOrderId,
            string requestClientId, string quantity, string price, string strictValidate,
            int id = 123)
        {
            var parameters = new JObject { ["client_order_id"] = clientOrderId };
            AddOptional(parameters, "new_client_order_id", requestClientId);
            AddOptional(parameters, "quantity", quantity);
            AddOptional(parameters, "price", price);
            if (bool.TryParse(strictValidate, out var strict)) parameters["strict_validate"] = strict;
            return await Send<SocketOrderReplace>("spot_replace_order", parameters, id);
        }

        public async Task<SocketOrderReplace> GetActiveOrder(int id = 123)
        {
            return await Send<SocketOrderReplace>("spot_get_orders", new JObject(), id);
        }

        public async Task<SocketBalance> GetTradingBalance(int id = 123)
        {
            return await Send<SocketBalance>("spot_get_trading_balance", new JObject(), id);
        }

        private async Task<T> Send<T>(string method, JObject parameters, int id) where T : class
        {
            var request = new JObject
            {
                ["method"] = method,
                ["params"] = parameters,
                ["id"] = id
            }.ToString(Newtonsoft.Json.Formatting.None);
            var response = await _api.Execute(request);
            return Utilities.ConvertFromJson<T>(response);
        }

        private static void AddOptional(JObject parameters, string name, string value)
        {
            if (!string.IsNullOrWhiteSpace(value)) parameters[name] = value;
        }
    }
}
