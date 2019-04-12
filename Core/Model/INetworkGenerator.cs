﻿using System;
using System.Collections.Generic;

using Core.Enumerations;
using Core.Utility;

namespace Core.Model
{
    /// <summary>
    /// Interface for random network generator.
    /// </summary>
    public interface INetworkGenerator
    {
        /// <summary>
        /// Generated network's container.
        /// </summary>
        INetworkContainer Container { get; set; }

        /// <summary>
        /// Generates random network with specified generation parameters.
        /// </summary>
        /// <param name="genParam">Generation parameter values.</param>
        void RandomGeneration(Dictionary<GenerationParameter, Object> genParam);

        /// <summary>
        /// Generates network from input network information.
        /// </summary>
        /// <param name="info"></param>
        void StaticGeneration(NetworkInfoToRead info);
    }
}
