namespace TwitterStatistics.Model.Statistics
{
    public interface ITweetStatistics
    {
        public int TotalTweetsReceived { get; set; }

        public List<HashTagStatistics> TopHashtags { get; set; }

        /// <summary>
        /// The minimum rank of the top hash tags, so if the new rank is less than the minimum one, no attempt is made to update the hashtags
        /// </summary>
        int MinRank { get; set; }
    }
}