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

            Int32 branchingIndex = Convert.ToInt32(genParam[GenerationParameter.BranchingIndex]);
            Int32 level = Convert.ToInt32(genParam[GenerationParameter.Level]);
            Double mu = Double.Parse(genParam[GenerationParameter.Mu].ToString(), CultureInfo.InvariantCulture);

            if (mu < 0)
                throw new InvalidGenerationParameters();

            container.Size = (Int32)Math.Pow(branchingIndex, level);

            int nodeDataLength = (branchingIndex - 1) * branchingIndex / 2;
            long levelDataLength;
            for (Int32 currentLevel = level; currentLevel > 0; --currentLevel)
            {
                levelDataLength = Convert.ToInt64(Math.Pow(branchingIndex, level - currentLevel) * nodeDataLength);

                Double nodesInSubtree = Math.Pow(branchingIndex, currentLevel - 1);
                for (int subtreeConnection = 0; subtreeConnection < levelDataLength; subtreeConnection++)
                {
                    if (rand.NextDouble() <= (1 / Math.Pow(branchingIndex, currentLevel * mu)))
                    {
                        int connectionsCount = 0;
                        Int32 x = subtreeConnection / nodeDataLength;
                        Int32 d = subtreeConnection % nodeDataLength;
                        Double D = Math.Pow(1 - 2 * nodeDataLength, 2) - 8 * (d + 1);
                        Double firstSubtreeIndex = 0;
                        if (D >= 0)
                        {
                            firstSubtreeIndex = nodeDataLength - 0.5 - Math.Sqrt(D) / 2;
                            if (firstSubtreeIndex == Math.Floor(firstSubtreeIndex))
                                --firstSubtreeIndex;
                            else
                                firstSubtreeIndex = Math.Floor(firstSubtreeIndex);
                        }
                        Double secondSubtreeIndex = firstSubtreeIndex + d + 1 - firstSubtreeIndex * nodeDataLength
                            + (firstSubtreeIndex + 1)*firstSubtreeIndex/2 + x * branchingIndex;
                        firstSubtreeIndex += x * branchingIndex;
                        for (int firstNode = 0; firstNode < nodesInSubtree; firstNode++)
                        {
                            for (int secondNode = 0; secondNode < nodesInSubtree; secondNode++)
                            {

                                if (rand.NextDouble() <= (1 / Math.Pow(branchingIndex, currentLevel * mu)))
                                {
                                    Int32 firstNodeIndex = Convert.ToInt32(firstSubtreeIndex * nodesInSubtree) + firstNode;
                                    Int32 secondNodeIndex = Convert.ToInt32(secondSubtreeIndex * nodesInSubtree) + secondNode;
                                    container.AddConnection(firstNodeIndex, secondNodeIndex);
                                    connectionsCount++;
                                }
                            }
                        }
                        if(connectionsCount == 0)
                        {
                            Int32 firstNode = Convert.ToInt32(rand.NextDouble(0, nodesInSubtree - 1));
                            Int32 secondNode = Convert.ToInt32(rand.NextDouble(0, nodesInSubtree - 1));
                            Int32 firstNodeIndex = Convert.ToInt32(firstSubtreeIndex * nodesInSubtree) + firstNode;
                            Int32 secondNodeIndex = Convert.ToInt32(secondSubtreeIndex * nodesInSubtree) + secondNode;
                            container.AddConnection(firstNodeIndex, secondNodeIndex);
                        }

                    }
                }


            }
        }

        private RNGCrypto rand = new RNGCrypto();
    }
}
