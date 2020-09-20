using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Microsoft.Extensions.Logging;

namespace ZsProskoviceNotifier.Core
{
    public class GetMainPageNewsQuery
    {
        
    }

    public class GetMainPageNewsQueryResult
    {
    }

    public class GetMainPageNewsQueryHandler
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger _log;
        private readonly string _baseUrl = "http://www.zs-proskovice.cz/";

        public GetMainPageNewsQueryHandler(HttpClient httpClient, ILogger log)
        {
            _httpClient = httpClient;
            _log = log;
        }

        public async Task<GetMainPageNewsQueryResult> Execute(GetMainPageNewsQuery query)
        {
            var result = new GetMainPageNewsQueryResult();
            var titles = new List<string>();

            string html = await _httpClient.GetStringAsync(_baseUrl);

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);


            var bigTable = doc.DocumentNode.SelectSingleNode("/html/body/div[1]/div/div[2]/div/div[1]/div[2]");
            var news = bigTable.SelectNodes("//div[contains(@class, 'galblock')]");

            foreach (var item in news)
            {
                var title = item.SelectSingleNode("//div[contains(@class, 'datew')]").InnerText;
                _log.LogInformation($"Title = '{title}'");

                titles.Add(title);
            }
            // /html/body/div[1]/div/div[2]/div/div[1]/div[2]
            //            var thirdTable = bigTable.SelectNodes("tr")[2];
            //            var rows = thirdTable.SelectNodes(".//tr");

            return await Task.FromResult(result);
        }


    }
}
