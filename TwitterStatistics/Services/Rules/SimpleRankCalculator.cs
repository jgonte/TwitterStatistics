using TwitterStatistics.Model.Tweets;

namespace TwitterStatistics.Services.Processors
{
    public class SimpleRankCalculator : IRankCalculator
    {
        public int CalculateRank(PublicMetrics publicMetrics)
        {
            return publicMetrics.LikeCount;
        }
    }
}
