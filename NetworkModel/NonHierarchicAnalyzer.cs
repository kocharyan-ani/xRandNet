using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

using Core;
using Core.Model;
using Core.Enumerations;
using RandomNumberGeneration;
using NetworkModel.Engines;

namespace NetworkModel
{
    public class NonHierarchicAnalyzer : AbstractNetworkAnalyzer
    {
        private NonHierarchicContainer container;

        public NonHierarchicAnalyzer(AbstractNetwork n) : base(n) { }

        public override INetworkContainer Container
        {
            get { return container; }
            set
            {
                abstractContainer = (NonHierarchicContainer)value;
                container = abstractContainer as NonHierarchicContainer;
            }
        }

        protected override Double CalculateEdgesCountOfNetwork()
        {
            return container.EdgesCount;
        }

        protected override Double CalculateAveragePath()
        {
            CheckPathEngine();
            Debug.Assert(pathEngine != null);            
            return pathEngine.AveragePath;
        }

        protected override Double CalculateDiameter()
        {
            CheckPathEngine();
            Debug.Assert(pathEngine != null);
            return pathEngine.Diameter;
        }

        protected override Double CalculateAverageClusteringCoefficient()
        {
            CheckPathEngine();
            Debug.Assert(pathEngine != null);
            double sum = 0, degree = 0;
            for (int i = 0; i < container.Size; ++i)
            {
                degree = container.GetVertexDegree(i);
                sum += degree * (degree - 1);
            }

            return 6 * pathEngine.Cycles3 / sum;
        }

        protected override Double CalculateCycles3()
        {
            CheckPathEngine();
            Debug.Assert(pathEngine != null);
            return pathEngine.Cycles3;
        }

        protected override Double CalculateCycles4()
        {
            Double count = 0;
            for (int i = 0; i < container.Size; i++)
                count += Get4OrderCyclesOfNode(i);

            return count / 4;
        }

        protected override SortedDictionary<Double, Double> CalculateDegreeDistribution()
        {
            SortedDictionary<Double, Double> degreeDistribution = new SortedDictionary<Double, Double>();
            for (uint i = 0; i < container.Size; ++i)
                degreeDistribution[i] = new Double();

            for (int i = 0; i < container.Size; ++i)
            {
                int degreeOfVertexI = container.GetVertexDegree(i);
                ++degreeDistribution[degreeOfVertexI];
            }

            for (uint i = 0; i < container.Size; ++i)
                if (degreeDistribution[i] == 0)
                    degreeDistribution.Remove(i);

            return degreeDistribution;
        }

        protected override SortedDictionary<Double, Double> CalculateClusteringCoefficientDistribution()
        {
            CheckCoeffEngine();
            Debug.Assert(coeffEngine != null);            
            return coeffEngine.CoefficientsDistribution;
        }

        protected override SortedDictionary<Double, Double> CalculateClusteringCoefficientPerVertex()
        {
            CheckCoeffEngine();
            Debug.Assert(coeffEngine != null);            
            return coeffEngine.CoefficientsPerVertex;
        }

        protected override SortedDictionary<Double, Double> CalculateConnectedComponentDistribution()
        {
            SortedDictionary<Double, Double> result = new SortedDictionary<Double, Double>();
            Queue<int> q = new Queue<int>();
            Node[] nodes = new Node[container.Size];
            for (int i = 0; i < nodes.Length; i++)
                nodes[i] = new Node();

            for (int i = 0; i < container.Size; i++)
            {
                Int32 order = 0;
                q.Enqueue(i);
                while (q.Count != 0)
                {
                    int item = q.Dequeue();
                    if (nodes[item].lenght != 2)
                    {
                        if (nodes[item].lenght == -1)
                        {
                            order++;
                        }
                        List<int> list = container.GetAdjacentVertices(item);
                        nodes[item].lenght = 2;

                        for (int j = 0; j < list.Count; j++)
                        {
                            if (nodes[list[j]].lenght == -1)
                            {
                                nodes[list[j]].lenght = 1;
                                order++;
                                q.Enqueue(list[j]);
                            }

                        }
                    }
                }

                if (order != 0)
                {
                    if (result.ContainsKey(order))
                        result[order]++;
                    else
                        result.Add(order, 1);
                }
            }

            return result;
        }

        protected override SortedDictionary<Double, Double> CalculateDistanceDistribution()
        {
            CheckPathEngine();
            Debug.Assert(pathEngine != null);
            return pathEngine.DistanceDistribution;
        }

        protected override SortedDictionary<Double, Double> CalculateTriangleByVertexDistribution()
        {
            CheckPathEngine();
            Debug.Assert(pathEngine != null);
            SortedDictionary<Double, Double> result = new SortedDictionary<Double, Double>();
            for (int i = 0; i < container.Size; ++i)
            {
                Double countTringle = pathEngine.CalculatedInformation[i];
                if (result.ContainsKey(countTringle))
                {
                    result[countTringle]++;
                }
                else
                {
                    result.Add(countTringle, 1);
                }
            }

            return result;
        }

