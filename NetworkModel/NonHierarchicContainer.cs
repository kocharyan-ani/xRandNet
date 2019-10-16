using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

using Core.Model;
using RandomNumberGeneration;

namespace NetworkModel
{
    public class NonHierarchicContainer : AbstractNetworkContainer
    {
        public override Int32 Size
        {
            get
            {
                return size;
            }
            set
            {
                size = value;
                if (size > maxSize)
                {
                    maxSize = (size > MAX_SIZE) ? size : MAX_SIZE;
                    data = new BitArray[maxSize];
                    for (int i = 1; i < maxSize; i++)
                    {
                        data[i] = new BitArray(i, false);
                    }
                }
                else
                {
                    for (int i = 1; i < maxSize; i++)
                    {
                        data[i].SetAll(false);
                    }
                }

                degrees.Clear();
                for (int i = 0; i < size; ++i)
                {
                    degrees.Add(0);
                }
                numberOfEdges = 0;
            }
        }

        public override Int32 EdgesCount
        {
            get
            {
                return numberOfEdges;
            }
        }

        public override void SetMatrix(BitArray[] matrix)
        {
            Size = matrix.Length;
            for (int i = 0; i < matrix.Length; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (matrix[i][j])
                    {
                        AddConnection(i, j);
                    }
                }
            }
        }

        public override Object Clone()
        {
            NonHierarchicContainer clone = (NonHierarchicContainer)this.MemberwiseClone();
            clone.data = (BitArray[])this.data.Clone();
            clone.degrees = new List<int>(this.degrees);
            if(evolutionInformation != null)
            {
                clone.evolutionInformation = (EdgesInformation)this.evolutionInformation.Clone();
            }
            if (activeNodes != null)
            {
                clone.activeNodes = (BitArray)this.activeNodes.Clone();
            }
            return clone;
        }

        public override bool AreConnected(int firstVertex, int secondVertex)
        {
            Debug.Assert(firstVertex >= 0 && firstVertex < size, "First vertex is out of range!");
            Debug.Assert(secondVertex >= 0 && secondVertex < size, "Second vertex is out of range!");
            if (firstVertex == secondVertex)
            {
                return false;
            }
            if (firstVertex < secondVertex)
            {
                int temp = firstVertex;
                firstVertex = secondVertex;
                secondVertex = temp;
            }

            return data[firstVertex][secondVertex] == true;
        }

        private const int MAX_SIZE = 1024;
        private int size;
        private int maxSize;
        private int numberOfEdges;
        private BitArray[] data;
        private List<int> degrees;
        private SortedDictionary<int, List<int>> neighbourship = new SortedDictionary<int, List<int>>();

        public override List<int> Degrees
        {
            get { return degrees; }
        }
        
        public NonHierarchicContainer()
        {
            size = 0;
            maxSize = MAX_SIZE;
            data = new BitArray[maxSize];
            for (int i = 1; i < maxSize; i++)
            {
                data[i] = new BitArray(i, false);
            }
            degrees = new List<int>();
            numberOfEdges = 0;
        }

        public int AddVertex()
        {
            if (maxSize == 0)
            {
                maxSize = 1;
                data = new BitArray[maxSize];
            }
            if (size < maxSize)
            {
                data[size] = new BitArray(size, false);
                ++size;
            }
            else
            {
                maxSize *= 2;
                BitArray[] BiggerData = new BitArray[maxSize];
                for (int i = 1; i < maxSize / 2; i++)
                {
                    BiggerData[i] = new BitArray(i, false);
                    for (int j = 0; j < i; ++j)
                    {
                        BiggerData[i][j] = data[i][j];
                    }
                }
                BiggerData[size] = new BitArray(size, false);
                data = BiggerData;
                ++size;
            }
            degrees.Add(0);
            return size - 1;
        }

        public void AddConnection(int firstVertex, int secondVertex)
        {
            Debug.Assert(firstVertex >= 0 && firstVertex < size, "First vertex is out of range!");
            Debug.Assert(secondVertex >= 0 && secondVertex < size, "Second vertex is out of range!");
            Debug.Assert(firstVertex != secondVertex, "Vertices are equal!");

            if (firstVertex < secondVertex)
            {
                int temp = firstVertex;
                firstVertex = secondVertex;
                secondVertex = temp;
            }
            if (!data[firstVertex][secondVertex])
            {
                data[firstVertex][secondVertex] = true;
                numberOfEdges++;
                ++degrees[firstVertex];
                ++degrees[secondVertex];
            }

            if (evolutionInformation != null)
            {
                evolutionInformation.Add(firstVertex, secondVertex);                
            }
        }

        public void RemoveConnection(int firstVertex, int secondVertex)
        {
            Debug.Assert(firstVertex >= 0 && firstVertex < size, "First vertex is out of range!");
            Debug.Assert(secondVertex >= 0 && secondVertex < size, "Second vertex is out of range!");
            Debug.Assert(firstVertex != secondVertex, "Vertices are equal!");

            if (firstVertex < secondVertex)
            {
                int temp = firstVertex;
                firstVertex = secondVertex;
                secondVertex = temp;
            }
            if (data[firstVertex][secondVertex])
            {
                data[firstVertex][secondVertex] = false;
                numberOfEdges--;
                --degrees[firstVertex];
                --degrees[secondVertex];
            }

            if (evolutionInformation != null)
            {
                evolutionInformation.Remove(firstVertex, secondVertex);                
            }
        }
        
