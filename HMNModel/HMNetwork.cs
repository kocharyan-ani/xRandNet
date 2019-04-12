using System;
using System.Collections.Generic;

using Core;
using Core.Attributes;
using Core.Enumerations;
using NetworkModel;

namespace HMNModel
{
    [RequiredGenerationParameter(GenerationParameter.Vertices)]
    [RequiredGenerationParameter(GenerationParameter.Probability)]
    [RequiredGenerationParameter(GenerationParameter.Alpha)]
    [RequiredGenerationParameter(GenerationParameter.ZeroLevelNodesCount)]
    [RequiredGenerationParameter(GenerationParameter.BlocksCount)]
    [RequiredGenerationParameter(GenerationParameter.MakeConnected)]
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
    public class HMNetwork : AbstractNetwork
    {
        public HMNetwork(String rName,
            ResearchType rType,
            GenerationType gType,
            TracingType tType,
            Dictionary<ResearchParameter, Object> rParams,
            Dictionary<GenerationParameter, Object> genParams,
            AnalyzeOption analyzeOpts) : base(rName, rType, gType, tType, rParams, genParams, analyzeOpts)
        {
            networkGenerator = new HMNetworkGenerator();
            networkAnalyzer = new NonHierarchicAnalyzer(this);
        }
    }
}
