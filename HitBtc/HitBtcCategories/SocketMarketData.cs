using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hitbtc.HitBtcModel;

namespace Hitbtc.HitBtcCategories
{
    public class SocketMarketData
    {
        private readonly HitBtcSocketApi _hitBtcSocketApi;

        public SocketMarketData(HitBtcSocketApi hitBtcSocketApi)
        {
            _hitBtcSocketApi = hitBtcSocketApi;
        }

        public async Task<SocketCurrencies> GetCurrencies(int id = 123)
        {
            var request = string.Format("{{ \"method\": \"getCurrencies\", \"params\": {{  }}, \"id\": {0} }}", id);
            return await _hitBtcSocketApi.Execute(request, false);
        }


        public async Task<SocketCurrency> GetCurrency(string currencyName, int id = 123)
        {
            var request =
                string.Format(
                    "{{ \"method\": \"getCurrency\", \"params\": {{ \"currency\": \"{0}\" }}, \"id\": {1} }}",
                    currencyName, id);
            return await _hitBtcSocketApi.Execute(request, false);
        }

        public async Task<SocketSymbols> GetSymbols( int id = 123)
        {
            var request = string.Format("{{ \"method\": \"getSymbols\", \"params\": {{  }}, \"id\": {0} }}", id);
            return await _hitBtcSocketApi.Execute(request, false);
        }

        public async Task<SocketSymbol> GetSymbol(string symbol, int id = 123)
        {
            var request =
                string.Format("{{ \"method\": \"getSymbol\", \"params\": {{ \"symbol\": \"{0}\" }}, \"id\": {1} }}",
                    symbol, id);
            return await _hitBtcSocketApi.Execute(request, false);
        }

        public async Task<SocketSubscribe> SubscribeTicker(string symbol, int id = 123)
        {
            var request =
                string.Format(
                    "{{ \"method\": \"subscribeTicker\", \"params\": {{ \"symbol\": \"{0}\" }}, \"id\": {1} }}", 
                    symbol, id);
            return await _hitBtcSocketApi.Execute(request, false);
        }

        public async Task<SocketSubscribe> UnsubscribeTicker(string symbol, int id = 123)
        {
            var request =
                string.Format(
                    "{{ \"method\": \"unsubscribeTicker\", \"params\": {{ \"symbol\": \"{0}\" }}, \"id\": {1} }}",
                    symbol, id);
            return await _hitBtcSocketApi.Execute(request, false);
        }

        public async Task<SocketSubscribe> SubscribeOrderbook(string symbol, int id = 123)
        {
            var request =
                string.Format(
                    "{{ \"method\": \"subscribeOrderbook\", \"params\": {{ \"symbol\": \"{0}\" }}, \"id\": {1} }}",
                    symbol, id);
            return await _hitBtcSocketApi.Execute(request, false);
        }

        public async Task<SocketSubscribe> UnsubscribeOrderbook(string symbol, int id = 123)
        {
            var request =
                string.Format(
                    "{{ \"method\": \"unsubscribeOrderbook\", \"params\": {{ \"symbol\": \"{0}\" }}, \"id\": {1} }}",
                    symbol, id);
            return await _hitBtcSocketApi.Execute(request, false);
        }

        public async Task<SocketSubscribe> SubscribeTrades(string symbol, int id = 123)
        {
            var request =
                string.Format(
                    "{{ \"method\": \"subscribeTrades\", \"params\": {{ \"symbol\": \"{0}\" }}, \"id\": {1} }}", symbol,
                    id);
            return await _hitBtcSocketApi.Execute(request, false);
        }

        public async Task<SocketSubscribe> UnsubscribeTrades(string symbol, int id = 123)
        {
            var request =
                string.Format(
                    "{{ \"method\": \"unsubscribeTrades\", \"params\": {{ \"symbol\": \"{0}\" }}, \"id\": {1} }}",
                    symbol, id);
            return await _hitBtcSocketApi.Execute(request, false);
        }


        public async Task<SocketSubscribe> SubscribeCandles(string symbol, PublicEnum.EnPeriod enPeriod = PublicEnum.EnPeriod.M30, int id = 123)
        {
            var period = enPeriod.ToString() == "Month" ? "1M" : enPeriod.ToString();
            var request =
                string.Format(
                    "{{ \"method\": \"subscribeCandles\", \"params\": {{ \"symbol\": \"{0}\" , \"period\": \"{1}\" }}, \"id\": {2} }}",
                    symbol, period, id);
            return await _hitBtcSocketApi.Execute(request, false);
        }

