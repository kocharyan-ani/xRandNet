using System;
using System.Collections.Generic;

using Core;
using Core.Attributes;
using Core.Enumerations;

namespace RegularHierarchicModel
{
    /// <summary>
    /// Implementation of regularly branching block-hierarchic network.
    /// </summary>
    [RequiredGenerationParameter(GenerationParameter.AdjacencyMatrix)]
    [RequiredGenerationParameter(GenerationParameter.BranchingIndex)]
    [RequiredGenerationParameter(GenerationParameter.Level)]
    [RequiredGenerationParameter(GenerationParameter.Mu)]
    [AvailableAnalyzeOption(
        // Globals //
          AnalyzeOption.AvgPathLength
        | AnalyzeOption.Diameter
        | AnalyzeOption.AvgDegree
        | AnalyzeOption.AvgClusteringCoefficient
        | AnalyzeOption.Cycles3
        | AnalyzeOption.Cycles4
        // Eigens //
        | AnalyzeOption.EigenDistanceDistribution
        | AnalyzeOption.EigenValues
        | AnalyzeOption.LaplacianEigenValues
        // Distributions //
        | AnalyzeOption.DegreeDistribution
        | AnalyzeOption.ClusteringCoefficientDistribution
        | AnalyzeOption.ConnectedComponentDistribution
        | AnalyzeOption.ClusteringCoefficientPerVertex
        | AnalyzeOption.DistanceDistribution
        | AnalyzeOption.TriangleByVertexDistribution
        )]
    public class RegularHierarchicNetwork : AbstractNetwork
    {
        public RegularHierarchicNetwork(String rName,
            ResearchType rType,
            GenerationType gType,
            TracingType tType,
            Dictionary<ResearchParameter, Object> rParams,
            Dictionary<GenerationParameter, Object> genParams,
            AnalyzeOption analyzeOpts) : base(rName, rType, gType, tType, rParams, genParams, analyzeOpts)
        {
            networkGenerator = new RegularHierarchicNetworkGenerator();
            networkAnalyzer = new RegularHierarchicNetworkAnalyzer(this);
        }
    }
}
