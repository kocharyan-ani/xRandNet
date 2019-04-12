using System;
using System.Collections.Generic;
using System.Diagnostics;

using Model.Eigenvalues;

namespace Core.Model.Engines
{
    /// <summary>
    /// Utility engine class, which calculates eigen value options for non hierarchical network.
    /// <options>Options are
    ///     EigenValues
    ///     EigenDistanceDistribution     
    /// </options>
    /// </summary>
    class AnalyzerEigenValuesEngine
    {
        private readonly INetworkContainer container;
        private EigenValueUtils eigenValueUtility = new EigenValueUtils();

        public List<Double> EigenValues { get; private set; }
        public SortedDictionary<Double, Double> EigenDistances { get; private set; }

        public AnalyzerEigenValuesEngine(INetworkContainer c)
        {
            container = c;
            EigenValues = new List<Double>();
            EigenDistances = new SortedDictionary<Double, Double>();
        }

        public void Calculate()
        {
            try
            {
                EigenValues = eigenValueUtility.CalculateEigenValue(container.GetMatrix());
                EigenDistances = eigenValueUtility.CalcEigenValuesDist(EigenValues);
            }
            catch (SystemException)
            {
                Debug.Assert(false, "Exceptions is thrown while calculating eigen values.");
            }
        }
    }
}
