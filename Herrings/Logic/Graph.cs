using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Herrings.Logic
{
    class Graph<T> where T : AbstractVertice
    {
        private List<T> vertices;

        public List<T> Vertices
        {
            get { return vertices; }
            set { vertices = value; }
        }

        private Dictionary<int,List<T>> sourceTargetEdges;

        public Dictionary<int,List<T>> SourceTargetEdges
        {
            get { return sourceTargetEdges; }
            set { sourceTargetEdges = value; }
        }

        private Dictionary<int, List<T>> targetSourceEdges;

        public Dictionary<int, List<T>> TargetSourceEdges
        {
            get { return targetSourceEdges; }
            set { targetSourceEdges = value; }
        }

        private Dictionary<int, List<T>> verticeEdges;

        public Dictionary<int, List<T>> VerticeEdges
        {
            get { return verticeEdges; }
            set { verticeEdges = value; }
        }

        private List<T> visitedVertices;
        private Dictionary<int, List<T>> notVisitedEdges;

        public Graph() {
            Vertices = new List<T>();
            VerticeEdges = new Dictionary<int, List<T>>();
            SourceTargetEdges = new Dictionary<int, List<T>>();
            TargetSourceEdges = new Dictionary<int, List<T>>();
        }

        public Graph(List<T> _vertices)
        {
            Vertices = _vertices;
            VerticeEdges = new Dictionary<int, List<T>>();
            SourceTargetEdges = new Dictionary<int, List<T>>();
            TargetSourceEdges = new Dictionary<int, List<T>>();
        }

        public Graph<T> clone()
        {
            Graph<T> graph = new Graph<T>(new List<T>(Vertices));
            graph.VerticeEdges = new Dictionary<int, List<T>>();
            foreach (var key in VerticeEdges.Keys)
            {
                graph.VerticeEdges.Add(key, new List<T>(VerticeEdges[key]));
            }
            return graph;
        }

        public void addSourceTargetEdge(T source, T target)
        {
            if (source != null && target != null)
            {
                if (SourceTargetEdges.Keys.Contains(source))
                {
                    SourceTargetEdges[source].Add(target);
                }
                else
                {
                    SourceTargetEdges.Add(source, new List<T>() { target });
                }
                addVerticeEdge(source, target);
                addVerticeEdge(target, source);
            }
        }

        public void addTargetSourceEdge(T target, T source)
        {
            if (source != null && target != null)
            {
                if (TargetSourceEdges.Keys.Contains(target))
                {
                    TargetSourceEdges[target].Add(source);
                }
                else
                {
                    TargetSourceEdges.Add(target, new List<T>() { source });
                }
                addVerticeEdge(source, target);
                addVerticeEdge(target, source);
            }
        }

        private void addVerticeEdge(T source, T target)
        {
            if (source != null && target != null)
            {
                if (VerticeEdges.Keys.Contains(source))
                {
                    if (!VerticeEdges[source].Contains(target))
                    {
                        VerticeEdges[source].Add(target);
                    }
                }
                else
                {
                    VerticeEdges.Add(source, new List<T>() { target });
                }
            }
        }

        public KeyValuePair<float, List<T>> getMaxIndependentSet(bool checkCycles = false)
        {
            List<T> _vertices = new List<T>(Vertices);
            float maxIndependentSetSum = 0;
            List<T> maxIndependentSetMembers = new List<T>();
            while(_vertices.Count > 0)
            {
                T vertice = _vertices[0];
                _vertices.RemoveRange(0,1);
                KeyValuePair<float, List<T>> maxSet = new KeyValuePair<float, List<T>>();
                if(checkCycles)
                {
                    List<Graph<T>> graphs = getGraphsWithoutCycle(vertice);
                    foreach (Graph<T> graph in graphs)
                    {
                        foreach (T visitedVertice in graph.Vertices)
                        {
                            _vertices.Remove(visitedVertice);
                        }
                    }
                    if (graphs.Count == 1)
                    {
                        maxSet = graphs[0].getMaxIndependentSet();
                    }
                    else
                    {
                        maxSet = getMaxKeyValuePair(graphs[0].getMaxIndependentSet(), graphs[1].getMaxIndependentSet());
                    }
                }
                else
                {
                    Tree<T> tree = new Tree<T>(this, vertice);
                    maxSet = tree.getMaxIndependentSet();
                    foreach (T visitedVertice in tree.AllVisitedVertices)
                    {
                        _vertices.Remove(visitedVertice);
                    }
                }
                maxIndependentSetSum += maxSet.Key;
                maxIndependentSetMembers.AddRange(maxSet.Value);
            }
            return new KeyValuePair<float, List<T>>(maxIndependentSetSum, maxIndependentSetMembers);
        }

        public static KeyValuePair<float, List<T>> getMaxKeyValuePair(KeyValuePair<float, List<T>> kvp1, KeyValuePair<float, List<T>> kvp2)
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

        public List<Graph<T>> getGraphsWithoutCycle(T _vertice)
        {
            T verticeInCycle = findVerticeInCycle(_vertice, true);
            Graph<T> visitedGraph = this.clone();
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
            foreach (T vertice in this.VerticeEdges[verticeInCycle])
            {
                graphIncludingVertice.Vertices.Remove(vertice);
            }
            graphIncludingVertice.removeNotExistingEdges();

            return new List<Graph<T>>() { graphIncludingVertice, graphExcludingVertice };
        }

        private T findVerticeInCycle(T vertice, bool init = false)
        {
            if (init)
            {
                visitedVertices = new List<T>();
                notVisitedEdges = new Dictionary<int, List<T>>();
                foreach (var key in this.VerticeEdges.Keys)
                {
                    notVisitedEdges.Add(key, new List<T>(this.VerticeEdges[key]));
                }
            }
            if (isVisited(vertice))
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

        public void removeNotExistingEdges()
        {
            Dictionary<int, List<T>> updatedVerticeEdges = new Dictionary<int, List<T>>();
            foreach(T vertice in Vertices)
            {
                updatedVerticeEdges.Add(vertice, new List<T>());
                if(VerticeEdges.Keys.Contains(vertice))
                {
                    foreach (T verticeEdge in VerticeEdges[vertice])
                    {
                        if (Vertices.Contains(verticeEdge))
                        {
                            updatedVerticeEdges[vertice].Add(verticeEdge);
                        }
                    }
                }
            }
            VerticeEdges = updatedVerticeEdges;
        }

    }
}
