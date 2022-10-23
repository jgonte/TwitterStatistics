using TwitterStatistics.Model.Tweets;

namespace TwitterStatistics.Services.Readers
{
    public interface ITwitterReader
    {
        Task ReadTweetsStreamAsync(string url, Action<Tweet> process);
    }
}
