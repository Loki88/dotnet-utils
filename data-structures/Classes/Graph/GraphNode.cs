
using System.Collections.Generic;
using System.Linq;

namespace data_structures.Classes.Graph
{
    public class GraphNode<T>
    {
        protected T _Element { get; set; }
        public T Element
        {
            get
            {
                return this._Element;
            }
        }
        private Dictionary<T, GraphNode<T>> OutNeighbours { get; set; }
        private Dictionary<T, GraphNode<T>> InNeighbours { get; set; }

        public IEnumerable<GraphNode<T>> OutNodes
        {
            get
            {
                return this.OutNeighbours.Values;
            }
        }
        public IEnumerable<GraphNode<T>> InNodes
        {
            get
            {
                return this.InNeighbours.Values;
            }
        }

        public int OutDegree
        {
            get
            {
                return this.OutNeighbours.Count;
            }
        }

        public int InDegree
        {
            get
            {
                return this.InNeighbours.Count;
            }
        }

        protected GraphNode() { }

        public GraphNode(T element)
        {
            this._Element = element;
            this.OutNeighbours = new Dictionary<T, GraphNode<T>>();
            this.InNeighbours = new Dictionary<T, GraphNode<T>>();
        }

        public void AddRelation(GraphNode<T> node)
        {
            this.AddOutRelation(node);
            this.AddInRelation(node);
        }

        public void RemoveRelation(GraphNode<T> node)
        {
            this.RemoveOutRelation(node);
            this.RemoveInRelation(node);
        }

        public void RemoveOutRelation(GraphNode<T> node)
        {
            this.OutNeighbours.Remove(node.Element);
        }

        public void RemoveInRelation(GraphNode<T> node)
        {
            this.InNeighbours.Remove(node.Element);
        }

        public void AddOutRelation(GraphNode<T> node)
        {
            if (!this.OutNeighbours.ContainsKey(node.Element))
            {
                this.OutNeighbours.Add(node.Element, node);
                node.AddInRelation(this);
            }
        }

        public void AddInRelation(GraphNode<T> node)
        {
            if (!this.InNeighbours.ContainsKey(node.Element))
            {
                this.InNeighbours.Add(node.Element, node);
                node.AddOutRelation(this);
            }
        }

        public GraphNode<T> Clone()
        {
            GraphNode<T> clone = new GraphNode<T>()
            {
                _Element = this._Element,
                InNeighbours = this.InNeighbours.ToDictionary(x => x.Key, x => x.Value),
                OutNeighbours = this.OutNeighbours.ToDictionary(x => x.Key, x => x.Value),
            };
            return clone;
        }
    }
}