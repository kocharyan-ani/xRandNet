using System;
using System.Collections.Generic;

namespace Core.Model
{
    /// <summary>
    /// Abstract class presenting container of hierarchic type.
    /// </summary>
    public abstract class AbstractHierarchicContainer : AbstractNetworkContainer
    {
        /// <summary>
        /// Gets branches for the graph.
        /// </summary>
        /// <returns>Branches by levels.</returns>
        public abstract List<List<int>> GetBranches();
    }
}
