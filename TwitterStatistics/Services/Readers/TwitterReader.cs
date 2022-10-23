using System.Diagnostics;
using System.Text;
using System.Text.Json;
using TwitterStatistics.Model.Tweets;
using TwitterStatistics.Services.Converters;

namespace TwitterStatistics.Services.Readers
{
    public class TwitterReader : ITwitterReader
    {
        private string _oauthUrl;

        private string _apiKey;

        private string _apiSecret;

        public TwitterReader(string oauthUrl,
            string apiKey,
            string apiSecret)
        {
            _oauthUrl = oauthUrl;

            _apiKey = apiKey;

            _apiSecret = apiSecret;
        }

        public async Task ReadTweetsStreamAsync(string url, Action<Tweet> process)
        {
            var accessToken = await GetAccessToken();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                IncludeFields = true,
            };

            using (var client = new HttpClient())
            {
                client.Timeout = new TimeSpan(0, 0, 30);

                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);

                var stream = await client.GetStreamAsync(url);

                using (var reader = new StreamReader(stream))
                {
                    while (!reader.EndOfStream)
                    {
                        await ProcessLine(process, options, reader);
                    }
                }
            }
        }

        private static async Task ProcessLine(Action<Tweet> process, JsonSerializerOptions options, StreamReader reader)
        {
            var tweetsReceived = 0;

            var line = await reader.ReadLineAsync();

            var stopwatch = new Stopwatch();

            stopwatch.Start();

            while (!string.IsNullOrWhiteSpace(line))
            {
                ++tweetsReceived;

                var tweetData = await JsonSerializer.DeserializeAsync<TweetWrapper>(
                    new MemoryStream(Encoding.UTF8.GetBytes(line)),
                    options);

                process(tweetData.Data);

                line = await reader.ReadLineAsync();
            }

            stopwatch.Stop();

            Debug.WriteLine($"{tweetsReceived} tweets received in {stopwatch.ElapsedMilliseconds / 1000} seconds");
        }

        private async Task<string> GetAccessToken()
        {
            using (var client = new HttpClient())
            {
                var authKey = Convert.ToBase64String(
                    new UTF8Encoding().GetBytes($"{_apiKey}:{_apiSecret}")
                );

                client.DefaultRequestHeaders.Add("Authorization", $"Basic {authKey}");

                var body = new StringContent("grant_type=client_credentials", Encoding.UTF8, "application/x-www-form-urlencoded");

                Dictionary<string, string> tokenWrapper;

                using (var response = await client.PostAsync(_oauthUrl, body))
                {
                    response.EnsureSuccessStatusCode();

                    var content = await response.Content.ReadAsStringAsync();

                    var options = new JsonSerializerOptions
                    {
                        Converters =
                        {
                            new JsonDictionaryConverter()
                        }
                    };

                    tokenWrapper = await JsonSerializer.DeserializeAsync<Dictionary<string, string>>(new MemoryStream(Encoding.UTF8.GetBytes(content)), options);
                }

                return tokenWrapper["access_token"];
            }
        }

        public class TweetWrapper
        {
            public Tweet Data { get; set; }
        }

    }
}
