using WazeCredit.Models;

namespace WazeCredit.Service
{
    public class MarketForecaster : IMarketForecaster
    {
        public MarketResult GetMarketPrediction()
        {
            // Call api to do come complex calculations and current stock market forecast.
            return new MarketResult
            {
                MarketCondition = MarketCondition.StableUp
            };
        }
    }

}
