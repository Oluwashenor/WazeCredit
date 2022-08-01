namespace WazeCredit.Models.Service
{
    public class MarketForecaster
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

    public class MarketResult
    {
        public MarketCondition MarketCondition { get; set; }
    }
}
