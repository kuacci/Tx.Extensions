namespace Tx.Core
{
    internal interface IHierarchical
    {
        string HierarchyId { get; }
        string ParentHierarchyId { get; }
    }
}