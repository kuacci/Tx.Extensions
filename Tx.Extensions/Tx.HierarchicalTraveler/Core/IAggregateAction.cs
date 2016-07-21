using System;
using System.Collections.Generic;

namespace Tx.HierarchicalTraveler.Core
{
    internal interface IAggregateAction<TTarget> : IHierarchicalAction where TTarget : IHierarchical
    {
        Action<TxElement<TTarget>, IList<TxElement<TTarget>>> Action { get; }
    }
}