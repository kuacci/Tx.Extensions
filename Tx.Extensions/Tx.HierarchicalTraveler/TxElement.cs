using System.Collections.Generic;
using Tx.HierarchicalTraveler.Core;

namespace Tx.HierarchicalTraveler
{
    internal class TxElement<TSource> where TSource : IHierarchical
    {
        public IList<TxElement<TSource>> Children { get; set; }
        public TSource Entry { get; set; }
    }
}
