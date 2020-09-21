using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Microsoft.Extensions.Logging;
using ZsProskoviceNotifier.Core.MainPage;

namespace ZsProskoviceNotifier.Core.WeeklyPlan
{
    public class WeeklyPlanQuery
    {
        
    }

    public class WeeklyPlanQueryHandler
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger _log;
        private readonly string _baseUrl;

        public WeeklyPlanQueryHandler(string baseUrl, HttpClient httpClient, ILogger log)
        {
            _baseUrl = baseUrl;
            _httpClient = httpClient;
            _log = log;
        }

        public async Task<WeeklyPlanQueryResult> Execute(WeeklyPlanQuery query)
        {
            var result = new WeeklyPlanQueryResult();
            string html = await _httpClient.GetStringAsync(_baseUrl);

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);

            var fileElements = doc.DocumentNode.SelectNodes(".//div[contains(@class, 'file')]");

            var builder = new WeeklyPlanItemBuilder(_httpClient);

            foreach (var fileElement in fileElements)
            {
                var weeklyPlanItem = builder.Create(fileElement);
                result.Items.Add(weeklyPlanItem);
                _log.LogInformation($"Title = '{weeklyPlanItem.Title}', Hash = '{weeklyPlanItem.Hash}'");
            }

            return await Task.FromResult(result);
        }
    }

    public class WeeklyPlanQueryResult
    {
        public WeeklyPlanQueryResult()
        {
            Items = new List<WeeklyPlanItem>();
        }
        
        public List<WeeklyPlanItem> Items { get; }
    }
}
