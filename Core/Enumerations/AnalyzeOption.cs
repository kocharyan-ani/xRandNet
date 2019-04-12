﻿using System;
using System.Collections.Generic;

using Core.Attributes;

namespace Core.Enumerations
{
    /// <summary>
    /// Flags enumaration, used for indicating which statistical properties
    /// should be analyzed during the Research run.
    /// Uses Attribute AnalyzeOptionInfo for storing metadata about every option.
    /// This metadata is used mainly for getting user-friendly information.
    /// </summary>
    [Flags]
    public enum AnalyzeOption : UInt64
    {
        [AnalyzeOptionInfo("None", 
            "Indication of empty selection.",
            OptionType.Global)]
        None = 0x0,

        // Global properties. //

        [AnalyzeOptionInfo("Average path length", 
            "The average length of the shortest paths for all possible pairs in the network.",
            OptionType.Global)]
        AvgPathLength = 0x01,

        [AnalyzeOptionInfo("Diameter", 
            "The longest shortest path between any two nodes in the network.",
            OptionType.Global)]
        Diameter = 0x02,

        [AnalyzeOptionInfo("Average degree", 
            "The average value of the degrees of nodes in the network.",
            OptionType.Global)]
        AvgDegree = 0x04,

        [AnalyzeOptionInfo("Average clustering coefficient", 
            "The average value of the clustering coefficients of nodes in the network.",
            OptionType.Global)]
        AvgClusteringCoefficient = 0x08,

        [AnalyzeOptionInfo("3-length cycles", 
            "Count of cycles of length 3 in the network.",
            OptionType.Global)]
        Cycles3 = 0x10,

        [AnalyzeOptionInfo("4-length cycles",
            "Count of cycles of length 4 in the network.",
            OptionType.Global)]
        Cycles4 = 0x20,

        [AnalyzeOptionInfo("5-length cycles",
            "Count of cycles of length 5 in the network.",
            OptionType.Global)]
        Cycles5 = 0x40,

        /*[AnalyzeOptionInfo("Dr",
            "Dr",
            OptionType.Global)]
        Dr = 0x80,*/

        // Eigenvalues spectra properties. //

        [AnalyzeOptionInfo("Eigenvalues",
            "The spectrum of network's adjacency matrix’s eigenvalues.",
            OptionType.ValueList)]
        EigenValues = 0x100,

        [AnalyzeOptionInfo("Eigenvalues distances distribution",
            "The distribution of intervals between network's adjacency matrix’s eigenvalues.",
            OptionType.Distribution,
            "Distance", 
            "Count")]
        EigenDistanceDistribution = 0x200,

        [AnalyzeOptionInfo("Eigenvalues for Laplacian matrix",
            "Eigenvalues for Laplacian matrix.",
            OptionType.ValueList)]
        LaplacianEigenValues = 0x400,

        // Distributions. //

        [AnalyzeOptionInfo("Degree distribution", 
            "Network's node degree distribution.",
            OptionType.Distribution,
            "Degree",
            "AvgCount")]
        DegreeDistribution = 0x800,

        [AnalyzeOptionInfo("Clustering coefficients distribution", 
            "Network's node clustering coefficient distribution.",
            OptionType.Distribution,
            "Coefficient",
            "AvgCount")]
        ClusteringCoefficientDistribution = 0x1000,

        [AnalyzeOptionInfo("Clustering coefficient per vertex",
            "Clustering coefficient for each node of network.",
            OptionType.Distribution,
            "Vertex",
            "AvgCoefficient")]
        ClusteringCoefficientPerVertex = 0x2000,

        [AnalyzeOptionInfo("Connected component distribution",
            "Length distribution of the connected subnetworks in the network.",
            OptionType.Distribution,
            "Order",
            "AvgCount")]
        ConnectedComponentDistribution = 0x4000,

        [AnalyzeOptionInfo("Complete component distribution", 
            "Length distribution of the complete subnetworks in the network.",
            OptionType.Distribution,
            "Order",
            "AvgCount")]
        CompleteComponentDistribution = 0x8000,

        [AnalyzeOptionInfo("Subtree distribution",
            "Length distribution of the subtrees in the network.",
            OptionType.Distribution,
            "Order",
            "AvgCount")]
        SubtreeDistribution = 0x10000,

        [AnalyzeOptionInfo("Distance distribution", 
            "Node-node distance distribution in the network.",
            OptionType.Distribution,
            "Distance",
            "AvgCount")]
        DistanceDistribution = 0x20000,

        [AnalyzeOptionInfo("Triangle distribution", 
            "The distribution of cycles of length 3 (triangles), which contain the node x.",
            OptionType.Distribution,
            "TriangleCount",
            "AvgCount")]
        TriangleByVertexDistribution = 0x40000,

        /*[AnalyzeOptionInfo("DrDistribution",
            "Dr",
            OptionType.Distribution,
            "Dr",
            "DrV")]
        Dr = 0x80000,*/

        // Trajectories. //

        [AnalyzeOptionInfo("3-length cycles evolution",
            "Count of 3-length cycles in evolution process.",
            OptionType.Trajectory,
            "StepCount",
            "AvgCount")]
        Cycles3Evolution = 0x100000,

        [AnalyzeOptionInfo("Algorithm 1 from state - by all nodes",
            "Active part of vertices in activation process.",
            OptionType.Trajectory,
            "StepCount",
            "AvgCount")]
        Algorithm_1_By_All_Nodes = 0x200000,

        [AnalyzeOptionInfo("Algorithm 2 from state - by active nodes list",
            "Active part of vertices in activation process.",
            OptionType.Trajectory,
            "StepCount",
            "AvgCount")]
        Algorithm_2_By_Active_Nodes_List = 0x400000,

        [AnalyzeOptionInfo("Final algorithm",
            "Active part of vertices in activation process.",
            OptionType.Trajectory,
            "StepCount",
            "AvgCount")]
        Algorithm_Final = 0x800000,

        //Centralities//

        [AnalyzeOptionInfo("Degree Centrality",
             "Network's node Degree Centrality.",
             OptionType.Centrality)]
        DegreeCentrality = 0x1000000,

        [AnalyzeOptionInfo("Betweenness Centrality",
           "Network's node Betweenness Centrality.",
           OptionType.Centrality)]
        BetweennessCentrality = 0x2000000,

        [AnalyzeOptionInfo("Closeness Centrality",
           "Network's node Closeness Centrality.",
           OptionType.Centrality)]
        ClosenessCentrality = 0x4000000
    }
}
