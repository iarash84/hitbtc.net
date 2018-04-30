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
            return response == null ? null : Utilities.ConverFromJasons<Symbol>(response);
        }

        public static implicit operator Symbol(ApiResponse response)
        {
            return response == null ? null : Utilities.ConverFromJason<Symbol>(response);
        }

        public static implicit operator List<Currency>(ApiResponse response)
        {
            return response == null ? null : Utilities.ConverFromJasons<Currency>(response);
        }

        public static implicit operator Currency(ApiResponse response)
        {
            return response == null ? null : Utilities.ConverFromJason<Currency>(response);
        }

        public static implicit operator List<Ticker>(ApiResponse response)
        {
            return response == null ? null : Utilities.ConverFromJasons<Ticker>(response);
        }

        public static implicit operator Ticker(ApiResponse response)
        {
            return response == null ? null : Utilities.ConverFromJason<Ticker>(response);
        }

        public static implicit operator Dictionary<string, Ticker>(ApiResponse response)
        {
            return response == null ? null : Utilities.ConverFromJasonArray(response);
        }

        public static implicit operator Orderbook(ApiResponse response)
        {
            return response == null ? null : Utilities.ConverFromJason<Orderbook>(response);
        }

        public static implicit operator List<Candle>(ApiResponse response)
        {
            return response == null ? null : Utilities.ConverFromJasons<Candle>(response);
        }

        public static implicit operator List<Balance>(ApiResponse response)
        {
            return response == null ? null : Utilities.ConverFromJasons<Balance>(response);
        }

        public static implicit operator Fee(ApiResponse response)
        {
            return response == null ? null : Utilities.ConverFromJason<Fee>(response);
        }

        public static implicit operator List<Order>(ApiResponse response)
        {
            return response == null ? null : Utilities.ConverFromJasons<Order>(response);
        }

        public static implicit operator Order(ApiResponse response)
        {
            return response == null ? null : Utilities.ConverFromJason<Order>(response);
        }

        public static implicit operator List<Transaction>(ApiResponse response)
        {
            if (response != null)
            {
                if (response.Content.Contains("message"))
                {
                    var error = Utilities.ConverFromJason<Error>(response);
                    throw new Exception(error.ToString());
                }
                return Utilities.ConverFromJasons<Transaction>(response);
            }
            return null;
        }

        public static implicit operator Transaction(ApiResponse response)
        {
            return response == null ? null : Utilities.ConverFromJason<Transaction>(response);
        }

        public static implicit operator IdObject(ApiResponse response)
        {
            return response == null ? null : Utilities.ConverFromJason<IdObject>(response);
        }

        public static implicit operator AddressModel(ApiResponse response)
        {
            return response == null ? null : Utilities.ConverFromJason<AddressModel>(response);
        }

        public static implicit operator WithdrawConfirm(ApiResponse response)
        {
            return response == null ? null : Utilities.ConverFromJason<WithdrawConfirm>(response);
        }

        public static implicit operator List<Trade>(ApiResponse response)
        {
            return response == null ? null : Utilities.ConverFromJasons<Trade>(response);
        }

        public static implicit operator List<TradeHistory>(ApiResponse response)
        {
            return response == null ? null : Utilities.ConverFromJasons<TradeHistory>(response);
        }

        public static implicit operator SocketCurrency(ApiResponse response)
        {
            return response == null ? null : Utilities.ConverFromJason<SocketCurrency>(response);
        }

        public static implicit operator SocketCurrencies(ApiResponse response)
        {
            return response == null ? null : Utilities.ConverFromJason<SocketCurrencies>(response);
        }

        public static implicit operator SocketSymbol(ApiResponse response)
        {
            return response == null ? null : Utilities.ConverFromJason<SocketSymbol>(response);
        }

        public static implicit operator SocketSymbols(ApiResponse response)
        {
            return response == null ? null : Utilities.ConverFromJason<SocketSymbols>(response);
        }

        public static implicit operator SocketTrades(ApiResponse response)
        {
            return response == null ? null : Utilities.ConverFromJason<SocketTrades>(response);
        }

        public static implicit operator SocketSubscribe(ApiResponse response)
        {
            return response == null ? null : Utilities.ConverFromJason<SocketSubscribe>(response);
        }

        public static implicit operator SocketOrder(ApiResponse response)
        {
            return response == null ? null : Utilities.ConverFromJason<SocketOrder>(response);
        }

        public static implicit operator SocketOrderReplace(ApiResponse response)
        {
            return response == null ? null : Utilities.ConverFromJason<SocketOrderReplace>(response);
        }

        public static implicit operator SocketBalance(ApiResponse response)
        {
            return response == null ? null : Utilities.ConverFromJason<SocketBalance>(response);
        }
    }
}
