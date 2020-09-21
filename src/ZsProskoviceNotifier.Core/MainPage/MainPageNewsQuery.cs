using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Microsoft.Extensions.Logging;

namespace ZsProskoviceNotifier.Core.MainPage
{
    public class MainPageNewsQuery
    {
        
    }

    public class MainPageNewsQueryResult
    {
        public MainPageNewsQueryResult()
        {
            Items = new List<MainPageItem>();
        }

        public List<MainPageItem> Items { get; }
    }

    public class MainPageNewsQueryHandler
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger _log;
        private readonly string _baseUrl = "http://www.zs-proskovice.cz/";

        public MainPageNewsQueryHandler(HttpClient httpClient, ILogger log)
        {
            _httpClient = httpClient;
            _log = log;
        }

        public async Task<MainPageNewsQueryResult> Execute(MainPageNewsQuery query)
        {
            var result = new MainPageNewsQueryResult();
            string html = await _httpClient.GetStringAsync(_baseUrl);

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);

            var mainPageItemsSection = doc.DocumentNode.SelectSingleNode("/html/body/div[1]/div/div[2]/div/div[1]/div[2]");
            var mainPageItems = mainPageItemsSection.SelectNodes(".//div[contains(@class, 'galblock')]");

            var builder = new MainPageItemBuilder(_httpClient);

            foreach (var mainPageItemElement in mainPageItems)
            {
                var mainPageItem = await builder.Create(mainPageItemElement);
                result.Items.Add(mainPageItem);
                _log.LogInformation($"Title = '{mainPageItem.Title}', Updated = '{mainPageItem.Updated}', Hash = '{mainPageItem.Hash}'");
            }

            return await Task.FromResult(result);
        }
    }
}
