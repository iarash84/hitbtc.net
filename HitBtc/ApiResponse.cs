using System;
using System.Collections.Generic;
using Hitbtc.HitBtcModel;

namespace Hitbtc
{
    public class ApiResponse
    {
        public string Content { get; set; }

        public static implicit operator List<Symbol>(ApiResponse response)
        {
            return response == null ? null : Utilities.ConvertListFromJson<Symbol>(response);
        }

        public static implicit operator Symbol(ApiResponse response)
        {
            return response == null ? null : Utilities.ConvertFromJson<Symbol>(response);
        }

        public static implicit operator List<Currency>(ApiResponse response)
        {
            return response == null ? null : Utilities.ConvertListFromJson<Currency>(response);
        }

        public static implicit operator Currency(ApiResponse response)
        {
            return response == null ? null : Utilities.ConvertFromJson<Currency>(response);
        }

        public static implicit operator List<Ticker>(ApiResponse response)
        {
            return response == null ? null : Utilities.ConvertListFromJson<Ticker>(response);
        }

        public static implicit operator Ticker(ApiResponse response)
        {
            return response == null ? null : Utilities.ConvertFromJson<Ticker>(response);
        }

        public static implicit operator Dictionary<string, Ticker>(ApiResponse response)
        {
            return response == null ? null : Utilities.ConvertTickerDictionaryFromJson(response);
        }

        public static implicit operator Orderbook(ApiResponse response)
        {
            return response == null ? null : Utilities.ConvertFromJson<Orderbook>(response);
        }

        public static implicit operator List<Candle>(ApiResponse response)
        {
            return response == null ? null : Utilities.ConvertListFromJson<Candle>(response);
        }

        public static implicit operator List<Balance>(ApiResponse response)
        {
            return response == null ? null : Utilities.ConvertListFromJson<Balance>(response);
        }

        public static implicit operator Fee(ApiResponse response)
        {
            return response == null ? null : Utilities.ConvertFromJson<Fee>(response);
        }

        public static implicit operator List<Order>(ApiResponse response)
        {
            return response == null ? null : Utilities.ConvertListFromJson<Order>(response);
        }

        public static implicit operator Order(ApiResponse response)
        {
            return response == null ? null : Utilities.ConvertFromJson<Order>(response);
        }

        public static implicit operator List<Transaction>(ApiResponse response)
        {
            return response == null ? null : Utilities.ConvertListFromJson<Transaction>(response);
        }

        public static implicit operator Transaction(ApiResponse response)
        {
            return response == null ? null : Utilities.ConvertFromJson<Transaction>(response);
        }

        public static implicit operator IdObject(ApiResponse response)
        {
            return response == null ? null : Utilities.ConvertFromJson<IdObject>(response);
        }

        public static implicit operator AddressModel(ApiResponse response)
        {
            return response == null ? null : Utilities.ConvertFromJson<AddressModel>(response);
        }

        public static implicit operator WithdrawConfirm(ApiResponse response)
        {
            return response == null ? null : Utilities.ConvertFromJson<WithdrawConfirm>(response);
        }

        public static implicit operator List<Trade>(ApiResponse response)
        {
            return response == null ? null : Utilities.ConvertListFromJson<Trade>(response);
        }

        public static implicit operator List<TradeHistory>(ApiResponse response)
        {
            return response == null ? null : Utilities.ConvertListFromJson<TradeHistory>(response);
        }

        public static implicit operator SocketCurrency(ApiResponse response)
        {
            return response == null ? null : Utilities.ConvertFromJson<SocketCurrency>(response);
        }

        public static implicit operator SocketCurrencies(ApiResponse response)
        {
            return response == null ? null : Utilities.ConvertFromJson<SocketCurrencies>(response);
        }

        public static implicit operator SocketSymbol(ApiResponse response)
        {
            return response == null ? null : Utilities.ConvertFromJson<SocketSymbol>(response);
        }

        public static implicit operator SocketSymbols(ApiResponse response)
        {
            return response == null ? null : Utilities.ConvertFromJson<SocketSymbols>(response);
        }

        public static implicit operator SocketTrades(ApiResponse response)
        {
            return response == null ? null : Utilities.ConvertFromJson<SocketTrades>(response);
        }

        public static implicit operator SocketSubscribe(ApiResponse response)
        {
            return response == null ? null : Utilities.ConvertFromJson<SocketSubscribe>(response);
        }

        public static implicit operator SocketOrder(ApiResponse response)
        {
            return response == null ? null : Utilities.ConvertFromJson<SocketOrder>(response);
        }

        public static implicit operator SocketOrderReplace(ApiResponse response)
        {
            return response == null ? null : Utilities.ConvertFromJson<SocketOrderReplace>(response);
        }

        public static implicit operator SocketBalance(ApiResponse response)
        {
            return response == null ? null : Utilities.ConvertFromJson<SocketBalance>(response);
        }
    }
}
