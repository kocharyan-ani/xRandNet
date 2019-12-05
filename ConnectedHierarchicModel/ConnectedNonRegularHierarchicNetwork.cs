using System;
using System.Collections.Generic;

using Core;
using Core.Attributes;
using Core.Enumerations;
using NetworkModel;

namespace ConnectedHierarchicModel
{
    /// <summary>
    /// Implementation of random network TODO.
    /// </summary>
    [RequiredGenerationParameter(GenerationParameter.AdjacencyMatrix)]
    [RequiredGenerationParameter(GenerationParameter.BranchingIndex)]
    [RequiredGenerationParameter(GenerationParameter.Vertices)]
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
        // Trajectories //
        | AnalyzeOption.Algorithm_1_By_All_Nodes
        | AnalyzeOption.Algorithm_2_By_Active_Nodes_List
        | AnalyzeOption.Algorithm_Final
        // Centralities //
        | AnalyzeOption.DegreeCentrality
        | AnalyzeOption.ClosenessCentrality
        | AnalyzeOption.BetweennessCentrality
        )]
    public class ConnectedNonRegularHierarchicNetwork : AbstractNetwork
    {
        public ConnectedNonRegularHierarchicNetwork(String rName,
            ResearchType rType,
            GenerationType gType,
            TracingType tType,
            Dictionary<ResearchParameter, Object> rParams,
            Dictionary<GenerationParameter, Object> genParams,
            AnalyzeOption analyzeOpts) : base(rName, rType, gType, tType, rParams, genParams, analyzeOpts)
        {
            networkGenerator = new ConnectedNonRegularHierarchicGenerator();
            networkAnalyzer = new NonHierarchicAnalyzer(this);
        }
    }
}
