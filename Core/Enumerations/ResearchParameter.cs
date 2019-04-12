using System;

using Core.Attributes;
using Core.Utility;

namespace Core.Enumerations
{
    /// <summary>
    /// Enumeration of research parameters that should be used for Research run.
    /// Uses Attribute ResearchParameterInfo for storing metadata about every research parameter.
    /// This metadata is used mainly for validating and getting user-friendly information.
    /// </summary>
    public enum ResearchParameter
    {
        [ResearchParameterInfo("Count of evolution steps",
            "Count of evolution steps.",
            typeof(Int32), "1")]
        EvolutionStepCount,

        [ResearchParameterInfo("ω",
            "Probability parameter in evolution process.",
            typeof(Double), "0.1")]
        Omega,

        [ResearchParameterInfo("Tracing step increment",
            "Tracing step increment.",
            typeof(Int32), "0")]
        TracingStepIncrement,

        [ResearchParameterInfo("Permanent degree distribution",
            "Degree distribution is permanent during evolution process.",
            typeof(Boolean), "true")]
        PermanentDistribution,

        [ResearchParameterInfo("Max probability",
            "Maximal value of probability parameter while searching percolation.",
            typeof(Double), "1.0")]
        ProbabilityMax,

        [ResearchParameterInfo("Delta of probability",
            "Delta of probability parameter change while searching percolation.",
            typeof(Double), "0.01")]
        ProbabilityDelta,

        [ResearchParameterInfo("μ",
            "Deactivation speed in activation spreading process.",
            typeof(Double), "0.1")]
        DeactivationSpeed,

        [ResearchParameterInfo("λ",
            "Activation speed in activation spreading process",
            typeof(Double), "0.1")]
        ActivationSpeed,

        [ResearchParameterInfo("Count of activation steps",
            "Count of activation steps.",
            typeof(Int32), "10")]
        ActivationStepCount,

        [ResearchParameterInfo("Initial activation probability",
            "Initial activation probability in activation spreading process",
            typeof(Double), "1")]
        InitialActivationProbability,

        [ResearchParameterInfo("Research type",
            "Research type for each research in collection.",
            typeof(ResearchType), "BasicResearch")]
        ResearchItemType,

        [ResearchParameterInfo("Input path",
            "Path for input data.",
            typeof(MatrixPath), null)]
        InputPath
    }
}
