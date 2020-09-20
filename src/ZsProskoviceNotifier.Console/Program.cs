using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using ZsProskoviceNotifier.Core;

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
                var query = new GetMainPageNewsQuery();
                var handler = new GetMainPageNewsQueryHandler(httpClient, logger);
                var result = await handler.Execute(query);
            }
        }
    }
}
