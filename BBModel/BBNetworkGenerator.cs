using System;
using System.Collections.Generic;
using System.Globalization;
using System.Diagnostics;

using Core.Enumerations;
using Core.Model;
using Core.Exceptions;
using NetworkModel;
using RandomNumberGeneration;

namespace BBModel
{
    /// <summary>
    /// Implementation of generator of random network of Baraba´si-Albert's model.
    /// </summary>
    class BBNetworkGenerator : AbstractNetworkGenerator
    {
        private NonHierarchicContainer container = new NonHierarchicContainer();

        public override INetworkContainer Container
        {
            get { return container; }
            set { container = (NonHierarchicContainer)value; }
        }

        public override void RandomGeneration(Dictionary<GenerationParameter, Object> genParam)
        {
            // TODO
            Debug.Assert(genParam.ContainsKey(GenerationParameter.Vertices));
            Debug.Assert(genParam.ContainsKey(GenerationParameter.Edges));
            Debug.Assert(genParam.ContainsKey(GenerationParameter.Probability));
            Debug.Assert(genParam.ContainsKey(GenerationParameter.StepCount));

            Int32 numberOfVertices = Convert.ToInt32(genParam[GenerationParameter.Vertices]);
            Int32 edges = Convert.ToInt32(genParam[GenerationParameter.Edges]);
            Double probability = Double.Parse(genParam[GenerationParameter.Probability].ToString(), CultureInfo.InvariantCulture);
            Int32 stepCount = Convert.ToInt32(genParam[GenerationParameter.StepCount]);

            if ((probability < 0 || probability > 1) || (edges > numberOfVertices))
                throw new InvalidGenerationParameters();

            container.Size = 0;
            Generate();
        }

        private RNGCrypto rand = new RNGCrypto();

        private void Generate()
        {
        }        
    }
}