        protected override SortedDictionary<Double, Double> CalculateCycles3Evolution()
        {
            RetrieveEvolutionResearchParameters(out Int32 s, out Double o, out Boolean p, out Int32 t);
            CheckPathEngine();
            Debug.Assert(pathEngine != null);
            NonHierarchicAnalyzerEvolutionEngine engine =
                new NonHierarchicAnalyzerEvolutionEngine(network, (NonHierarchicContainer)container, s, o, p, t, pathEngine.Cycles3);
            engine.Calculate();
            return engine.Cycles3Trajectory;            
        }

        protected override List<Double> CalculateDegreeCentrality() => container.Degrees.Select(i => (Double)i).ToList();

        protected override List<double> CalculateClosenessCentrality()
        {
            List<Double> result = new List<Double>(container.Size);
            int[] far = new int[container.Size];
            int[] d = new int[container.Size];

            for (int i = 0; i < container.Size; i++)
            {
                Queue<int> q = new Queue<int>();
                for (int j = 0; j < container.Size; j++)
                {
                    d[j] = -1;
                }
                q.Enqueue(i);
                d[i] = far[i] = 0;
                while (q.Count != 0)
                {
                    int v = q.Dequeue();
                    foreach (int w in container.GetAdjacentVertices(v))
                    {
                        if (d[w] < 0)
                        {
                            q.Enqueue(w);
                            d[w] = d[v] + 1;
                            far[i] = far[i] + d[w];
                        }
                    }
                }
                result.Add(1.0 / (double)far[i]);
            }

            return result;
        }

        protected override List<double> CalculateBetweennessCentrality()
        {
            List<Double> result = new List<Double>(new Double[container.Size]);
            double[] dist = new double[container.Size];
            double[] sigma = new double[container.Size];
            double[] delta = new double[container.Size];
            List<int>[] pred = new List<int>[container.Size];

            for (int i = 0; i < container.Size; i++)
            {
                //initialization part//
                Stack<int> s = new Stack<int>();
                Queue<int> q = new Queue<int>();

                for (int w = 0; w < container.Size; w++)
                {
                    dist[w] = -1;
                    sigma[w] = 0;
                    pred[w] = new List<int>();
                }
                dist[i] = 0;
                sigma[i] = 1;
                q.Enqueue(i);
                //end initialization part//

                while (q.Count != 0)
                {
                    int v = q.Dequeue();
                    s.Push(v);
                    foreach (int n in container.GetAdjacentVertices(v))
                    {
                        if (dist[n] < 0)
                        {
                            q.Enqueue(n);
                            dist[n] = dist[v] + 1;
                        }
                        if (dist[n] == dist[v] + 1)
                        {
                            sigma[n] = sigma[n] + sigma[v];
                            pred[n].Add(v);
                        }
                    }
                }
                for (int j = 0; j < container.Size; j++)
                {
                    delta[j] = 0;
                }

                //accumulation part//
                while (s.Count != 0)
                {
                    int w = s.Pop();
                    for (int j = 0; j < pred[w].Count; j++)
                    {
                        int t = pred[w][j];
                        delta[t] = delta[t] + (sigma[t] / sigma[w]) * (1 + delta[w]);
                    }
                    if (w != i)
                    {
                        result[w] = result[w] + delta[w];
                    }
                }
                //end accumulation part//
            }
            for (int i = 0; i < result.Count; i++)
            {
                result[i] = result[i] / 2;
            }

            return result;
        }

        private NonHierarchicAnalyzerPathEngine pathEngine;
        private NonHierarchicAnalyzerCoeffEngine coeffEngine;

        private void CheckPathEngine()
        {
            if (pathEngine == null)
            {
                pathEngine = new NonHierarchicAnalyzerPathEngine((NonHierarchicContainer)container);
                pathEngine.Calculate();
            }
        }        

        private void CheckCoeffEngine()
        {
            CheckPathEngine();
            Debug.Assert(pathEngine != null);
            if (coeffEngine == null)
            {                
                coeffEngine = new NonHierarchicAnalyzerCoeffEngine((NonHierarchicContainer)container, pathEngine.CalculatedInformation);
                coeffEngine.Calculate();
            }
        }

        private Double Get4OrderCyclesOfNode(int j)
        {
            List<int> neigboursList = container.GetAdjacentVertices(j);
            List<int> neigboursList1 = new List<int>(), neigboursList2 = new List<int>();
            Double count = 0;
            for (int i = 0; i < neigboursList.Count; i++)
            {
                neigboursList1 = container.GetAdjacentVertices(neigboursList[i]);
                for (int t = 0; t < neigboursList1.Count; t++)
                {
                    if (j != neigboursList1[t])
                    {
                        neigboursList2 = container.GetAdjacentVertices(neigboursList1[t]);
                        for (int k = 0; k < neigboursList2.Count; k++)
                            if (container.AreConnected(neigboursList2[k], j) &&
                                neigboursList2[k] != neigboursList1[t] &&
                                neigboursList2[k] != neigboursList[i])
                                count++;
                    }
                }
            }

            return count / 2;
        }     
    }
}
