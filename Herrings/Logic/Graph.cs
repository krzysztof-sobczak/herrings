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
                Tree<T> tree = new Tree<T>(this, vertice);
                KeyValuePair<float, List<T>> maxSet = tree.getMaxIndependentSet(checkCycles);
                maxIndependentSetSum += maxSet.Key;
                maxIndependentSetMembers.AddRange(maxSet.Value);
                foreach (T visitedVertice in tree.AllVisitedVertices)
                {
                    _vertices.Remove(visitedVertice);
                }
            }
            return new KeyValuePair<float, List<T>>(maxIndependentSetSum, maxIndependentSetMembers);
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