        public async Task<SocketSubscribe> UnsubscribeCandles(string symbol, PublicEnum.EnPeriod enPeriod = PublicEnum.EnPeriod.M30, int id = 123)
        {
            var period = enPeriod.ToString() == "Month" ? "1M" : enPeriod.ToString();
            var request =
                string.Format(
                    "{{ \"method\": \"unsubscribeCandles\", \"params\": {{ \"symbol\": \"{0}\" , \"period\": \"{1}\"}}, \"id\": {2} }}",
                    symbol, period, id);
            return await _hitBtcSocketApi.Execute(request, false);
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
        /// <param name="id"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public async Task<SocketTrades> GetTrades(string symoblName, string from, string till, int offset, int id = 123,
            int limit = 100,
            PublicEnum.EnSort sort = PublicEnum.EnSort.Desc, PublicEnum.EnBy by = PublicEnum.EnBy.timestamp)
        {

            string[] paramterArray = new string[7];
            if (!string.IsNullOrEmpty(symoblName))
                paramterArray[0] = string.Format("\"symbol\": \"{0}\"", symoblName);
            paramterArray[1] = string.Format("\"sort\": \"{0}\"", sort.ToString());
            paramterArray[2] = string.Format("\"by\": \"{0}\"", by.ToString());
            if (!string.IsNullOrEmpty(from))
                paramterArray[3] = string.Format("\"from\": \"{0}\"", from);
            if (!string.IsNullOrEmpty(till))
                paramterArray[4] = string.Format("\"till\": \"{0}\"", till);
            if (limit > 0)
                paramterArray[5] = string.Format("\"limit\": \"{0}\"", limit.ToString());
            if (offset > 0)
                paramterArray[6] = string.Format("\"offset\": \"{0}\"", offset.ToString());
            string parameters = string.Empty;

            for (var index = 0; index < paramterArray.Length - 1; index++)
            {
                if (!string.IsNullOrEmpty(paramterArray[index]))
                    parameters += paramterArray[index] + " , ";
            }

            if (!string.IsNullOrEmpty(paramterArray[6]))
                parameters += paramterArray[6];

            var request =
                string.Format("{{ \"method\": \"getTrades\", \"params\": {{ {0} }}, \"id\": {1} }}",
                parameters, id);
            return await _hitBtcSocketApi.Execute(request, false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ask"></param>
        /// <param name="bid"></param>
        /// <param name="last"></param>
        /// <param name="open"></param>
        /// <param name="low"></param>
        /// <param name="high"></param>
        /// <param name="volume"></param>
        /// <param name="volumeQuote"></param>
        /// <param name="timestamp"></param>
        /// <param name="symbol"></param>
        public async void NotificationTicker(string ask, string bid, string last, string open, string low, string high,
            string volume, string volumeQuote, string timestamp, string symbol)
        {
            var request =
                string.Format(
                    "{{\"jsonrpc\":\"2.0\",\"method\":\"ticker\",\"params\":{{\"ask\":\"{0}\",\"bid\":\"{1}\",\"last\":\"{2}\",\"open\":\"{3}3\",\"low\":\"{4}\",\"high\":\"{5}\",\"volume\":\"{6}\",\"volumeQuote\":\"{7}\",\"timestamp\":\"{8}\",\"symbol\":\"{9}\"}}}}",
                    ask, bid, last, open, low, high, volume, volumeQuote, timestamp, symbol);
            await _hitBtcSocketApi.Execute(request);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="sequence"></param>
        /// <param name="asks"></param>
        /// <param name="bids"></param>
        public async void NotificationSnapshotOrderbook(string symbol, string sequence, List<OrderBookParamter> asks,
            List<OrderBookParamter> bids)
        {
            string askParamter = string.Empty;
            string bidParamter = string.Empty;

            askParamter = asks.Aggregate(askParamter,
                (current, ask) =>
                    current + string.Format(" {{\"price\":\"{0}\",\"size\":\"{1}\"}},", ask.price, ask.size));

            askParamter = bids.Aggregate(askParamter,
                (current, bid) =>
                    current + string.Format(" {{\"price\":\"{0}\",\"size\":\"{1}\"}},", bid.price, bid.size));

            askParamter = askParamter.TrimEnd(',');
            bidParamter = bidParamter.TrimEnd(',');
            var request =
                string.Format(
                    "{{\"jsonrpc\":\"2.0\",\"method\":\"snapshotOrderbook\",\"params\":{{\"ask\":[{0}],\"bid\":[{1}],\"symbol\":\"{2}\",\"sequence\":{3}}}}}",
                    askParamter, bidParamter, symbol, sequence);
            await _hitBtcSocketApi.Execute(request);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="sequence"></param>
        /// <param name="asks"></param>
        /// <param name="bids"></param>
        public async void NotificationUpdateOrderbook(string symbol, string sequence, List<OrderBookParamter> asks,
            List<OrderBookParamter> bids)
        {
            string askParamter = string.Empty;
            string bidParamter = string.Empty;

            askParamter = asks.Aggregate(askParamter,
                (current, ask) =>
                    current + string.Format(" {{\"price\":\"{0}\",\"size\":\"{1}\"}},", ask.price, ask.size));

            askParamter = bids.Aggregate(askParamter,
                (current, bid) =>
                    current + string.Format(" {{\"price\":\"{0}\",\"size\":\"{1}\"}},", bid.price, bid.size));

            askParamter = askParamter.TrimEnd(',');
            bidParamter = bidParamter.TrimEnd(',');
            var request =
                string.Format(
                    "{{\"jsonrpc\":\"2.0\",\"method\":\"updateOrderbook\",\"params\":{{\"ask\":[{0}],\"bid\":[{1}],\"symbol\":\"{2}\",\"sequence\":{3}}}}}",
                    askParamter, bidParamter, symbol, sequence);
            await _hitBtcSocketApi.Execute(request);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="data"></param>
        public async void NotificationSnapshotTrades(string symbol, List<SocketTrade> data)
        {
            string dataParameters = data.Aggregate(string.Empty,
                (current, parameter) =>
                    current +
                    string.Format(
                        " {{\"id\":{0},\"price\":\"{1}\",\"quantity\":\"{2}\",\"side\":\"{3}\",\"timestamp\":\"{4}\"}},",
                        parameter.id, parameter.price, parameter.quantity, parameter.side, parameter.timestamp));

            dataParameters = dataParameters.TrimEnd(',');
            var request =
                string.Format(
                    "{{\"jsonrpc\":\"2.0\",\"method\":\"snapshotTrades\",\"params\":{{\"data\":[{0}],\"symbol\":\"{1}\"}}}}",
                    dataParameters, symbol);
            await _hitBtcSocketApi.Execute(request);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="data"></param>
        public async void NotificationUpdateTrades(string symbol, List<SocketTrade> data)
        {
            
            string dataParameters = data.Aggregate(string.Empty,
                (current, parameter) =>
                    current +
                    string.Format(
                        " {{\"id\":{0},\"price\":\"{1}\",\"quantity\":\"{2}\",\"side\":\"{3}\",\"timestamp\":\"{4}\"}},",
                        parameter.id, parameter.price, parameter.quantity, parameter.side, parameter.timestamp));

            dataParameters = dataParameters.TrimEnd(',');
            var request =
                string.Format(
                    "{{\"jsonrpc\":\"2.0\",\"method\":\"updateTrades\",\"params\":{{\"data\":[{0}],\"symbol\":\"{1}\"}}}}",
                    dataParameters, symbol);
            await _hitBtcSocketApi.Execute(request);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="enPeriod"></param>
        /// <param name="data"></param>
        public async void NotificationSnapshotCandles(string symbol, PublicEnum.EnPeriod enPeriod, List<Candle> data)
        {
            string dataParameters = data.Aggregate(string.Empty,
                (current, parameter) =>
                    current +
                    string.Format(
                        " {{\"timestamp\":\"{0}\",\"open\":\"{1}\",\"close\":\"{2}\",\"min\":\"{3}\",\"max\":\"{4}\",\"volume\":\"{5}\",\"volumeQuote\":\"{6}\"}},",
                        parameter.timestamp, parameter.open, parameter.close, parameter.min, parameter.max,
                        parameter.volume, parameter.volumeQuote));

            dataParameters = dataParameters.TrimEnd(',');
            var period = enPeriod.ToString() == "Month" ? "1M" : enPeriod.ToString();
            var request =
                string.Format(
                    "{{\"jsonrpc\":\"2.0\",\"method\":\"snapshotCandles\",\"params\":{{\"data\":[{0}],\"symbol\":\"{1}\",\"period\":\"{2}\"}}}}",
                    dataParameters, symbol, period);
            await _hitBtcSocketApi.Execute(request);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="enPeriod"></param>
        /// <param name="data"></param>
        public async void NotificationUpdateCandles(string symbol, PublicEnum.EnPeriod enPeriod, List<Candle> data)
        {
            string dataParameters = data.Aggregate(string.Empty,
                (current, parameter) =>
                    current +
                    string.Format(
                        " {{\"timestamp\":\"{0}\",\"open\":\"{1}\",\"close\":\"{2}\",\"min\":\"{3}\",\"max\":\"{4}\",\"volume\":\"{5}\",\"volumeQuote\":\"{6}\"}},",
                        parameter.timestamp, parameter.open, parameter.close, parameter.min, parameter.max,
                        parameter.volume, parameter.volumeQuote));

            dataParameters = dataParameters.TrimEnd(',');
            var period = enPeriod.ToString() == "Month" ? "1M" : enPeriod.ToString();
            var request =
                string.Format(
                    "{{\"jsonrpc\":\"2.0\",\"method\":\"updateCandles\",\"params\":{{\"data\":[{0}],\"symbol\":\"{1}\",\"period\":\"{2}\"}}}}",
                    dataParameters, symbol, period);
            await _hitBtcSocketApi.Execute(request);
        }

    }
}

