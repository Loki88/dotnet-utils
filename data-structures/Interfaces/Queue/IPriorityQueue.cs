using System;
using System.Collections.Generic;

namespace data_structures.Interfaces.Queue
{
    public interface IPriorityQueue<J, T> : IEnumerable<T>
        where J : IComparable<J>
    {
        int Count();
        
        bool IsEmpty();

        bool FindMin(out T value);

        bool Insert(J key, T value);

        bool DeleteMin(out T value, out J key);

        bool DecreaseKey(T value, J decr);

        bool Delete(T value);

        bool IncreaseKey(T value, J incr);

        bool Merge(IPriorityQueue<J, T> q1, IPriorityQueue<J, T> q2);
    }
}