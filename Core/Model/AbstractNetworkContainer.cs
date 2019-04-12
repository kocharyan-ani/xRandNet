using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

using RandomNumberGeneration;

namespace Core.Model
{
    public abstract class AbstractNetworkContainer : INetworkContainer, ICloneable
    {
        public abstract Int32 Size { get; set; }

        public abstract Int32 EdgesCount { get; }

        public abstract List<int> Degrees { get; }

        /// <summary>
        /// Sets given adjacency matrix on the network.
        /// </summary>
        /// <param name="matrix">Given adjacency matrix.</param>
        /// <note>Specific for each network model type.</note>
        public abstract void SetMatrix(BitArray[] matrix);

        /// <summary>
        /// Gets adjacency matrix for the network.
        /// </summary>
        /// <returns>Adjacency matrix.</returns>
        public BitArray[] GetMatrix()
        {
            BitArray[] matrix = new BitArray[Size];
            for (int i = 0; i < Size; ++i)
            {
                matrix[i] = new BitArray(Size);
                matrix[i][i] = false;
                for (int j = 0; j < i; ++j)
                {
                    matrix[i][j] = matrix[j][i] = AreConnected(i, j);
                }
            }
            return matrix;
        }

        /// <summary>
        /// Sets given neighbourship on the network.
        /// </summary>
        /// <param name="neighbours">Given neighbourship.</param>
        /// <param name="size">Size of network.</param>
        public virtual void SetNeighbourship(List<int> neighbours, int size)
        {
            Debug.Assert(size != 0);
            Debug.Assert(neighbours.Count() % 2 == 0);

            BitArray[] matrix = new BitArray[size];
            for (int i = 0; i < size; ++i)
                matrix[i] = new BitArray(size);
            
            for (int i = 0; i < neighbours.Count(); i += 2)
            {
                matrix[neighbours[i]][neighbours[i + 1]] = true;
                matrix[neighbours[i + 1]][neighbours[i]] = true;
            }
            SetMatrix(matrix);
        }
        
        /// <summary>
        /// Gets neighbourship information from the network.
        /// </summary>
        /// <returns>Neighbourship information.</returns>
        public List<KeyValuePair<int, int>> GetNeighbourship()
        {
            List<KeyValuePair<int, int>> n = new List<KeyValuePair<int, int>>();
            for (int i = 0; i < Size; i++)
            {
                for (int j = i + 1; j < Size; j++)
                {
                    if (AreConnected(i, j))
                    {
                        n.Add(new KeyValuePair<int, int>(i, j));
                    }
                }
            }
            return n;
        }

        /// <summary>
        /// Deep copies the network.
        /// </summary>
        /// <returns>Copy of network.</returns>
        public abstract Object Clone();

        /// <summary>
        /// TODO closed
        /// </summary>
        /// <param name="directoryName"></param>
        /// <param name="subDirectoryName"></param>
        /// <param name="fileName"></param>
        /*public void Trace(String directoryName, String subDirectoryName, String fileName)
        {
            String tracingDirectory = RandNetSettings.TracingDirectory;
            String dPath = tracingDirectory + "\\" + directoryName;
            String sdPath = dPath + "\\" + subDirectoryName;
            String fPath = sdPath + "\\" + fileName;
            if (tracingDirectory != "")
            {
                if (!Directory.Exists(dPath))
                    Directory.CreateDirectory(dPath);
                if (!Directory.Exists(sdPath))
                    Directory.CreateDirectory(sdPath);

                if (RandNetSettings.TracingType == TracingType.Matrix)
                {
                    MatrixInfoToWrite matrixInfo = new MatrixInfoToWrite();
                    matrixInfo.Matrix = GetMatrix();
                    matrixInfo.Branches = null;
                    matrixInfo.ActiveStates = GetActiveStatuses();
                    FileManager.Write(RandNetSettings.TracingDirectory, matrixInfo, fPath);
                }
                else if (RandNetSettings.TracingType == TracingType.Neighbourship)
                {
                    NeighbourshipInfoToWrite neighbourshipInfo = new NeighbourshipInfoToWrite();
                    neighbourshipInfo.Neighbourship = GetNeighbourship();
                    neighbourshipInfo.Branches = null;
                    neighbourshipInfo.ActiveStates = GetActiveStatuses();
                    FileManager.Write(RandNetSettings.TracingDirectory, neighbourshipInfo, fPath);
                }
            }
        }*/

        /// <summary>
        /// Checks if given vertices are connected in the network.
        /// </summary>
        /// <param name="v1">First vertex index.</param>
        /// <param name="v2">Second vertex index.</param>
        /// <returns></returns>
        public abstract bool AreConnected(int v1, int v2);

        /// <summary>
        /// Gets list of vertices, which are adjacent to given vertex.
        /// </summary>
        /// <param name="vertex">Given vertex.</param>
        /// <returns>List of adjacent vertices.</returns>
        public abstract List<int> GetAdjacentVertices(int vertex);

