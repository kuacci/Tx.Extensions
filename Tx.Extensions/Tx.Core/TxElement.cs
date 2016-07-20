using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tx.Core
{
    internal class TxElement<TSource> where TSource : IHierarchical
    {
        public IList<TxElement<TSource>> Children { get; set; }
        public TSource Entry { get; set; }
    }
}
