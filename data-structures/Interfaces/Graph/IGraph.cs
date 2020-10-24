using System;
using System.Collections.Generic;

namespace data_structures.Interfaces.Graph
{
    public interface IGraph<T>
    {
        bool AddVertex(T element);
        bool AddEdge(T element1, T element2);
        bool IsCyclic();
        bool GetCycles(List<List<T>> cycles);
        bool BFS(Func<T> function);
        bool DFS(Func<T> function);
    }
}