using System;
using System.Collections.Generic;
using data_structures.Classes.Graph;

namespace data_structures.Interfaces.Graph
{
    public interface IGraph<T>
    {
        delegate J ComputePriority<J>(GraphNode<T> element) where J : IComparable<J>;
        delegate J PriorityDecrease<J>(J currentKey, GraphNode<T> element) where J : IComparable<J>;
        
        void AddElement(T element, IEnumerable<T> relations = null);
        void AddRelations(T element, IEnumerable<T> relations);
        void AddOutgoingRelations(T element, IEnumerable<T> relations);
        ISet<T> GetElements();
        bool HasCycles(out HashSet<IEnumerable<T>> cycles);
        IEnumerable<T> PriorityVisit<J>(ComputePriority<J> func, PriorityDecrease<J> decrease) where J : IComparable<J>;
        void Remove(T element);

    }
}