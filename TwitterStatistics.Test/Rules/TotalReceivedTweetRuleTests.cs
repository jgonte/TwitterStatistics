using TwitterStatistics.Model.Statistics;
using TwitterStatistics.Model.Tweets;
using TwitterStatistics.Services.Processors;

namespace TwitterStatistics.Test.Processors
{
    [TestClass]
    public class TotalReceivedTweetRuleTests
    {
        [TestMethod]
        public void TotalTweetCountRule_It_Should_Increment_The_Total_Of_Tweets_Received()
        {
            var tweet = new Tweet();

            ITweetStatistics mock = new MockTweetStatistics();

            var processor = new TotalReceivedTweetRule();

            mock = processor.Process(mock, tweet);

            Assert.AreEqual(1, mock.TotalTweetsReceived);

            mock = processor.Process(mock, tweet);

            Assert.AreEqual(2, mock.TotalTweetsReceived);
        }
    }
}
