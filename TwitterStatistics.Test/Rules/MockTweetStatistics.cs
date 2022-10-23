using TwitterStatistics.Model.Statistics;

namespace TwitterStatistics.Test.Processors
{
    public class MockTweetStatistics : ITweetStatistics
    {
        public MockTweetStatistics()
        {
        }

        public int TotalTweetsReceived { get; set; }

        public List<HashTagStatistics> TopHashtags { get; set; } = new List<HashTagStatistics>();

        public int MinRank { get; set; }
    }
}