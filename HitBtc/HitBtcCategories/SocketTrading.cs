using System.Collections.Generic;
using System.Threading.Tasks;
using Hitbtc.HitBtcModel;

namespace Hitbtc.HitBtcCategories
{
    public class SocketTrading
    {
        private readonly HitBtcSocketApi _hitBtcSocketApi;

        public SocketTrading(HitBtcSocketApi hitBtcSocketApi)
        {
            _hitBtcSocketApi = hitBtcSocketApi;
        }


        public async Task<SocketSubscribe> SubscribeReports(int id = 123)
        {
            var request = "{ \"method\": \"subscribeReports\", \"params\": {  } }";
            return await _hitBtcSocketApi.Execute(request, false);
        }

        public async Task<SocketOrder> NewOrder(string symbolName, string clientOrderId, string quantity, string price,
            int id = 123, PublicEnum.EnTradingSide side = PublicEnum.EnTradingSide.buy)
        {
            string[] paramterArray = new string[5];
            if (!string.IsNullOrEmpty(symbolName))
                paramterArray[0] = string.Format("\"symbol\": \"{0}\"", symbolName);
            paramterArray[1] = string.Format("\"clientOrderId\": \"{0}\"", clientOrderId);
            if (!string.IsNullOrEmpty(quantity))
                paramterArray[2] = string.Format("\"quantity\": \"{0}\"", quantity);
            if (!string.IsNullOrEmpty(price))
                paramterArray[3] = string.Format("\"price\": \"{0}\"", price);
            paramterArray[4] = string.Format("\"side\": \"{0}\"", side.ToString());

            string parameters = string.Empty;

            for (var index = 0; index < paramterArray.Length - 1; index++)
            {
                if (!string.IsNullOrEmpty(paramterArray[index]))
                    parameters += paramterArray[index] + " , ";
            }

            if (!string.IsNullOrEmpty(paramterArray[4]))
                parameters += paramterArray[4];


            var request =
                      string.Format("{{ \"method\": \"newOrder\", \"params\": {{ {0} }}, \"id\": {1} }}",
                      parameters, id);
            return await _hitBtcSocketApi.Execute(request);
        }

        public async Task<SocketOrder> CancelOrder(string clientOrderId, int id = 123)
        {
            var request =
                string.Format(
                    "{{ \"method\": \"cancelOrder\", \"params\": {{ \"clientOrderId\": \"{0}\" }}, \"id\": {1} }}",
                    clientOrderId, id);
            return await _hitBtcSocketApi.Execute(request);
        }

        public async Task<SocketOrderReplace> CancelReplaceOrder(string clientOrderId, string requestClientId, string quantity,
            string price, string strictValidate, int id = 123)
        {

            string[] paramterArray = new string[5];

            paramterArray[0] = string.Format("\"clientOrderId\": \"{0}\"", clientOrderId);
            if (!string.IsNullOrEmpty(requestClientId))
                paramterArray[1] = string.Format("\"requestClientId\": \"{0}\"", requestClientId);
            if (!string.IsNullOrEmpty(quantity))
                paramterArray[2] = string.Format("\"quantity\": \"{0}\"", quantity);
            if (!string.IsNullOrEmpty(price))
                paramterArray[3] = string.Format("\"price\": \"{0}\"", price);
            if (!string.IsNullOrEmpty(strictValidate))
                paramterArray[4] = string.Format("\"strictValidate\": \"{0}\"", strictValidate);

            string parameters = string.Empty;

            for (var index = 0; index < paramterArray.Length - 1; index++)
            {
                if (!string.IsNullOrEmpty(paramterArray[index]))
                    parameters += paramterArray[index] + " , ";
            }

            if (!string.IsNullOrEmpty(paramterArray[4]))
                parameters += paramterArray[4];

            var request = string.Format("{{ \"method\": \"cancelReplaceOrder\", \"params\": {{ {0} }}, \"id\": {1} }}",
                parameters, id);
            return await _hitBtcSocketApi.Execute(request);
        }


        public async Task<SocketOrderReplace> GetActiveOrder(int id = 123)
        {
            var request =
                string.Format("{{ \"method\": \"getOrders\", \"params\": {{ }}, \"id\": {0} }}",id);
            return await _hitBtcSocketApi.Execute(request);
        }

        public async Task<SocketBalance> GetTradingBalance(int id = 123)
        {
            var request =
                string.Format("{{ \"method\": \"getTradingBalance\", \"params\": {{ }}, \"id\": {0} }}", id);
            return await _hitBtcSocketApi.Execute(request);
        }


        public async void NotificationActiveOrders(string id, string clientOrderId, string symbol,
            PublicEnum.EnTradingSide side,
            PublicEnum.EnStatus status,
            PublicEnum.EnTradingType type, PublicEnum.EnTradingTimeInForce timeInForce, string quantity, string price,
            string cumQuantity, string createdAt,
            string updatedAt,
            PublicEnum.EnReportType reportType)
        {
            var request =
                string.Format(
                    "{{\"jsonrpc\":\"2.0\",\"method\":\"activeOrders\",\"params\":[{{\"id\":\"{0}\",\"clientOrderId\":\"{1}\",\"symbol\":\"{2}\",\"side\":\"{3}\",\"status\":\"{4}\",\"type\":\"{5}\",\"timeInForce\":\"{6}\",\"quantity\":\"{7}\",\"price\":\"{8}\",\"cumQuantity\":\"{9}\",\"createdAt\":\"{10}\",\"updatedAt\":\"{11}\",\"reportType\":\"{12}\"}}]}}",
                    id, clientOrderId, symbol, side, Utilities.FirstCharToLower(status.ToString()),
                    Utilities.FirstCharToLower(type.ToString()), timeInForce, quantity, price, cumQuantity, createdAt,
                    updatedAt, reportType);
            await _hitBtcSocketApi.Execute(request);
        }

        public async void NotificationReport(string id, string clientOrderId, string symbol,
            PublicEnum.EnTradingSide side,
            PublicEnum.EnStatus status,
            PublicEnum.EnTradingType type, PublicEnum.EnTradingTimeInForce timeInForce, string quantity, string price,
            string cumQuantity, string createdAt,
            string updatedAt,
            PublicEnum.EnReportType reportType, string tradeQuantity, string tradePrice, string tradeId,
            string tradeFee)
        {
            var request =
                string.Format(
                    "{{\"jsonrpc\":\"2.0\",\"method\":\"report\",\"params\":{{\"id\":\"{0}\",\"clientOrderId\":\"{1}\",\"symbol\":\"{2}\",\"side\":\"{3}\",\"status\":\"{4}\",\"type\":\"{5}\",\"timeInForce\":\"{6}\",\"quantity\":\"{7}\",\"price\":\"{8}\",\"cumQuantity\":\"{9}\",\"createdAt\":\"{10}\",\"updatedAt\":\"{11}\",\"reportType\":\"{12}\",\"tradeQuantity\":\"{13}\",\"tradePrice\":\"{14}\",\"tradeId\":{15},\"tradeFee\":\"{16}\"}}}}",
                    id, clientOrderId, symbol, side, Utilities.FirstCharToLower(status.ToString()),
                    Utilities.FirstCharToLower(type.ToString()), timeInForce, quantity, price, cumQuantity, createdAt,
                    updatedAt, reportType, tradeQuantity, tradePrice, tradeId, tradeFee);
            await _hitBtcSocketApi.Execute(request);
        }

    }
}
