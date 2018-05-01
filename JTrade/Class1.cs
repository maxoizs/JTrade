using System;
using System.Collections.Generic;
namespace JTrade
{
    public abstract class Stock : ICalculateStock
    {
        public string Symbol { get; private set; }
        public StockType Type { get; private set; }
        public decimal LastDividend { get; private set; }
        public double FixedDividend { get; private set; }
        public int ParValue { get; private set; }

        public List<Trade> Trades { get; private set; }
        public Stock(string symbol, StockType type, decimal lastDividend, double fixedDividend, int parValue)
        {
            Symbol = symbol;
            Type = type;
            LastDividend = lastDividend;
            FixedDividend = fixedDividend;
            ParValue = parValue;
        }

        public decimal CalculateDividendYild(decimal price) { throw new NotImplementedException(); }
        public decimal CalculatePERatio(decimal price)
        {
            if (price == 0)
            {
                return 0;
            }
            if (LastDividend == 0)
            {
                return 0;
            }
            return price / LastDividend; // I've assumed this is the dividend, sorry I'm not financial expert
        }
        public double VolumeWeightedStockPrice(decimal price) { throw new NotImplementedException(); }
        public bool RecordTrade(int quantity, TradeType tradeType, decimal price)
        {
            throw new NotImplementedException();
        }
    }

    public class Trade
    {
        public DateTime TimeStamp { get; private set; }
        public int Quantity { get; private set; }
        public TradeType Type { get; private set; }
        public Trade(DateTime timeStamp, int quantity, TradeType tradeType)
        {
            TimeStamp = timeStamp;
            Quantity = quantity;
            Type = tradeType;
        }
    }

    public interface ICalculateStock
    {
        decimal CalculateDividendYild(decimal price);
        decimal CalculatePERatio(decimal price);
        double VolumeWeightedStockPrice(decimal price);
        bool RecordTrade(int quantity, TradeType tradeType, decimal price);
    }

    public class CommonStock : Stock
    {
        public CommonStock(string symbol, decimal lastDividend, double fixDividend, int parValue) :
            base(symbol, StockType.Common, lastDividend, fixDividend, parValue)
        {
        }

        public new decimal CalculateDividendYild(decimal price)
        {
            if (LastDividend == 0)
            {
                return 0;
            }
            if (price == 0)
            {
                return 0;
            }
            return LastDividend / price;
        }
    }

    public class PreferredStock : Stock
    {
        public PreferredStock(string symbol, decimal lastDividend, double fixDividend, int parValue) :
            base(symbol, StockType.Preferred, lastDividend, fixDividend, parValue)
        {
        }
        public new decimal CalculateDividendYild(decimal price)
        {
            if (FixedDividend == 0)
            {
                return 0;
            }
            if (ParValue == 0)
            {
                return 0;
            }
            if (price == 0)
            {
                return 0;
            }
            return LastDividend * ParValue / price;
        }
    }


    public enum StockType
    {
        Common,
        Preferred
    }

    public enum TradeType
    {
        Buy,
        Sell
    }
}
