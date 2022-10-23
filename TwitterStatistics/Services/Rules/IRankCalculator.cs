using TwitterStatistics.Model.Tweets;

namespace TwitterStatistics.Services.Processors
{
    public interface IRankCalculator
    {
        int CalculateRank(PublicMetrics publicMetrics);
    }
}