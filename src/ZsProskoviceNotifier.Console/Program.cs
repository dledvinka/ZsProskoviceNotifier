using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using ZsProskoviceNotifier.Core;
using ZsProskoviceNotifier.Core.MainPage;
using ZsProskoviceNotifier.Core.WeeklyPlan;

namespace ZsProskoviceNotifier.Console
{
    class Program
    {
        static async Task Main(string[] args)
        {
            System.Console.WriteLine("Hello World!");

            var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddFilter("Microsoft", LogLevel.Warning)
                       .AddFilter("System", LogLevel.Warning)
                       .AddFilter("ZsProskoviceNotifier", LogLevel.Debug)
                       .AddConsole();
            });

            var logger = loggerFactory.CreateLogger<Program>();

            using (var httpClient = new HttpClient())
            {
                //var query = new MainPageNewsQuery();
                //var handler = new MainPageNewsQueryHandler(httpClient, logger);
                //var result = await handler.Execute(query);

                var weeklyPlanQuery = new WeeklyPlanQuery();
                var weeklyPlanHandler = new WeeklyPlanQueryHandler(WeeklyPlanUrls.Grade02Url, httpClient, logger);
                var weeklyPlanResult = await weeklyPlanHandler.Execute(weeklyPlanQuery);
            }
        }
    }
}
