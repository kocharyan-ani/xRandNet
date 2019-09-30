using System;
using System.Collections.Generic;

using Core;
using Core.Model;

namespace HierarchicModel
{
    /// <summary>
    /// Implementation of generalized block-hierarchic network's analyzer.
    /// </summary>
    class HierarchicNetworkAnalyzer : AbstractNetworkAnalyzer
    {
        private HierarchicNetworkContainer container;

        public HierarchicNetworkAnalyzer(AbstractNetwork n) : base(n) { }

        public override INetworkContainer Container
        {
            get { return container; }
            set
            {
                abstractContainer = (HierarchicNetworkContainer)value;
                container = abstractContainer as HierarchicNetworkContainer;
            }
        }

        protected override Double CalculateAveragePath()
        {
            throw new NotImplementedException();
        }

        protected override Double CalculateDiameter()
        {
            throw new NotImplementedException();
        }

        protected override Double CalculateAverageClusteringCoefficient()
        {
            throw new NotImplementedException();
        }

        protected override Double CalculateCycles3()
        {
            throw new NotImplementedException();
        }

        protected override Double CalculateCycles4()
        {
            throw new NotImplementedException();
        }

        protected override Double CalculateCycles5()
        {
            throw new NotImplementedException();
        }

        protected override SortedDictionary<Double, Double> CalculateDegreeDistribution()
        {
            throw new NotImplementedException();
        }

        protected override SortedDictionary<Double, Double> CalculateClusteringCoefficientDistribution()
        {
            throw new NotImplementedException();
        }
                
        protected override SortedDictionary<Double, Double> CalculateClusteringCoefficientPerVertex()
        {
            throw new NotImplementedException();
        }

        protected override SortedDictionary<Double, Double> CalculateConnectedComponentDistribution()
        {
            throw new NotImplementedException();
        }

        protected override SortedDictionary<Double, Double> CalculateDistanceDistribution()
        {
            throw new NotImplementedException();
        }

        protected override SortedDictionary<Double, Double> CalculateTriangleByVertexDistribution()
        {
            throw new NotImplementedException();
        }
    }
}
