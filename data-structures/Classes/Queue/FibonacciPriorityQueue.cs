using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using data_structures.Interfaces.Queue;

namespace data_structures.Classes.Queue
{
    public class FibonacciPriorityQueue<J, T> : IPriorityQueue<J, T>
        where J : IComparable<J>
    {

        private readonly List<Node<J, T>> BinHeap;
        private T MinVal;
        private Dictionary<T, Node<J, T>> NodeDict;

        public FibonacciPriorityQueue()
        {
            this.BinHeap = new List<Node<J, T>>();
            this.NodeDict = new Dictionary<T, Node<J, T>>();
        }

        public int Count()
        {
            return this.NodeDict.Count;
        }

        public bool DecreaseKey(T value, J decr)
        {
            bool executed = false;
            Node<J, T> node = null;
            if (this.NodeDict.TryGetValue(value, out node))
            {
                if (node.Key.CompareTo(decr) > 0)
                {
                    node.Key = decr;

                    Node<J, T> minNode = null;
                    if (this.MinVal != null && this.NodeDict.TryGetValue(this.MinVal, out minNode))
                    {
                        if (minNode.Key.CompareTo(node.Key) > 0)
                        {
                            this.MinVal = value;
                        }
                    }
                    else
                    {
                        this.MinVal = value;
                    }

                    this.PushUp(node);
                    executed = true;
                }
            }
            return executed;
        }

        public bool Delete(T value)
        {
            bool executed = this.DecreaseKey(value, default(J));
            if (executed)
            {
                T deletedValue = default(T);
                J key = default(J);
                executed = this.DeleteMin(out deletedValue, out key);
            }
            return executed;
        }

        public bool DeleteMin(out T value, out J key)
        {
            bool executed = false;
            value = default(T);
            key = default(J);

            if (this.MinVal != null)
            {
                Node<J, T> minNode = null;
                if (this.NodeDict.TryGetValue(this.MinVal, out minNode))
                {
                    if (this.BinHeap.Remove(minNode))
                    {
                        value = minNode.Element;
                        key = minNode.Key;
                        this.NodeDict.Remove(minNode.Element);

                        List<Node<J, T>> children = minNode.Children;
                        children.ForEach(x =>
                        {
                            x.Parent = null;
                            this.BinHeap.Add(x);
                        });

                        this.MinVal = default(T);
                        this.FixHeap();
                        executed = true;
                    }
                }
            }

            return executed;
        }

        public bool FindMin(out T value)
        {
            bool executed = false;
            value = default(T);

            if (this.MinVal != null)
            {
                value = this.MinVal;
                executed = true;
            }

            return executed;
        }

        public IEnumerator<T> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public bool IncreaseKey(T value, J incr)
        {
            bool executed = false;
            Node<J, T> node = null;
            if (this.NodeDict.TryGetValue(value, out node))
            {
                if (node.Key.CompareTo(incr) < 0)
                {
                    executed = this.Delete(value);
                    if (executed)
                    {
                        this.NodeDict.Remove(value);
                        executed = this.Insert(incr, value);
                    }
                }
            }
            return executed;
        }

        public bool Insert(J key, T value)
        {
            bool executed = false;
            if (value != null)
            {
                Node<J, T> newNode = new Node<J, T>()
                {
                    Key = key,
                    Element = value,
                    Degree = 1,
                    Parent = null
                };

                if (!this.NodeDict.ContainsKey(value))
                {
                    this.NodeDict.Add(value, newNode);
                    this.BinHeap.Add(newNode);

                    Node<J, T> minNode = null;
                    if (this.MinVal != null && this.NodeDict.TryGetValue(this.MinVal, out minNode))
                    {
                        if (minNode.Key.CompareTo(newNode.Key) > 0)
                        {
                            this.MinVal = value;
                        }
                    }
                    else
                    {
                        this.MinVal = value;
                    }

                    executed = true;
                } else {
                    throw new Exception("Value already exists");
                }
            }
            return executed;
        }

        public bool IsEmpty()
        {
            return this.BinHeap.Count == 0;
        }

        public bool Merge(IPriorityQueue<J, T> q1, IPriorityQueue<J, T> q2)
        {
            bool executed = true;
            while (!q1.IsEmpty())
            {
                T element = default(T);
                J key = default(J);
                if (q1.DeleteMin(out element, out key))
                {
                    this.Insert(key, element);
                }
            }

            while (!q2.IsEmpty())
            {
                T element = default(T);
                J key = default(J);
                if (q2.DeleteMin(out element, out key))
                {
                    this.Insert(key, element);
                }
            }

            return executed;
        }

        internal void FixHeap()
        {
            if (this.BinHeap.Count > 0)
            {
                List<Node<J, T>> tmpList = this.BinHeap.ToList();
                Dictionary<int, Node<J, T>> nodeSizeDict = new Dictionary<int, Node<J, T>>();
                IEnumerator<Node<J, T>> nodeEnum = tmpList.GetEnumerator();
                while (nodeEnum.MoveNext())
                {
                    Node<J, T> node = nodeEnum.Current;
                    Node<J, T> otherNode = null;
                    while (nodeSizeDict.TryGetValue(node.Degree, out otherNode))
                    {
                        nodeSizeDict.Remove(node.Degree);
                        Node<J, T> mergedNode = this.MergeBinomialTrees(otherNode, node);
                        node = mergedNode;
                    }

                    Node<J, T> minNode = null;
                    if (this.MinVal != null && this.NodeDict.TryGetValue(this.MinVal, out minNode))
                    {
                        if (minNode.Key.CompareTo(node.Key) > 0)
                        {
                            this.MinVal = node.Element;
                        }
                    }
                    else
                    {
                        this.MinVal = node.Element;
                    }

                    nodeSizeDict.Add(node.Degree, node);
                }

                this.BinHeap.Clear();
                this.BinHeap.AddRange(nodeSizeDict.Values);
            }
        }

        internal Node<J, T> MergeBinomialTrees(Node<J, T> b1, Node<J, T> b2)
        {
            if (b1.Key.CompareTo(b2.Key) > 0)
            {
                b2.Children.Add(b1);
                b1.Parent = b2;
                b2.Degree++;
                return b2;
            }
            else
            {
                b1.Children.Add(b2);
                b2.Parent = b1;
                b1.Degree++;
                return b1;
            }
        }

        internal void PushUp(Node<J, T> n)
        {
            while (n.Parent != null && n.Parent.Key.CompareTo(n.Key) > 0)
            {
                J tmpKey = n.Key;
                T tmpElement = n.Element;

                n.Key = n.Parent.Key;
                n.Element = n.Parent.Element;
                this.NodeDict[n.Element] = n;

                n.Parent.Key = tmpKey;
                n.Parent.Element = tmpElement;
                this.NodeDict[n.Parent.Element] = n.Parent;

                n = n.Parent;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.NodeDict.Values.GetEnumerator();
        }
    }

    internal class Node<J, T>
    {
        internal J Key;

        internal T Element;

        internal int Degree;

        internal Node<J, T> Parent;

        internal List<Node<J, T>> Children;

        public Node()
        {
            this.Children = new List<Node<J, T>>();
            this.Degree = 1;
        }
    }
}