using System;

namespace Tx.HierarchicalTraveler.Core
{
    internal interface IIndividualAction<TTarget> : IHierarchicalAction where TTarget : IHierarchical
    {
        Action<TxElement<TTarget>, TxElement<TTarget>> Action { get; }
    }
}