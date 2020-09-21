using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace ZsProskoviceNotifier.Core.WeeklyPlan
{
    public class WeeklyPlanItemBuilder
    {
        private readonly HttpClient _httpClient;

        public WeeklyPlanItemBuilder(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        internal WeeklyPlanItem Create(HtmlNode rootElement)
        {
            var item = new WeeklyPlanItem();

            item.Title = GetTitle(rootElement);
            item.Url = GetUrl(rootElement);
            item.Hash = HashBuilder.CreateMD5(item.Title + item.Url);

            return item;
        }

        private string GetUrl(HtmlNode rootElement) => rootElement.SelectSingleNode(".//a").Attributes["href"].Value;

        private string GetTitle(HtmlNode rootElement) => rootElement.SelectSingleNode(".//span").InnerText;
    }
}
