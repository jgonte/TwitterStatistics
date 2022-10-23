using Microsoft.AspNetCore.Mvc;
using TwitterStatistics.Model.Statistics;

namespace TwitterStatistics.Web.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("[controller]")]
    //[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
    public class TweetStatisticsController : ControllerBase
    {     
        private readonly ILogger<TweetStatisticsController> _logger;

        public TweetStatisticsController(ILogger<TweetStatisticsController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetTweetStatistics")]
        public TweetStatistics Get()
        {
            return TweetStatistics.Instance;
        }
    }
}