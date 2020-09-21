using System;

namespace ZsProskoviceNotifier.Core.WeeklyPlan
{
    public class WeeklyPlanItem : IHasHash
    {
        public string Title { get; set; }
        public string Url { get; set; }
        public string Hash { get; set; }
    }
}