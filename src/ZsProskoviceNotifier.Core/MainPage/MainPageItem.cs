using System;

namespace ZsProskoviceNotifier.Core.MainPage
{
    public class MainPageItem : IHasHash
    {
        public string Title { get; set; }
        public DateTime? Updated { get; set; }
        public string Url { get; set; }
        public string Hash { get; set; }
        public string Message { get; internal set; }
    }
}