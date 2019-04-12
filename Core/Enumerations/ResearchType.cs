using System;

using Core.Attributes;

namespace Core.Enumerations
{
    /// <summary>
    /// Enumaration, used for indicating Research types.
    /// Uses Attribute ResearchTypeInfo for storing metadata about every Research.
    /// </summary>
    public enum ResearchType
    {
        [ResearchTypeInfo("Basic research", 
            "The basic analysis for random networks (single ensemble).", 
            "Research.BasicResearch, Research")]
        Basic,

        [ResearchTypeInfo("Evolution research", 
            "Analysis the evolution process for random networks (several ensembles).", 
            "Research.EvolutionResearch, Research")]
        Evolution,

        [ResearchTypeInfo("Threshold research", 
            "Analysis of Threshold probability (critical probability) for random network (several ensembles).", 
            "Research.ThresholdResearch, Research")]
        Threshold,

        [ResearchTypeInfo("Collection of researches",
            "The basic analysis for several random network in catalog (several ensembles).",
            "Research.CollectionResearch, Research")]
        Collection,

        [ResearchTypeInfo("Structural research",
            "Analysis of communities random network (several ensembles).",
            "Research.StructuralResearch, Research")]
        Structural,

        [ResearchTypeInfo("Activation research",
            "Analysis of activation spreading process for random network (several ensembles).",
            "Research.ActivationResearch, Research")]
        Activation
    }
}