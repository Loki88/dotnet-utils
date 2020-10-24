using System;
using System.Collections.Generic;
using System.Linq;
using data_structures.Classes.Queue;
using data_structures.Interfaces.Queue;

namespace data_structures.Classes.Graph
{
    public class Graph<T>
    {
        public delegate bool AreElementsRelated(T a, T b);
        public delegate J ComputePriority<J>(GraphNode<T> element) where J : IComparable<J>;
        public delegate J PriorityDecrease<J>(J currentKey, GraphNode<T> element) where J : IComparable<J>;

        private Dictionary<T, GraphNode<T>> NodeDict { get; set; }

        private List<T> Elements { get; set; }

        public Graph()
        {
            this.NodeDict = new Dictionary<T, GraphNode<T>>();
            this.Elements = new List<T>();
        }

        public ISet<T> GetElements()
        {
            return new HashSet<T>(this.Elements);
        }

        public Graph(IEnumerable<T> elements, AreElementsRelated func = null) : this()
        {
            foreach (T e in elements)
            {
                if (!this.NodeDict.ContainsKey(e))
                {
                    GraphNode<T> node = new GraphNode<T>(e);
                    if (func != null)
                    {
                        IEnumerable<GraphNode<T>> otherNodes = this.NodeDict.Values;
                        foreach (GraphNode<T> otherNode in otherNodes)
                        {
                            if (func.Invoke(node.Element, otherNode.Element))
                            {
                                node.AddRelation(otherNode);
                                otherNode.AddRelation(node);
                            }
                        }
                    }
                    this.NodeDict.Add(e, node);
                    this.Elements.Add(e);
                }
                else
                {
                    throw new System.Exception("Duplicate element.");
                }
            }
        }

        public void AddElement(T element, IEnumerable<T> relations = null)
        {
            if (!this.NodeDict.ContainsKey(element))
            {
                GraphNode<T> node = new GraphNode<T>(element);
                this.NodeDict.Add(element, node);
                this.Elements.Add(element);

                if (relations != null)
                {
                    this.AddRelations(element, relations);
                }
            }
            else
            {
                throw new System.Exception("Element already inside graph.");
            }
        }

        public void AddRelations(T element, IEnumerable<T> relations)
        {
            if (this.NodeDict.ContainsKey(element))
            {
                GraphNode<T> node = this.NodeDict[element];
                foreach (T e in relations)
                {
                    if (this.NodeDict.ContainsKey(e))
                    {
                        GraphNode<T> otherNode = this.NodeDict[e];
                        node.AddRelation(otherNode);
                        otherNode.AddRelation(node);
                    }
                }
            }
            else
            {
                throw new System.Exception("Element does not exist.");
            }
        }

        public void AddOutgoingRelations(T element, IEnumerable<T> relations)
        {
            if (this.NodeDict.ContainsKey(element))
            {
                GraphNode<T> node = this.NodeDict[element];
                foreach (T e in relations)
                {
                    if (this.NodeDict.ContainsKey(e))
                    {
                        GraphNode<T> otherNode = this.NodeDict[e];
                        node.AddOutRelation(otherNode);
                    }
                }
            }
            else
            {
                throw new System.Exception("Element does not exist.");
            }
        }

        public IEnumerable<T> PriorityVisit<J>(ComputePriority<J> func, PriorityDecrease<J> decrease) where J : IComparable<J>
        {
            List<T> visitedNodes = new List<T>();
            IPriorityQueue<J, GraphNode<T>> queue = new FibonacciPriorityQueue<J, GraphNode<T>>();
            Dictionary<T, bool> visitedDict = new Dictionary<T, bool>();
            Dictionary<T, J> keyDict = new Dictionary<T, J>();

            foreach (GraphNode<T> node in this.NodeDict.Values)
            {
                J priority = func.Invoke(node);
                keyDict.Add(node.Element, priority);
                queue.Insert(priority, node);
                visitedDict.Add(node.Element, false);
            }

            while (!queue.IsEmpty())
            {
                GraphNode<T> node = null;
                J key = default(J);
                queue.DeleteMin(out node, out key);
                if (node != null)
                {
                    visitedDict[node.Element] = true;
                    visitedNodes.Add(node.Element);
                    foreach (GraphNode<T> inNode in node.InNodes)
                    {
                        if (!visitedDict[inNode.Element])
                        {
                            keyDict[inNode.Element] = decrease.Invoke(keyDict[inNode.Element], inNode);
                            queue.DecreaseKey(inNode, keyDict[inNode.Element]);
                        }
                    }
                }
                else
                {
                    throw new Exception("Error travelling the graph.");
                }
            }

            return visitedNodes;
        }
    }
}