using System.Collections.Generic;
using System.Linq;
using Tx.HierarchicalTraveler.Core;

namespace Tx.HierarchicalTraveler
{
    internal class TxTraveler<T> where T : IHierarchical
    {
        private readonly IList<T> _map;
        public IList<TxElement<T>> Elements { get; }

        private readonly IDictionary<HierarchicalActionType, ActionDescriptor<T>> _actions = new Dictionary<HierarchicalActionType, ActionDescriptor<T>>();

        public TxTraveler(IList<T> hierarchical, ActionDescriptor<T> action)
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

                    var individual = _actions[HierarchicalActionType.Individual] as IIndividualAction<T>;

                    if (individual != null)
                    {
                        individual.Action.Invoke(element, child);
                    }
                }

                var aggregate = _actions[HierarchicalActionType.Aggregate] as IAggregateAction<T>;

                if (aggregate != null)
                {
                    aggregate.Action.Invoke(element, element.Children.ToList());
                }
            }
        }
    }

    internal enum HierarchicalActionType
    {
        Individual,
        Aggregate
    }
}
