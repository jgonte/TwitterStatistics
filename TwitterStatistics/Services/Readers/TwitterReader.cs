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

        private object _lock = new Object();

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
                        await ProcessSream(process, options, reader);
                    }
                }
            }
        }

        private async Task ProcessSream(Action<Tweet> process, JsonSerializerOptions options, StreamReader reader)
        {
            var tweetsReceived = 0;
   
            var stopwatch = new Stopwatch();

            stopwatch.Start();

            var line = await reader.ReadLineAsync();


            while (!string.IsNullOrWhiteSpace(line))
            {
                ++tweetsReceived;

                ThreadPool.QueueUserWorkItem(ProcessLine, 
                    new ProcessLineParams(process, options, line));

                //ProcessLine(process, options, line);

                line = await reader.ReadLineAsync();
            }

            stopwatch.Stop();

            Debug.WriteLine($"{tweetsReceived} tweets received in {stopwatch.ElapsedMilliseconds / 1000} seconds");
        }

        private void ProcessLine(object state)
        {
            var p = state as ProcessLineParams;

            var tweetData = JsonSerializer.Deserialize<TweetWrapper>(p.Line, p.Options);
            
            lock(_lock)
            {
                p.Process(tweetData.Data);
            }  
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

    internal class ProcessLineParams
    {
        public Action<Tweet> Process { get; private set; }

        public JsonSerializerOptions Options { get; private set; }

        public string Line { get; private set; }

        public ProcessLineParams(Action<Tweet> process, JsonSerializerOptions options, string line)
        {
            Process = process;

            Options = options;

            Line = line;
        }  
    }
}
