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
    class ConnectedHierarchicGenerator : AbstractNetworkGenerator
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
            Debug.Assert(genParam.ContainsKey(GenerationParameter.Level));
            Debug.Assert(genParam.ContainsKey(GenerationParameter.Mu));

            Int32 branchincgIndex = Convert.ToInt32(genParam[GenerationParameter.BranchingIndex]);
            Int32 level = Convert.ToInt32(genParam[GenerationParameter.Level]);
            Double mu = Double.Parse(genParam[GenerationParameter.Mu].ToString(), CultureInfo.InvariantCulture);

            if (mu < 0)
                throw new InvalidGenerationParameters();

            container.Size = (Int32)Math.Pow(branchincgIndex, level);

            // TODO add connections
        }

        private RNGCrypto rand = new RNGCrypto();
    }
}
