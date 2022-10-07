using System;
using System.Collections.Generic;
using System.Text;

namespace SmartCryptoManager.Models
{
    public class CoinCapInfo
    {
        public string Name { get; set; }
        public string Symbol { get; set; }
        public double MarketCap { get; set; }
        public string MarketCapDisplay => MarketCap.ToString();
        public double MarketCapDominanceRatio(double highestMarketCapValue)
        {
            return MarketCap / highestMarketCapValue;
        }
    }
}
