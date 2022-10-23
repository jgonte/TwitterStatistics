using TwitterStatistics.Model.Statistics;
using TwitterStatistics.Model.Tweets;
using TwitterStatistics.Services;
using TwitterStatistics.Services.Processors;
using TwitterStatistics.Services.Readers;

namespace TwitterStatistics.Web
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddUserSecrets("aspnet-TwitterStatistics.Web-55FBEAF4-7FCA-46D2-A692-19D809FEF5BC")
                .Build();

            //// Add services to the container.
            //builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            //    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            //app.UseAuthentication();
            //app.UseAuthorization();

            var reader = new TwitterReader(
                oauthUrl: "https://api.twitter.com/oauth2/token",
                apiKey: config["apiKey"],
                apiSecret: config["apiSecret"]
            );

            var processor = new TweetProcessor(
                reader,
                new List<ITweetRule>
                {
                    new TotalReceivedTweetRule(),
                    new TopTenHashTagsTweetRule(new SimpleRankCalculator())
                });

            processor.Start();

            app.MapControllers();

            app.Run();
        }
    }
}