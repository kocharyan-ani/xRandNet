using System;

using Core.Attributes;

namespace Core.Enumerations
{
    /// <summary>
    /// Enumaration, used for indicating the model type for Research.
    /// Uses Attribute ModelTypeInfo for storing metadata about every model.
    /// </summary>
    public enum ModelType
    {
        [ModelTypeInfo("Erdős-Rényi model", 
            "The classical random network.", 
            "ERModel.ERNetwork, ERModel")]
        ER,

        [ModelTypeInfo("Watts-Strogatz model",
            "Random network, which represents the small-world property.", 
            "WSModel.WSNetwork, WSModel")]
        WS,

        [ModelTypeInfo("Baraba´si-Albert model",
            "Random network, which represents the scale-free property.",
            "BAModel.BANetwork, BAModel")]
        BA,

        [ModelTypeInfo("Regular Hierarchical model", 
            "Random regularly branching block-hierarchical network.",
            "RegularHierarchicModel.RegularHierarchicNetwork, RegularHierarchicModel")]
        RegularHierarchic,

        [ModelTypeInfo("Regular Hierarchical model, L-links",
            "Random regularly branching block-hierarchical network with L-links.",
            "RegularHierarchicModelLLinks.RegularHierarchicNetworkLLinks, RegularHierarchicLLinks")]
        RegularHierarchicLLinks,

        [ModelTypeInfo("Non Regular Hierarchical model", 
            "Random non-regularly branching block-hierarchical network.",
            "NonRegularHierarchicModel.NonRegularHierarchicNetwork, NonRegularHierarchicModel")]
        NonRegularHierarchic,

        [ModelTypeInfo("HMN-1 model",
            "Random hierarchical modular networks (HMN-1)",
            "HMNModel.HMNetwork, HMNModel")]
        HMN
    }
}
