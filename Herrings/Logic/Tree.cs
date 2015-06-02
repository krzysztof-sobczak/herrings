using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Herrings.Logic
{
    class Tree<T> where T : AbstractVertice
    {
        private T root;

        public T Root
        {
            get { return root; }
            set { root = value; }
        }

        private List<T> allVisitedVertices;
        public List<T> AllVisitedVertices
        {
            get { return allVisitedVertices; }
            set { allVisitedVertices = value; }
        }

        private Dictionary<int, List<T>> children;
        private Dictionary<int, List<T>> notVisitedEdges;

        public Tree(Graph<T> _graph, T _root)
        {
            allVisitedVertices = new List<T>();
            children = new Dictionary<int, List<T>>();
            Root = _root;

            notVisitedEdges = new Dictionary<int, List<T>>();
            foreach (var key in _graph.VerticeEdges.Keys)
            {
                notVisitedEdges.Add(key, new List<T>(_graph.VerticeEdges[key]));
            }
            buildTree(_graph, Root);
        }

        private void buildTree(Graph<T> _graph, T node)
        {
            List<T> _notVisitedEdges = (notVisitedEdges.Keys.Contains(node)) ? new List<T>(notVisitedEdges[node]) : new List<T>();
            children.Add(node, new List<T>());
            foreach (T child in _notVisitedEdges)
            {
                if (children.Keys.Contains(node))
                {
                    children[node].Add(child);
                }
                // each edge is double stored for both vertices
                // remove both
                notVisitedEdges[child].Remove(node);
                notVisitedEdges[node].Remove(child);
                // search child
                buildTree(_graph, child);
            }
        }

        public KeyValuePair<float, List<T>> getMaxIndependentSet()
        {
            KeyValuePair<float, List<T>> maxSetIncludingVertice = _getMaxIndependentSetIncludingVertice(Root);
            KeyValuePair<float, List<T>> maxSetExcludingVertice = _getMaxIndependentSetExcludingVertice(Root);
            addToAllVisitedVertices(Root);
            return Graph<T>.getMaxKeyValuePair(maxSetIncludingVertice, maxSetExcludingVertice);
        }

        private void addToAllVisitedVertices(T vertice)
        {
            if (!AllVisitedVertices.Contains(vertice))
            {
                AllVisitedVertices.Add(vertice);
            }
        }

        private KeyValuePair<float,List<T>> _getMaxIndependentSetIncludingVertice(T vertice)
        {
            //1 + sum(B(j) for j in children(i))
            float sum = vertice.getValue();
            List<T> set = new List<T>() { vertice };
            foreach (T child in children[vertice])
            {
                addToAllVisitedVertices(child);
                var childResult = _getMaxIndependentSetExcludingVertice(child);
                sum += childResult.Key;
                set.AddRange(childResult.Value);
            }
            return new KeyValuePair<float, List<T>>(sum, set);
        }

        private KeyValuePair<float, List<T>> _getMaxIndependentSetExcludingVertice(T vertice)
        {
            //sum(max(A(j),B(j)) for j in children(i))
            float sum = 0;
            List<T> set = new List<T>();
            foreach (T child in children[vertice])
            {
                addToAllVisitedVertices(child);
                var childResultIncluding = _getMaxIndependentSetIncludingVertice(child);
                var childResultExcluding = _getMaxIndependentSetExcludingVertice(child);
                if (childResultIncluding.Key >= childResultExcluding.Key)
                {
                    sum += childResultIncluding.Key;
                    set.AddRange(childResultIncluding.Value);
                }
                else
                {
                    sum += childResultExcluding.Key;
                    set.AddRange(childResultExcluding.Value);
                }
             } 
             return new KeyValuePair<float, List<T>>(sum, set);
        }

    }
}
