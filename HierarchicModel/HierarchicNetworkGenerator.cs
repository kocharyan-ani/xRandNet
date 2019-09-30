using System;
using System.Collections.Generic;

using Core.Enumerations;
using Core.Model;
using RandomNumberGeneration;

namespace HierarchicModel
{
    /// <summary>
    /// Implementation of generalized block-hierarchic network's generator.
    /// </summary>
    class HierarchicNetworkGenerator : AbstractNetworkGenerator
    {
        /// <summary>
        /// Container with network of specified model (generalized block-hierarchic).
        /// </summary>
        private HierarchicNetworkContainer container = new HierarchicNetworkContainer();

        public override INetworkContainer Container
        {
            get { return container; }
            set { container = (HierarchicNetworkContainer)value; }
        }

        public override void RandomGeneration(Dictionary<GenerationParameter, Object> genParam)
        {
            throw new NotImplementedException();
        }

        private RNGCrypto rand = new RNGCrypto();
    }
}
