namespace Hitbtc
{
    public static class PublicEnum
    {

        public enum EnSort
        {
            Asc,
            Desc
        }

        public enum EnBy
        {
            timestamp,
            id
        }

        public enum EnTransferType
        {
            bankToExchange,
            exchangeToBank
        }

        public enum EnTradingSide
        {
            sell,
            buy
        }

        public enum EnTradingTimeInForce
        {
            GTC,
            IOC,
            FOK,
            Day,
            GTD
        }

        public enum EnTradingType
        {
            limit,
            market,
            stopLimit,
            stopMarket
        }

        public enum EnPeriod
        {
            M1,
            M3,
            M5,
            M15,
            M30,
            H1,
            H4,
            D1,
            D7,
            Month
        }

        public enum EnStatus
        {
            New,
            Suspended,
            PartiallyFilled,
            Filled,
            Canceled,
            Expired
        }

        public enum EnReportType
        {
            Status,
            New,
            Canceled,
            Expired,
            Suspended,
            Trade,
            Replaced
        }

    }
}