        public override List<int> GetAdjacentVertices(int vertex)
        {
            Debug.Assert(vertex < size && vertex >= 0, "Vertex is out of range!");
            if (!neighbourship.ContainsKey(vertex))
            {
                List<int> list = new List<int>();
                for (int i = 0; i < vertex; i++)
                {
                    if (data[vertex][i] == true)
                    {
                        list.Add(i);
                    }
                }

                for (int i = vertex + 1; i < size; i++)
                {
                    if (data[i][vertex] == true)
                    {
                        list.Add(i);
                    }
                }
                neighbourship.Add(vertex, list);
            }
            Debug.Assert(neighbourship.ContainsKey(vertex));
            return neighbourship[vertex];
        }

        public override int GetVertexDegree(int vertex)
        {
            Debug.Assert(vertex < size && vertex >= 0, "Vertex is out of range!");
            return (int)Degrees[vertex];
        }

        #region Extra Interface for Evolution Process

        internal Double PermanentRandomization()
        {
            Debug.Assert(evolutionInformation != null, "Evolution Information is not initialized.");
            RNGCrypto rand = new RNGCrypto();
            int e1 = rand.Next(0, EdgesCount);
            int e2 = rand.Next(0, EdgesCount);
            Debug.Assert(e1 < evolutionInformation.existingEdges.Count && e2 < evolutionInformation.existingEdges.Count);

            while (e1 == e2 ||
                evolutionInformation.existingEdges[e1].Key == evolutionInformation.existingEdges[e2].Key ||
                evolutionInformation.existingEdges[e1].Value == evolutionInformation.existingEdges[e2].Value ||
                evolutionInformation.existingEdges[e1].Key == evolutionInformation.existingEdges[e2].Value ||
                evolutionInformation.existingEdges[e1].Value == evolutionInformation.existingEdges[e2].Key ||
                AreConnected(evolutionInformation.existingEdges[e1].Key, evolutionInformation.existingEdges[e2].Key) ||
                AreConnected(evolutionInformation.existingEdges[e1].Value, evolutionInformation.existingEdges[e2].Value))
            {
                e1 = rand.Next(0, EdgesCount);
                e2 = rand.Next(0, EdgesCount);
                Debug.Assert(e1 < evolutionInformation.existingEdges.Count && e2 < evolutionInformation.existingEdges.Count);
            }

            int vertex1 = evolutionInformation.existingEdges[e1].Key, vertex2 = evolutionInformation.existingEdges[e1].Value;
            int vertex3 = evolutionInformation.existingEdges[e2].Key, vertex4 = evolutionInformation.existingEdges[e2].Value;
            RemoveConnection(vertex1, vertex2);
            RemoveConnection(vertex3, vertex4);
            // Calculate removed cycles count
            int removedCyclesCount = Cycles3ByVertices(vertex1, vertex2) +
                Cycles3ByVertices(vertex3, vertex4);

            AddConnection(vertex1, vertex3);
            AddConnection(vertex2, vertex4);
            // Calculate removed cycles count
            int addedCyclesCount = Cycles3ByVertices(vertex1, vertex3) +
                Cycles3ByVertices(vertex2, vertex4);

            return addedCyclesCount - removedCyclesCount;
        }

        internal Double NonPermanentRandomization()
        {
            Debug.Assert(evolutionInformation != null, "Evolution Information is not initialized.");
            RNGCrypto rand = new RNGCrypto();
            int edgeToRemove = rand.Next(0, EdgesCount);
            Debug.Assert(edgeToRemove < evolutionInformation.existingEdges.Count);
            int rvertex1 = evolutionInformation.existingEdges[edgeToRemove].Key;
            int rvertex2 = evolutionInformation.existingEdges[edgeToRemove].Value;

            int edgeToAdd = rand.Next(0, evolutionInformation.nonExistingEdges.Count);
            int avertex1 = evolutionInformation.nonExistingEdges[edgeToAdd].Key;
            int avertex2 = evolutionInformation.nonExistingEdges[edgeToAdd].Value;

            RemoveConnection(rvertex1, rvertex2);
            // Calculate removed cycles count
            int removedCyclesCount = Cycles3ByVertices(rvertex1, rvertex2);

            AddConnection(avertex1, avertex2);
            // Calculate removed cycles count
            int addedCyclesCount = Cycles3ByVertices(avertex1, avertex2);

            return addedCyclesCount - removedCyclesCount;
        }

        private int Cycles3ByVertices(int vertex1, int vertex2)
        {
            List<int> n = GetAdjacentVertices(vertex2);

            int count = 0;
            for (int t = 0; t < n.Count; t++)
            {
                if (n[t] != vertex1)
                {
                    if (GetAdjacentVertices(n[t]).Contains(vertex1))
                        ++count;
                }
            }

            return count;
        }

        #endregion
    }
}
