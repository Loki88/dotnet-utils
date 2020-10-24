using System;
using System.Collections.Generic;
using data_structures.Interfaces.Tree;

namespace data_structures.Interfaces.Graph
{
    public interface IWeightedGraph<J, T>
        where J : IComparable
    {
        bool AddVertex(T element);
        bool AddEdge(T element1, T element2, J weight);
        bool IsCyclic();
        bool GetCycles(List<List<T>> cycles);
        bool BFS(Func<T, J> function);
        bool DFS(Func<T, J> function);
        bool GetMinimumSpanningTree(out ITree<T> tree);
    }
}