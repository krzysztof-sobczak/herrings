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

        private Graph<T> graph;

        public Graph<T> Graph
        {
            get { return graph; }
            set { graph = value; }
        }

        private List<T> allVisitedVertices;
        public List<T> AllVisitedVertices
        {
            get { return allVisitedVertices; }
            set { allVisitedVertices = value; }
        }

        private List<T> visitedVertices;
        private Dictionary<int, List<T>> notVisitedEdges;


        public Tree(Graph<T> _graph, T _root)
        {
            Graph = _graph;
            Root = _root;
            AllVisitedVertices = new List<T>();
        }

        public KeyValuePair<float, List<T>> getMaxIndependentSet(bool checkCycles = false)
        {
            if(checkCycles)
            {
                List<Graph<T>> graphs = getGraphsWithoutCycle();
                foreach(Graph<T> graph in graphs)
                {
                    foreach(T vertice in graph.Vertices)
                    {
                        addToAllVisitedVertices(vertice);
                    }
                }
                if(graphs.Count == 1) {
                    return graphs[0].getMaxIndependentSet();
                } else {
                    return getMaxKeyValuePair(graphs[0].getMaxIndependentSet(),graphs[1].getMaxIndependentSet());
                }
            }

            // getting max independent set in tree
            return _getMaxIndependentSetForVertice(Root);
        }

        private void addToAllVisitedVertices(T vertice)
        {
            if (!AllVisitedVertices.Contains(vertice))
            {
                AllVisitedVertices.Add(vertice);
            }
        }

        private KeyValuePair<float,List<T>> getMaxKeyValuePair(KeyValuePair<float,List<T>> kvp1, KeyValuePair<float,List<T>>kvp2)
        {
            if (kvp1.Key >= kvp2.Key)
            {
                return kvp1;
            }
            else
            {
                return kvp2;
            }
        }

        private KeyValuePair<float, List<T>> _getMaxIndependentSetForVertice(T vertice)
        {
            notVisitedEdges = new Dictionary<int, List<T>>();
            foreach (var key in Graph.VerticeEdges.Keys)
            {
                notVisitedEdges.Add(key, new List<T>(Graph.VerticeEdges[key]));
            }
            KeyValuePair<float, List<T>> maxSetIncludingVertice = _getMaxIndependentSetIncludingVertice(vertice);
            KeyValuePair<float, List<T>> maxSetExcludingVertice = _getMaxIndependentSetExcludingVertice(vertice);
            addToAllVisitedVertices(vertice);
            return getMaxKeyValuePair(maxSetIncludingVertice, maxSetExcludingVertice);
        }

        private KeyValuePair<float,List<T>> _getMaxIndependentSetIncludingVertice(T vertice)
        {
            //1 + sum(B(j) for j in children(i))
            float sum = vertice.getValue();
            List<T> set = new List<T>() { vertice };
            List<T> children = (notVisitedEdges.Keys.Contains(vertice)) ? new List<T>(notVisitedEdges[vertice]) : new List<T>();
            foreach (T child in children)
            {
                addToAllVisitedVertices(child);
                // each edge is double stored for both vertices
                // remove both
                notVisitedEdges[child].Remove(vertice);
                notVisitedEdges[vertice].Remove(child);
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
            List<T> children = (notVisitedEdges.Keys.Contains(vertice)) ? new List<T>(notVisitedEdges[vertice]) : new List<T>();
            foreach (T child in children)
            {
                addToAllVisitedVertices(child);
                // each edge is double stored for both vertices
                // remove both
                notVisitedEdges[child].Remove(vertice);
                notVisitedEdges[vertice].Remove(child);
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

        public List<Graph<T>> getGraphsWithoutCycle()
        {
            T verticeInCycle = findVerticeInCycle(Root, true);
            Graph<T> visitedGraph = Graph.clone();
            visitedGraph.Vertices = visitedVertices;
            visitedGraph.removeNotExistingEdges();
            if (verticeInCycle == null)
            {
                return new List<Graph<T>>() { visitedGraph };
            }
            // split tree into to separate graphs via found vertice
            Graph<T> graphExcludingVertice = visitedGraph.clone();
            graphExcludingVertice.Vertices.Remove(verticeInCycle);
            graphExcludingVertice.removeNotExistingEdges();

            Graph<T> graphIncludingVertice = visitedGraph.clone();
            foreach (T vertice in Graph.VerticeEdges[verticeInCycle])
            {
                graphIncludingVertice.Vertices.Remove(vertice);
            }
            graphIncludingVertice.removeNotExistingEdges();

            return new List<Graph<T>>() { graphIncludingVertice, graphExcludingVertice};
        }

        private T findVerticeInCycle(T vertice, bool init = false)
        {
            if(init)
            {
                visitedVertices = new List<T>();
                notVisitedEdges = new Dictionary<int,List<T>>();
                foreach (var key in  Graph.VerticeEdges.Keys)
                {
                    notVisitedEdges.Add(key, new List<T>(Graph.VerticeEdges[key]));
                }
            }
            if(isVisited(vertice))
            {
                return vertice;
            }
            else
            {
                visitedVertices.Add(vertice);
            }
            List<T> _notVisitedEdges = (notVisitedEdges.Keys.Contains(vertice)) ? new List<T>(notVisitedEdges[vertice]) : new List<T>();
            if (_notVisitedEdges.Count > 0)
            {
                T result = null;
                foreach (T child in _notVisitedEdges)
                {
                    // each edge is double stored for both vertices
                    // remove both
                    notVisitedEdges[child].Remove(vertice);
                    notVisitedEdges[vertice].Remove(child);
                    // search child
                    T childResponse = findVerticeInCycle(child);
                    if (childResponse != null)
                    {
                        result = childResponse;
                    }
                }
                return result;
            }
            return null;
        }

        private bool isVisited(T vertice)
        {
            return visitedVertices.Contains(vertice);
        }
    }
}
