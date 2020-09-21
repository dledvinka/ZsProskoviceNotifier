using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace ZsProskoviceNotifier.Core.MainPage
{
    public class MainPageItemBuilder
    {
        private HttpClient _httpClient;

        public MainPageItemBuilder(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<MainPageItem> Create(HtmlNode rootElement)
        {
            var item = new MainPageItem();

            item.Title = GetTitle(rootElement);
            item.Updated = GetUpdated(rootElement);
            item.Url = GetUrl(rootElement);
            item.Message = await GetMessage(rootElement, item.Url);
            item.Hash = HashBuilder.CreateMD5(item.Title + item.Updated.ToString() + item.Url + item.Message);

            return item;
        }

        private async Task<string> GetMessage(HtmlNode rootElement, string detailUrl)
        {
            if (string.IsNullOrEmpty(detailUrl) ||detailUrl.EndsWith(".docx"))
                return string.Empty;
            
            string html = await _httpClient.GetStringAsync(detailUrl);

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);
            var detailSection = doc.DocumentNode.SelectSingleNode("//div[contains(@class, 'cbox_large')]/div");

            return detailSection.InnerText;
        }

        private string GetUrl(HtmlNode rootElement)
        {
            string url = string.Empty;
            var urlNode = rootElement.SelectSingleNode(".//*[contains(@class, 'galerie')]");
            var linkAttribute = urlNode.Attributes["onclick"] ?? urlNode.Attributes["href"];

            if (linkAttribute != null)
            {
                url = linkAttribute.Value.Replace("document.location.href=", string.Empty).Replace("'", string.Empty);
            }

            return url;
        }

        private DateTime? GetUpdated(HtmlNode rootElement)
        {
            var updatedString = rootElement.SelectSingleNode(".//span[contains(@class, 'date_akt')]").InnerText;
            string trimmed = updatedString.Replace("Poslední aktualizace:", string.Empty).Replace("&nbsp;", string.Empty).Trim();

            return DateTime.Parse(trimmed);
        }

        private string GetTitle(HtmlNode rootElement) => rootElement.SelectSingleNode(".//div[contains(@class, 'datew')]").InnerText;
    }
}
