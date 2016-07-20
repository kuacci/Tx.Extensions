using System;
using System.Collections.Generic;
using System.Linq;

namespace Tx.Core
{
    internal class TxTraveler<T> where T : IHierarchical
    {
        private readonly IList<T> _map;
        public IList<TxElement<T>> Elements { get; }


        public TxTraveler(IList<T> hierarchical, HierarchicalAction<T> action)
        {
            _map = hierarchical;
            Elements = new List<TxElement<T>>();

            _actions.Add(action.ActionType, action);

            InitilizeCore();
        }

        private void InitilizeCore()
        {
            var roots = _map.Where(x => x.ParentHierarchyId == "/" || string.IsNullOrEmpty(x.ParentHierarchyId));

            foreach (var root in roots)
            {
                BuildElement(root);
            }
        }

        private TxElement<T> BuildElement(T entry)
        {
            var children = _map.Where(x => x.ParentHierarchyId == entry.HierarchyId).Select(BuildElement);


            var element = new TxElement<T>
            {
                Children = children.ToList(),
                Entry = entry
            };

            Elements.Add(element);

            return element;
        }

        /// <summary>
        /// Traversal the element hierarchically 
        /// </summary>
        /// <param name="element">The element traversaling on</param>
        public void Traversal(TxElement<T> element)
        {
            if (element.Children.Any())
            {
                foreach (var child in element.Children)
                {
                    Traversal(child);

                    var individual = (IIndividualAction<T>)_actions[HierarchicalActionType.Individual];

                    if (individual != null)
                    {
                        individual.Action.Invoke(element, child);
                    }
                }

                var aggregate = (IAggregateAction<T>)_actions[HierarchicalActionType.Aggregate];

                if (aggregate != null)
                {
                    aggregate.Action.Invoke(element, element.Children.ToList());
                }
            }
        }

        private readonly IDictionary<HierarchicalActionType, HierarchicalAction<T>> _actions = new Dictionary<HierarchicalActionType, HierarchicalAction<T>>();
    }

    internal abstract class HierarchicalAction<TTarget> where TTarget : IHierarchical
    {
        internal abstract Action<TxElement<TTarget>, TxElement<TTarget>> Action { get; }

        public HierarchicalActionType ActionType { get; protected set; }

    }

    //internal class AggregateAction<TTarget> : HierarchicalAction<TTarget> where TTarget : IHierarchical
    //{
    //    internal override Action<TxElement<TTarget>, TxElement<TTarget>> Action { get; private set; }
    //}

    internal interface IAggregateAction<TTarget> : IHierarchicalAction where TTarget : IHierarchical
    {
        Action<TxElement<TTarget>, IList<TxElement<TTarget>>> Action { get; }
    }

    internal interface IHierarchicalAction
    {

    }

    internal interface IIndividualAction<TTarget> : IHierarchicalAction where TTarget : IHierarchical
    {
        Action<TxElement<TTarget>, TxElement<TTarget>> Action { get; }
    }

    internal enum HierarchicalActionType
    {
        Individual,
        Aggregate
    }
}
