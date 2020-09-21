using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ZsProskoviceNotifier.Core
{
    public class PersistentState<T> where T : IHasHash
    {
        public PersistentState(IEnumerable<T> items)
        {
            Items = items;
        }

        public IEnumerable<T> Items { get; }

        public string Serialize() => JsonConvert.SerializeObject(this);

        public StateDelta<T> CompareWithPrevious(PersistentState<T> previousState)
        {
            var previousStateHashes = new HashSet<string>(previousState.Items.Select(i => i.Hash));
            var deltas = Items.Where(i => !previousStateHashes.Contains(i.Hash));
            var stateDelta = new StateDelta<T>(deltas);

            return stateDelta;
        }

        public static PersistentState<T> Deserialize(string serializedData) => JsonConvert.DeserializeObject<PersistentState<T>>(serializedData);
    }
}
