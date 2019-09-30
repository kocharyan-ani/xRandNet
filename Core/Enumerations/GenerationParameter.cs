using System;

using Core.Utility;
using Core.Model;
using Core.Attributes;

namespace Core.Enumerations
{
    /// <summary>
    /// Enumeration of generation parameters that should be used for generating of a random network.
    /// Uses Attribute GenerationParameterInfo for storing metadata about every generation parameter.
    /// This metadata is used mainly for validating and getting user-friendly information.
    /// </summary>
    public enum GenerationParameter
    {
        [GenerationParameterInfo("N - Count of vertices", 
            "The initial count of vertices in the network.", 
            typeof(Int32), "24")]
        Vertices,

        [GenerationParameterInfo("p - Connectivity probability", 
            "The probability of existance of a connection between the nodes in the network.", 
            typeof(Double), "0.1")]
        Probability,

        [GenerationParameterInfo("Permanent network", 
            "Defines if the initial network is permanent for each generation step.", 
            typeof(Boolean), "false")]
        PermanentNetwork,

        [GenerationParameterInfo("E - Count of edges", 
            "The initial count of edges in the network.", 
            typeof(Int32), "1")]
        Edges,

        [GenerationParameterInfo("Step count", 
            "The count of generation steps to get each network in the ensemble.", 
            typeof(Int32), "1")]
        StepCount,

        [GenerationParameterInfo("b - Branching index", 
            "The branching index of the block-hierarchical network.", 
            typeof(Int32), "2")]
        BranchingIndex,

        [GenerationParameterInfo("Γ - Level", 
            "The level of the block-hierarchical network.", 
            typeof(Int32), "1")]
        Level,

        [GenerationParameterInfo("μ - Density", 
            "The density parameter of the block-hierarchical network.", 
            typeof(Double), "0.1")]
        Mu,

        [GenerationParameterInfo("M0 - Count of vertices on zero level",
            "Count of vertices on zero level.",
            typeof(Int32), "3")]
        ZeroLevelNodesCount,

        [GenerationParameterInfo("α",
            "Random parameter of IM network",
            typeof(Double), "0.1")]
        Alpha,

        [GenerationParameterInfo("b",
            "Count of blocks.",
            typeof(Int32), "2")]
        BlocksCount,

        [GenerationParameterInfo("Make Connected",
            "",
            typeof(Boolean), "true")]
        MakeConnected,

        [GenerationParameterInfo("Fitness Density Function",
            "",
            // TODO change to string
            typeof(String), "1")]
        FitnessDensityFunction,

        [GenerationParameterInfo("Adjacency matrix",
            "Adjacency matrix and branches of the networks.",
            typeof(MatrixPath), null)]
        AdjacencyMatrix
    }
}
