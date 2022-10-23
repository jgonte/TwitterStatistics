namespace TwitterStatistics.Model.Statistics
{
    public sealed class TweetStatistics : ITweetStatistics
    {
        private TweetStatistics() { }

        private static TweetStatistics instance = null;

        public static TweetStatistics Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new TweetStatistics();
                }

                return instance;
            }
        }

        public int TotalTweetsReceived { get; set; }

        public List<HashTagStatistics> TopHashtags { get; set; } = new List<HashTagStatistics>();

        public int MinRank { get; set; }
    }
}
