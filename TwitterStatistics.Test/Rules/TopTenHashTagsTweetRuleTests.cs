using TwitterStatistics.Model.Statistics;
using TwitterStatistics.Model.Tweets;
using TwitterStatistics.Services.Processors;

namespace TwitterStatistics.Test.Processors
{
    [TestClass]
    public class TopTenHashTagsTweetRuleTests
    {
        [TestMethod]
        public void TopTenHashTagsTweetRule_It_Should_Keep_The_HashTags_With_The_HIgher_Ranks()
        {
            ITweetStatistics mock = new MockTweetStatistics();

            var rankCalculator = new SimpleRankCalculator();

            var processor = new TopTenHashTagsTweetRule(rankCalculator);

            var tweet = new Tweet
            {
                Entities = new Entities
                {
                    Hashtags = new List<HashTag>
                    {
                        new HashTag
                        {
                            Tag = "A"
                        },
                        new HashTag
                        {
                            Tag = "B"
                        },
                        new HashTag
                        {
                            Tag = "C"
                        },
                        new HashTag
                        {
                            Tag = "D"
                        },
                        new HashTag
                        {
                            Tag = "E"
                        },
                    }
                },
                PublicMetrics = new PublicMetrics
                {
                    LikeCount = 3,
                }
            };

            mock = processor.Process(mock, tweet);

            Assert.AreEqual(5, mock.TopHashtags.Count);

            var hashTag = mock.TopHashtags[0];

            Assert.AreEqual("A", hashTag.Tag);

            Assert.AreEqual(3, hashTag.Rank);

            hashTag = mock.TopHashtags[mock.TopHashtags.Count - 1]; // Check the last one

            Assert.AreEqual("E", hashTag.Tag);

            Assert.AreEqual(3, hashTag.Rank);

            Assert.AreEqual(3, mock.MinRank);

            // Add tags with less rank (the list count is still less than 10)
            tweet = new Tweet
            {
                Entities = new Entities
                {
                    Hashtags = new List<HashTag>
                    {
                        
                        new HashTag
                        {
                            Tag = "F"
                        },
                        new HashTag
                        {
                            Tag = "G"
                        },
                        new HashTag
                        {
                            Tag = "H"
                        },
                        new HashTag
                        {
                            Tag = "I"
                        },
                        new HashTag
                        {
                            Tag = "K"
                        }
                    }
                },
                PublicMetrics = new PublicMetrics
                {
                    LikeCount = 2,
                }
            };

            mock = processor.Process(mock, tweet);

            Assert.AreEqual(10, mock.TopHashtags.Count);

            hashTag = mock.TopHashtags[0];

            Assert.AreEqual("A", hashTag.Tag);

            Assert.AreEqual(3, hashTag.Rank);

            hashTag = mock.TopHashtags[mock.TopHashtags.Count - 1]; // Check the last one

            Assert.AreEqual("K", hashTag.Tag);

            Assert.AreEqual(2, hashTag.Rank);

            Assert.AreEqual(2, mock.MinRank);

            // Try to add tags with the minimum rank, they should not be added
            tweet = new Tweet
            {
                Entities = new Entities
                {
                    Hashtags = new List<HashTag>
                    {

                        new HashTag
                        {
                            Tag = "1"
                        },
                        new HashTag
                        {
                            Tag = "2"
                        }
                    }
                },
                PublicMetrics = new PublicMetrics
                {
                    LikeCount = 2,
                }
            };

            mock = processor.Process(mock, tweet);

            Assert.AreEqual(10, mock.TopHashtags.Count);

            hashTag = mock.TopHashtags[0];

            Assert.AreEqual("A", hashTag.Tag);

            Assert.AreEqual(3, hashTag.Rank);

            hashTag = mock.TopHashtags[mock.TopHashtags.Count - 1]; // Check the last one

            Assert.AreEqual("K", hashTag.Tag);

            Assert.AreEqual(2, hashTag.Rank);

            Assert.AreEqual(2, mock.MinRank);

            // Add tags with a higher rank

            tweet = new Tweet
            {
                Entities = new Entities
                {
                    Hashtags = new List<HashTag>
                    {

                        new HashTag
                        {
                            Tag = "10"
                        },
                        new HashTag
                        {
                            Tag = "20"
                        }
                    }
                },
                PublicMetrics = new PublicMetrics
                {
                    LikeCount = 4,
                }
            };

            mock = processor.Process(mock, tweet);

            Assert.AreEqual(10, mock.TopHashtags.Count);

            hashTag = mock.TopHashtags[0];

            Assert.AreEqual("10", hashTag.Tag);

            Assert.AreEqual(4, hashTag.Rank);

            hashTag = mock.TopHashtags[mock.TopHashtags.Count - 1]; // Check the last one

            Assert.AreEqual("H", hashTag.Tag);

            Assert.AreEqual(2, hashTag.Rank);

            Assert.AreEqual(2, mock.MinRank);
        }
    }
}
