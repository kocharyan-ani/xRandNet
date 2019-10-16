using System;

namespace CoreTypes
{
    public class ModelType
    {
        public String FullName { get; private set; }
        public String Description { get; private set; }
        public Type Implementation { get; private set; }

        public ModelType(String fullName, String description, Type implementation)
        {
            FullName = fullName;
            Description = description;
            Implementation = implementation;
        }

        public override string ToString() => FullName;
    }

    public class ModelTypes
    {
        public readonly ModelType ER = new ModelType("Erdős-Rényi model",
                                                   "The classical random network.",
                                                   Type.GetType("ERModel.ERNetwork, ERModel", true));
    }

    /*public class WSModelType : ModelType
    {
        public override String FullName => "Watts-Strogatz model";
        public override String Description => "Random network, which represents the small-world property.";
        public override Type Implementation => Type.GetType("WSModel.WSNetwork, WSModel", true);
    }

    public class BAModelType : ModelType
    {
        public override String FullName => "Baraba´si-Albert model";
        public override String Description => "Random network, which represents the scale-free property.";
        public override Type Implementation => Type.GetType("BAModel.BANetwork, BAModel", true);
    }

    public class BBModelType : ModelType
    {
        public override String FullName => "Biancony-Baraba´si model";
        public override String Description => "Random network, which represents the scale-free property.";
        public override Type Implementation => Type.GetType("BBModel.BBNetwork, BBModel", true);
    }

    public class RegularHierarchicModelType : ModelType
    {
        public override String FullName => "Regular Hierarchical model";
        public override String Description => "Random regularly branching block-hierarchical network.";
        public override Type Implementation => Type.GetType("RegularHierarchicModel.RegularHierarchicNetwork, RegularHierarchicModel", true);
    }

    public class NonRegularHierarchicModelType : ModelType
    {
        public override String FullName => "Non Regular Hierarchical model";
        public override String Description => "Random non-regularly branching block-hierarchical network.";
        public override Type Implementation => Type.GetType("NonRegularHierarchicModel.NonRegularHierarchicNetwork, NonRegularHierarchicModel", true);
    }

    public class GeneralizedHierarchicModelType : ModelType
    {
        public override String FullName => "Generalized Hierarchical model";
        public override String Description => "Random generalized block-hierarchical network.";
        public override Type Implementation => Type.GetType("HierarchicModel.HierarchicNetwork, HierarchicModel", true);
    }

    public class HMNModelType : ModelType
    {
        public override String FullName => "HMN-1 model";
        public override String Description => "Random hierarchical modular networks (HMN-1)";
        public override Type Implementation => Type.GetType("HMNModel.HMNetwork, HMNModel", true);
    }*/
}