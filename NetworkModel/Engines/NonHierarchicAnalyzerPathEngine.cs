using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace NetworkModel.Engines
{
    /// <summary>
    /// Utility engine class, which calculates several options for non hierarchical network.
    /// <options>Options are
    ///     AveragePathLength
    ///     Diameter
    ///     Cycles3
    ///     DistanceDistribution
    /// </options>
    /// </summary>
    class NonHierarchicAnalyzerPathEngine
    {
        private readonly NonHierarchicContainer container;

        public Double AveragePath { get; private set; }
        public Double Diameter { get; private set; }
        public Double Cycles3 { get; private set; }
        public SortedDictionary<Double, Double> DistanceDistribution { get; private set; }

        public NonHierarchicAnalyzerPathEngine(NonHierarchicContainer c)
        {
            container = c;
            DistanceDistribution = new SortedDictionary<Double, Double>();
        }
        
        public void Calculate()
        {
            if (edgesBetweenNeighbours.Count == 0)
            {
                for (int i = 0; i < container.Size; ++i)
                    edgesBetweenNeighbours.Add(-1);
            }

            double avg = 0;
            int d = 0, uWay = 0, k = 0;

            for (int i = 0; i < container.Size; ++i)
            {
                for (int j = i + 1; j < container.Size; ++j)
                {
                    int way = MinimumWay(i, j);
                    if (way == -1)
                        continue;
                    else
                        uWay = way;

                    if (DistanceDistribution.ContainsKey(uWay))
                        ++DistanceDistribution[uWay];
                    else
                        DistanceDistribution.Add(uWay, 1);

                    if (uWay > d)
                        d = uWay;

                    avg += uWay;
                    ++k;
                }
            }

            Node[] nodes = new Node[container.Size];
            for (int t = 0; t < container.Size; ++t)
                nodes[t] = new Node();

            BFS((int)container.Size - 1, nodes);
            avg /= k;

            AveragePath = avg;
            Diameter = d;
            Cycles3 = CalculateCycles3();            
        }

        private List<double> edgesBetweenNeighbours = new List<double>();
        public List<double> CalculatedInformation => edgesBetweenNeighbours;

        private void BFS(int i, Node[] nodes)
        {
            Debug.Assert(i >= 0 && i < nodes.Length);

            nodes[i].lenght = nodes[i].ancestor = 0;
            bool b = true;
            Queue<int> q = new Queue<int>();
            q.Enqueue(i);
            if (edgesBetweenNeighbours[i] == -1)
                edgesBetweenNeighbours[i] = 0;
            else
                b = false;

            while (q.Count != 0)
            {
                int u = q.Dequeue();
                List<int> l = container.GetAdjacentVertices(u);
                for (int j = 0; j < l.Count; ++j)
                    if (nodes[l[j]].lenght == -1)
                    {
                        nodes[l[j]].lenght = nodes[u].lenght + 1;
                        nodes[l[j]].ancestor = u;
                        q.Enqueue(l[j]);
                    }
                    else
                    {
                        if (nodes[u].lenght == 1 && nodes[l[j]].lenght == 1 && b)
                        {
                            ++edgesBetweenNeighbours[i];
                        }
                    }
            }
            if (b)
                edgesBetweenNeighbours[i] /= 2;
        }

        private int MinimumWay(int i, int j)
        {
            if (i == j)
                return 0;

            Node[] nodes = new Node[container.Size];
            for (int k = 0; k < container.Size; ++k)
                nodes[k] = new Node();

            BFS(i, nodes);

            return nodes[j].lenght;
        }

        private Double CalculateCycles3()
        {
            Double cycles3 = 0;
            for (int i = 0; i < container.Size; ++i)
            {
                if (edgesBetweenNeighbours[i] != -1)
                    cycles3 += edgesBetweenNeighbours[i];
            }

            if (cycles3 > 0 && cycles3 < 3)
                cycles3 = 1;
            else
                cycles3 /= 3;

            return cycles3;
        }
    }
}
