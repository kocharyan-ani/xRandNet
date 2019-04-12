using System;
using System.Collections.Generic;
using System.Linq;

namespace NetworkModel.Engines
{
    /// <summary>
    /// Utility engine class, which calculates several options for non hierarchical network.
    /// <options>Options are
    ///     ClusteringCoefficientDistribution
    ///     ClusteringCoefficientPerVertex     
    /// </options>
    /// <note>Uses NonHierarchicAnalyzerPathEngine.CalculatedInformation</note>
    /// </summary>
    class NonHierarchicAnalyzerCoeffEngine
    {
        private readonly NonHierarchicContainer container;
        private readonly List<double> edgesBetweenNeighbours;

        public SortedDictionary<Double, Double> CoefficientsDistribution { get; private set; }
        public SortedDictionary<Double, Double> CoefficientsPerVertex { get; private set; }

        public NonHierarchicAnalyzerCoeffEngine(NonHierarchicContainer c, List<double> info)
        {
            container = c;
            edgesBetweenNeighbours = info;
            CoefficientsDistribution = new SortedDictionary<Double, Double>();
            CoefficientsPerVertex = new SortedDictionary<Double, Double>();
        }

        public void Calculate()
        {
            double iclusteringCoefficient = 0;
            int iEdgeCountForFullness = 0, iNeighbourCount = 0;

            for (int i = 0; i < container.Size; ++i)
            {
                iNeighbourCount = container.GetVertexDegree(i);
                if (iNeighbourCount != 0)
                {
                    iEdgeCountForFullness = (iNeighbourCount == 1) ? 1 : iNeighbourCount * (iNeighbourCount - 1) / 2;
                    iclusteringCoefficient = (edgesBetweenNeighbours[i]) / iEdgeCountForFullness;
                    CoefficientsPerVertex.Add(i, Math.Round(iclusteringCoefficient, 3));
                }
                else
                    CoefficientsPerVertex.Add(i, 0);
            }

            for (int i = 0; i < container.Size; ++i)
            {
                double result = CoefficientsPerVertex[(uint)i];
                if (CoefficientsDistribution.Keys.Contains(result))
                    CoefficientsDistribution[CoefficientsPerVertex[i]]++;
                else
                    CoefficientsDistribution[CoefficientsPerVertex[i]] = 1;
            }
        }
    }
}
