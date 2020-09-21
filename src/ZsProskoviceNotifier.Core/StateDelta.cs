using System.Collections.Generic;
using System.Linq;

namespace ZsProskoviceNotifier.Core
{
    public class StateDelta<T> where T: IHasHash
    {
        public StateDelta(IEnumerable<T> deltas)
        {
            Changes = new List<T>(deltas);
            HasChanges = Changes.Any();
        }
        
        public bool HasChanges { get; }
        public IReadOnlyList<T> Changes { get; set; }
    }
}