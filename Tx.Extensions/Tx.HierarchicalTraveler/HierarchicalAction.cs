using Tx.HierarchicalTraveler.Core;

namespace Tx.HierarchicalTraveler
{
    internal abstract class ActionDescriptor<TTarget> where TTarget : IHierarchical
    {
        internal abstract IHierarchicalAction Action { get; }

        public HierarchicalActionType ActionType { get; protected set; }

    }
}