using TwitterStatistics.Model.Statistics;
using TwitterStatistics.Model.Tweets;

namespace TwitterStatistics.Services.Processors
{
    public interface ITweetRule
    {
        ITweetStatistics Process(ITweetStatistics statistics, Tweet tweet);
    }
}