using TwitterStatistics.Model.Statistics;
using TwitterStatistics.Model.Tweets;
using TwitterStatistics.Services.Processors;
using TwitterStatistics.Services.Readers;

namespace TwitterStatistics.Services
{
    public class TweetProcessor
    {
        private ITwitterReader _reader;

        private List<ITweetRule> _rules;

        public TweetProcessor(ITwitterReader reader, List<ITweetRule> rules)
        {
            _reader = reader;

            _rules = rules;
        }

        public void Start()
        {
            _reader.ReadTweetsStreamAsync(
                "https://api.twitter.com/2/tweets/sample/stream?tweet.fields=id,text,entities,public_metrics",
                (Tweet tweet) =>
                {
                    var statistics = TweetStatistics.Instance as ITweetStatistics;

                    foreach (var rule in _rules)
                    {
                        statistics = rule.Process(statistics, tweet);
                    }
                });
        }
    }
}
