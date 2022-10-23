using TwitterStatistics.Model.Statistics;
using TwitterStatistics.Model.Tweets;

namespace TwitterStatistics.Services.Processors
{
    public class TopTenHashTagsTweetRule : ITweetRule
    {
        IRankCalculator _rankCalculator;

        public TopTenHashTagsTweetRule(IRankCalculator rankCalculator)
        {
            _rankCalculator = rankCalculator;
        }

        public ITweetStatistics Process(ITweetStatistics statistics, Tweet tweet)
        {
            if (!tweet.Entities.Hashtags.Any())
            {
                return statistics; // Nothing to rank
            }

            var rank = _rankCalculator.CalculateRank(tweet.PublicMetrics);

            var previousTags = statistics.TopHashtags;

            if (previousTags.Count >= 10 && // The list is complste
                statistics.MinRank >= rank)
            {
                return statistics;
            }

            // Add the tags to the list
            var tags = tweet.Entities.Hashtags.Select(x => x.Tag).ToList();

            

            foreach (var tag in tags)
            {
                previousTags.Add(new HashTagStatistics
                {
                    Tag = tag,
                    Rank = rank
                });
            }

            statistics.TopHashtags = previousTags
                .OrderByDescending(t => t.Rank)
                .Take(10)
                .ToList();

            statistics.MinRank = statistics.TopHashtags.Min(t => t.Rank);

            return statistics;
        }
    }
}
