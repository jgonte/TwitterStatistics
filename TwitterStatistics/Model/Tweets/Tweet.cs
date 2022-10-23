using System.Text.Json.Serialization;

namespace TwitterStatistics.Model.Tweets
{
    public class Tweet
    {
        public string Id { get; set; }

        public string Text { get; set; }

        [JsonPropertyName("public_metrics")]
        public PublicMetrics PublicMetrics { get; set; } = new PublicMetrics();

        public Entities Entities { get; set; } = new Entities();
    }
}
