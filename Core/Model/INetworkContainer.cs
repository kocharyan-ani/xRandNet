using System;
using System.Collections;
using System.Collections.Generic;

namespace Core.Model
{
    /// <summary>
    /// Interface for random network container.
    /// </summary>
    public interface INetworkContainer
    {
        /// <summary>
        /// The size of the network (number if vertices).
        /// </summary>
        Int32 Size { get; set; }

        /// <summary>
        /// Count of edges of the network.
        /// </summary>
        Int32 EdgesCount { get; }
        
        /// <summary>
        /// Constucts a network on the base of a adjacency matrix.
        /// </summary>
        /// <param name="matrix">Adjacency matrix.</param>
        void SetMatrix(BitArray[] matrix);

        /// <summary>
        /// Constructs adjacency matrix for the network.
        /// </summary>
        /// <returns>Adjacency matrix.</returns>
        BitArray[] GetMatrix();

        /// <summary>
        /// Constucts a network on the base of neighbours list.
        /// </summary>
        /// <param name="neighbours">Neighbours list.</param>
        /// <param name="size">Size of graph (number of vertices).</param>
        void SetNeighbourship(List<int> neighbours, int size);

        /// <summary>
        /// Constructs neighbourships for the network.
        /// </summary>
        /// <returns>Neighbourship pairs.</returns>
        List<KeyValuePair<int, int>> GetNeighbourship();

        /// <summary>
        /// Set active states for network vertices.
        /// </summary>
        /// <param name="act"></param>
        void SetActiveStatuses(BitArray act);
    }
}
