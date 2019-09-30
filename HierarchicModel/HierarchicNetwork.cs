using System;
using System.Collections.Generic;

using Core;
using Core.Attributes;
using Core.Enumerations;

namespace HierarchicModel
{
    /// <summary>
    /// Implementation of generalized block-hierarchic network.
    /// </summary>
    [RequiredGenerationParameter(GenerationParameter.AdjacencyMatrix)]
    [RequiredGenerationParameter(GenerationParameter.BranchingIndex)]
    [RequiredGenerationParameter(GenerationParameter.Level)]
    [RequiredGenerationParameter(GenerationParameter.Mu)]
    [AvailableAnalyzeOption(
          // Globals //
          AnalyzeOption.AvgDegree
        | AnalyzeOption.AvgClusteringCoefficient
        // Eigens //
        | AnalyzeOption.EigenDistanceDistribution
        | AnalyzeOption.EigenValues
        | AnalyzeOption.LaplacianEigenValues
        // Distributions //
        | AnalyzeOption.DegreeDistribution
        | AnalyzeOption.ClusteringCoefficientDistribution
        | AnalyzeOption.ClusteringCoefficientPerVertex
        )]
    public class HierarchicNetwork : AbstractNetwork
    {
        public HierarchicNetwork(String rName,
            ResearchType rType,
            GenerationType gType,
            TracingType tType,
            Dictionary<ResearchParameter, Object> rParams,
            Dictionary<GenerationParameter, Object> genParams,
            AnalyzeOption analyzeOpts) : base(rName, rType, gType, tType, rParams, genParams, analyzeOpts)
        {
            networkGenerator = new HierarchicNetworkGenerator();
            networkAnalyzer = new HierarchicNetworkAnalyzer(this);
        }
    }
}
