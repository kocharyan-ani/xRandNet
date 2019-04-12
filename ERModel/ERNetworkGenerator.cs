using System;
using System.Collections.Generic;
using System.Globalization;
using System.Diagnostics;

using Core.Model;
using Core.Enumerations;
using Core.Exceptions;
using NetworkModel;
using RandomNumberGeneration;

namespace ERModel
{
    /// <summary>
    /// Implementation of generator of random network of Erdős-Rényi's model.
    /// </summary>
    class ERNetworkGenerator : AbstractNetworkGenerator
    {
        private NonHierarchicContainer container = new NonHierarchicContainer();
     
        public override INetworkContainer Container
        {
            get { return container; }
            set { container = (NonHierarchicContainer)value; }
        }

        public override void RandomGeneration(Dictionary<GenerationParameter, Object> genParam)
        {
            Debug.Assert(genParam.ContainsKey(GenerationParameter.Vertices));
            Debug.Assert(genParam.ContainsKey(GenerationParameter.Probability));

            Int32 numberOfVertices = Convert.ToInt32(genParam[GenerationParameter.Vertices]);
            Double probability = Double.Parse(genParam[GenerationParameter.Probability].ToString(), CultureInfo.InvariantCulture);

            if (probability < 0 || probability > 1)
                throw new InvalidGenerationParameters();
            
            container.Size = numberOfVertices;           
            FillValuesByProbability(probability);
        }

        private RNGCrypto rand = new RNGCrypto();

        private void FillValuesByProbability(double p)
        {            
            for (int i = 0; i < container.Size; ++i)
            {
                for (int j = i + 1; j < container.Size; ++j)
                {
                    if (rand.NextDouble() < p)
                    {
                        container.AddConnection(i, j);
                    }
                }
            }
        }
    }
}
