﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Core;
using Core.Enumerations;
using Core.Attributes;

namespace NonRegularHierarchicModel
{
    /// <summary>
    /// Implementation of non regularly branching block-hierarchic network.
    /// </summary>
    [RequiredGenerationParameter(GenerationParameter.AdjacencyMatrix)]
    [RequiredGenerationParameter(GenerationParameter.Vertices)]
    [RequiredGenerationParameter(GenerationParameter.BranchingIndex)]
    [RequiredGenerationParameter(GenerationParameter.Mu)]
    [AvailableAnalyzeOption(AnalyzeOption.AvgClusteringCoefficient
        | AnalyzeOption.AvgDegree
        | AnalyzeOption.AvgPathLength
        | AnalyzeOption.ClusteringCoefficientDistribution
        | AnalyzeOption.ClusteringCoefficientPerVertex
        | AnalyzeOption.ConnectedComponentDistribution
        | AnalyzeOption.Cycles3
        | AnalyzeOption.Cycles4
        | AnalyzeOption.DegreeDistribution
        | AnalyzeOption.Diameter
        | AnalyzeOption.DistanceDistribution
        | AnalyzeOption.EigenDistanceDistribution
        | AnalyzeOption.EigenValues
        | AnalyzeOption.LaplacianEigenValues
        | AnalyzeOption.TriangleByVertexDistribution
        )]
    public class NonRegularHierarchicNetwork : AbstractNetwork
    {
        public NonRegularHierarchicNetwork(String rName,
            ResearchType rType,
            GenerationType gType,
            TracingType tType,
            Dictionary<ResearchParameter, Object> rParams,
            Dictionary<GenerationParameter, Object> genParams,
            AnalyzeOption analyzeOpts) : base(rName, rType, gType, tType, rParams, genParams, analyzeOpts)
        {
            networkGenerator = new NonRegularHierarchicNetworkGenerator();
            networkAnalyzer = new NonRegularHierarchicNetworkAnalyzer(this);
        }
    }
}
