using TwitterStatistics.Model.Statistics;
using TwitterStatistics.Model.Tweets;

namespace TwitterStatistics.Services.Processors
{
    public class TotalReceivedTweetRule : ITweetRule
    {
        public ITweetStatistics Process(ITweetStatistics statistics, Tweet tweet)
        {
            ++statistics.TotalTweetsReceived;

            return statistics;
        }
    }
}
