using System.Text;
using System.Text.Json;
using static TwitterStatistics.Services.Readers.TwitterReader;

namespace TwitterStatistics.Test
{
    [TestClass]
    public class TwitterDeserializerTests
    {
        [TestMethod]
        public async Task TweeterDeserializer_Should_Return_A_Tweet_With_Hashtags()
        {
            var json = @"
{
	""data"": {
		""edit_history_tweet_ids"": [""1583551724148510720""],
		""entities"": {
				""hashtags"": [{
					""start"": 49,
					""end"": 58,
					""tag"": ""alsancak""
				}, {
					""start"": 64,
					""end"": 70,
					""tag"": ""cigli""
			}],
			""mentions"": [{
				""start"": 3,
				""end"": 15,
				""username"": ""Usifuturay1"",
				""id"": ""1340796451635535876""
			}],
			""urls"": [{
					""start"": 105,
				""end"": 128,
				""url"": ""https://t.co/z5Jm3Mgvf4"",
				""expanded_url"": ""https://twitter.com/Usifuturay1/status/1583551709590130688/photo/1"",
				""display_url"": ""pic.twitter.com/z5Jm3Mgvf4"",
				""media_key"": ""3_1583551705584640013""
			}]
		},
		""id"": ""1583551724148510720"",
		""public_metrics"": {
			""retweet_count"": 39,
			""reply_count"": 2,
			""like_count"": 3,
			""quote_count"": 5
		},
		""text"": ""RT @Usifuturay1: ölür yirmi gömülür,Robin seksen #alsancak cogu #cigli  yaşında ama insan yaşında Sharma https://t.co/z5Jm3Mgvf4""
	}
}";
			var options = new JsonSerializerOptions
			{
				PropertyNameCaseInsensitive = true,
				IncludeFields = true,
			};

			var tweetData = await JsonSerializer.DeserializeAsync<TweetWrapper>(
				new MemoryStream(Encoding.UTF8.GetBytes(json)),
				options);

			Assert.IsNotNull(tweetData);

			var tweet = tweetData.Data;

			Assert.IsNotNull(tweet);

			Assert.AreEqual(
				"RT @Usifuturay1: ölür yirmi gömülür,Robin seksen #alsancak cogu #cigli  yaşında ama insan yaşında Sharma https://t.co/z5Jm3Mgvf4",
				tweet.Text
			);

			var metrics = tweet.PublicMetrics;

			Assert.IsNotNull(metrics);

			Assert.AreEqual(39, metrics.RetweetCount);

			Assert.AreEqual(2, metrics.ReplyCount);

			Assert.AreEqual(3, metrics.LikeCount);

			Assert.AreEqual(5, metrics.QuoteCount);

			var entities = tweet.Entities;

			Assert.IsNotNull(entities);

			var hashtags = entities.Hashtags;

			Assert.IsNotNull(hashtags);

			Assert.AreEqual(2, hashtags.Count);

			var hashtag = hashtags[0];

			Assert.IsNotNull(hashtag);

			Assert.AreEqual("alsancak", hashtag.Tag);

			hashtag = hashtags[1];

			Assert.IsNotNull(hashtag);

			Assert.AreEqual("cigli", hashtag.Tag);

		}

		[TestMethod]
		public async Task TweeterDeserializer_Should_Return_A_Tweet_Without_Hashtags()
		{
			var json = @"
{
	""data"": {
		""edit_history_tweet_ids"": [""1583551724148510720""],
		""entities"": {
			""mentions"": [{
				""start"": 3,
				""end"": 15,
				""username"": ""Usifuturay1"",
				""id"": ""1340796451635535876""
			}],
			""urls"": [{
					""start"": 105,
				""end"": 128,
				""url"": ""https://t.co/z5Jm3Mgvf4"",
				""expanded_url"": ""https://twitter.com/Usifuturay1/status/1583551709590130688/photo/1"",
				""display_url"": ""pic.twitter.com/z5Jm3Mgvf4"",
				""media_key"": ""3_1583551705584640013""
			}]
		},
		""id"": ""1583551724148510720"",
		""public_metrics"": {
			""retweet_count"": 39,
			""reply_count"": 2,
			""like_count"": 3,
			""quote_count"": 5
		},
		""text"": ""RT @Usifuturay1: ölür yirmi gömülür,Robin seksen #alsancak cogu #cigli  yaşında ama insan yaşında Sharma https://t.co/z5Jm3Mgvf4""
	}
}";
			var options = new JsonSerializerOptions
			{
				PropertyNameCaseInsensitive = true,
				IncludeFields = true,
			};

			var tweetData = await JsonSerializer.DeserializeAsync<TweetWrapper>(
				new MemoryStream(Encoding.UTF8.GetBytes(json)),
				options);

			Assert.IsNotNull(tweetData);

			var tweet = tweetData.Data;

			Assert.IsNotNull(tweet);

			Assert.AreEqual(
				"RT @Usifuturay1: ölür yirmi gömülür,Robin seksen #alsancak cogu #cigli  yaşında ama insan yaşında Sharma https://t.co/z5Jm3Mgvf4",
				tweet.Text
			);

			var metrics = tweet.PublicMetrics;

			Assert.IsNotNull(metrics);

			Assert.AreEqual(39, metrics.RetweetCount);

			Assert.AreEqual(2, metrics.ReplyCount);

			Assert.AreEqual(3, metrics.LikeCount);

			Assert.AreEqual(5, metrics.QuoteCount);

			var entities = tweet.Entities;

			Assert.IsNotNull(entities);

			var hashtags = entities.Hashtags;

			Assert.IsNotNull(hashtags);

			Assert.AreEqual(0, hashtags.Count);
		}
	}
}