        /// <summary>
        /// Gets given vertex degree.
        /// </summary>
        /// <param name="vertex">Given vertex.</param>
        /// <returns>Degree.</returns>
        public abstract int GetVertexDegree(int vertex);

        /// <summary>
        /// Gets sum of all degrees in the network.
        /// </summary>
        /// <returns>Sum of degrees.</returns>
        public int GetVertexDegreeSum()
        {
            int sum = 0;
            for (int i = 0; i < Size; ++i)
                sum += GetVertexDegree(i);

            return sum;
        }

        #region Extra Information for Evolution Process

        protected class EdgesInformation : ICloneable
        {
            public List<KeyValuePair<int, int>> existingEdges = new List<KeyValuePair<int, int>>();
            public List<KeyValuePair<int, int>> nonExistingEdges = new List<KeyValuePair<int, int>>();

            public Object Clone()
            {
                EdgesInformation clone = new EdgesInformation();
                clone.existingEdges = new List<KeyValuePair<int, int>>(this.existingEdges);
                clone.nonExistingEdges = new List<KeyValuePair<int, int>>(this.nonExistingEdges);
                return clone;
            }

            public void Add(int i, int j)
            {
                KeyValuePair<int, int> e = new KeyValuePair<int, int>(i, j);
                existingEdges.Add(e);
                nonExistingEdges.Remove(e);
            }

            public void Remove(int i, int j)
            {
                KeyValuePair<int, int> e = new KeyValuePair<int, int>(i, j);
                existingEdges.Remove(e);
                nonExistingEdges.Add(e);
            }
        }

        protected EdgesInformation evolutionInformation;

        public void InitializeEvolutionInformation()
        {
            Debug.Assert(evolutionInformation == null);
            evolutionInformation = new EdgesInformation();

            for(int i = 0; i < Size; ++i)
            {
                for(int j = i + 1; j < Size; ++j)
                {
                    KeyValuePair<int, int> e = new KeyValuePair<int, int>(i, j);
                    if (AreConnected(i, j))
                    {
                        evolutionInformation.existingEdges.Add(e);
                    }
                    else
                    {
                        evolutionInformation.nonExistingEdges.Add(e);
                    }
                }
            }
        }

        #endregion

        #region Information fo Activation Process

        protected BitArray activeNodes;

        /// <summary>
        /// Gets active statuses of all nodes of the network.
        /// </summary>
        /// <returns>Active statuses.</returns>
        public BitArray GetActiveStatuses() => activeNodes;

        /// <summary>
        /// Sets given active statuses to all nodes of the network.
        /// </summary>
        /// <param name="act">Given active statuses.</param>
        public virtual void SetActiveStatuses(BitArray act)
        {
            Debug.Assert(act.Count == Size);
            activeNodes = act;
        }
        
        /// <summary>
        /// Sets random active statuses with given probability to all nodes of the network.
        /// </summary>
        /// <param name="p">Activation probability for each node.</param>
        public void RandomActivating(Double p)
        {
            RNGCrypto rand = new RNGCrypto();
            activeNodes = new BitArray(Size, false);
            for (Int32 i = 0; i < Size; ++i)
            {
                if (rand.NextDouble() <= p)
                {
                    activeNodes[i] = true;
                }
            }
        }

        /// <summary>
        /// Sets given active status on the given node of the network. 
        /// </summary>
        /// <param name="i">Given node number.</param>
        /// <param name="a">Given active status.</param>
        public void SetActiveStatus(int i, bool a)
        {
            Debug.Assert(activeNodes != null);
            Debug.Assert(i >= 0 && i < activeNodes.Count);
            activeNodes[i] = a;
        }

        /// <summary>
        /// Gets active status of the given node of the network.
        /// </summary>
        /// <param name="i">Given node number.</param>
        /// <returns>Active status.</returns>
        public bool GetActiveStatus(int i)
        {
            Debug.Assert(activeNodes != null);
            Debug.Assert(i >= 0 && i < activeNodes.Count);
            return activeNodes[i];
        }

        /// <summary>
        /// Gets active nodes count of the network.
        /// </summary>
        /// <returns>Active nodes count</returns>
        public int GetActiveNodesCount()
        {
            Debug.Assert(activeNodes != null);
            int c = 0;
            for (int i = 0; i < activeNodes.Count; ++i)
            {
                if (activeNodes[i])
                {
                    ++c;
                }
            }
            return c;
        }

        /// <summary>
        /// Checks if any active node exists in the network.
        /// </summary>
        /// <returns>true, if any active node existst in the network. false otherwise.</returns>
        public bool DoesActiveNodeExist()
        {
            Debug.Assert(activeNodes != null);
            for (int i = 0; i < activeNodes.Count; ++i)
            {
                if (activeNodes[i])
                {
                    return true;
                }
            }
            return false;
        }

        #endregion
    }
}
