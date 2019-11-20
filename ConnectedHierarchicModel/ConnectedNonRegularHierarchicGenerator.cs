using System;
using System.Collections.Generic;
using System.Globalization;
using System.Diagnostics;

using Core.Enumerations;
using Core.Model;
using Core.Exceptions;
using NetworkModel;
using RandomNumberGeneration;

namespace ConnectedHierarchicModel
{
    /// <summary>
    /// Implementation of generator of random network of TODO
    /// </summary>
    class ConnectedNonRegularHierarchicGenerator : AbstractNetworkGenerator
    {
        private NonHierarchicContainer container = new NonHierarchicContainer();

        public override INetworkContainer Container
        {
            get { return container; }
            set { container = (NonHierarchicContainer)value; }
        }

        public override void RandomGeneration(Dictionary<GenerationParameter, Object> genParam)
        {
            Debug.Assert(genParam.ContainsKey(GenerationParameter.BranchingIndex));
            Debug.Assert(genParam.ContainsKey(GenerationParameter.Vertices));
            Debug.Assert(genParam.ContainsKey(GenerationParameter.Mu));

            Int32 branchincgIndex = Convert.ToInt32(genParam[GenerationParameter.BranchingIndex]);
            Int32 vertices = Convert.ToInt32(genParam[GenerationParameter.Vertices]);
            Double mu = Double.Parse(genParam[GenerationParameter.Mu].ToString(), CultureInfo.InvariantCulture);

            if (mu < 0)
                throw new InvalidGenerationParameters();

            container.Size = vertices;

            // TODO add connections
        }

        private RNGCrypto rand = new RNGCrypto();
    }
}
