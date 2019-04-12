using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Diagnostics;

using Core.Enumerations;
using Core.Model;
using Core.Utility;
using Core.Exceptions;
using NetworkModel;
using RandomNumberGeneration;

namespace HMNModel
{
    class HMNetworkGenerator : AbstractNetworkGenerator
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
            Debug.Assert(genParam.ContainsKey(GenerationParameter.ZeroLevelNodesCount));
            Debug.Assert(genParam.ContainsKey(GenerationParameter.Probability));
            Debug.Assert(genParam.ContainsKey(GenerationParameter.BlocksCount));
            Debug.Assert(genParam.ContainsKey(GenerationParameter.MakeConnected));

            Int32 numberOfVertices = Convert.ToInt32(genParam[GenerationParameter.Vertices]);
            Int32 zeroLevelNodesCount = Convert.ToInt32(genParam[GenerationParameter.ZeroLevelNodesCount]);
            Double probability = Double.Parse(genParam[GenerationParameter.Probability].ToString(), CultureInfo.InvariantCulture);
            Int32 blocksCount = Convert.ToInt32(genParam[GenerationParameter.BlocksCount]);
            Double alpha = Double.Parse(genParam[GenerationParameter.Alpha].ToString(), CultureInfo.InvariantCulture);
            Boolean makeConnected = Convert.ToBoolean(genParam[GenerationParameter.MakeConnected]);
            
            if ((probability < 0 || probability > 1) ||
                (alpha < 0 || alpha > 1) ||
                !(IsPowerOfN(numberOfVertices/zeroLevelNodesCount, blocksCount)))
            {
                throw new InvalidGenerationParameters();
            }

            container.Size = numberOfVertices;
            Generate(numberOfVertices, zeroLevelNodesCount, blocksCount, probability, alpha, makeConnected);
        }

        private RNGCrypto rand = new RNGCrypto();
        private Int32 node { get; set; }

        private Boolean IsPowerOfN(Int32 x, Int32 n)
        {
            if (x == 1)
            {
                return true;
            }

            if (x % n != 0)
            {
                return false;
            }

            return IsPowerOfN(x / n, n);
        }

        private void GenerateFullGraph(Int32[] nodes)
        {
            for (Int32 i = 0; i < nodes.Length - 1; ++i)
            {
                for (Int32 j = i + 1; j < nodes.Length; ++j)
                {
                    container.AddConnection(nodes[i], nodes[j]);
                }
            }
        }

        private void AddTwoBlocksConnection(Double probability, Int32[] firstBlock, Int32[] secondBlock)
        {
            Boolean stop = false;

            while (!stop)
            {
                for (Int32 i = 0; i < firstBlock.Length; ++i)
                    for (Int32 j = 0; j < secondBlock.Length; ++j)
                    {
                        if (probability >= rand.NextDouble())
                        {
                            container.AddConnection(firstBlock[i], secondBlock[j]);
                            stop = true;
                        }
                    }
            }
        }

        private void AddTwoBlocksPropablyConnection(Double probability, Int32[] firstBlock, Int32[] secondBlock)
        {
            for (Int32 i = 0; i < firstBlock.Length; ++i)
                for (Int32 j = 0; j < secondBlock.Length; ++j)
                {
                    if (probability >= rand.NextDouble())
                    {
                        container.AddConnection(firstBlock[i], secondBlock[j]);
                    }
                }
        }

        private List<Int32[]> GetBlocksFromNetwork(Int32 count, Int32 size)
        {
            Int32[] block = null;
            List<Int32[]> Blocks = new List<Int32[]>();
            for (Int32 j = 1; j <= count; j++)
            {
                block = new Int32[size];
                for (Int32 i = 0; i < size; ++i)
                {
                    block[i] = node++;
                }
                Blocks.Add(block);
            }

            return Blocks;
        }

        private void Generate(Int32 numberOfVertices, Int32 zeroLevelNodesCount,
            Int32 blocksCount, Double probability, Double alpha, Boolean makeConnected)
        {
            Int32 fullGraphNodesCount = zeroLevelNodesCount;
            for (Int32 i = 0; i < container.Size; i += fullGraphNodesCount)
            {
                Int32[] fullgraphNodesArray = new Int32[fullGraphNodesCount];
                Int32 k = 0;
                for (Int32 j = i; j < i + fullGraphNodesCount; ++j)
                {
                    fullgraphNodesArray[k++] = j;
                }

                GenerateFullGraph(fullgraphNodesArray);
            }

            Int32 level = 1;
            Int32 levelsCount = (Int32)(Math.Log(numberOfVertices / zeroLevelNodesCount, blocksCount));

            while (levelsCount > 0)
            {
                List<Int32[]> blocks = null;
                Double levelP = alpha * Math.Pow(probability, level);
                Int32 size = Convert.ToInt32(Math.Pow(blocksCount, level - 1) * zeroLevelNodesCount);
                node = 0;
                for (Int32 i = 0; i < container.Size; i += blocksCount * size)
                {
                    blocks = GetBlocksFromNetwork(blocksCount, size);

                    for (Int32 j = 1; j < blocks.Count; ++j)
                    {
                        if (makeConnected)
                            AddTwoBlocksConnection(levelP, blocks[0], blocks[j]);
                        else
                            AddTwoBlocksPropablyConnection(levelP, blocks[0], blocks[j]);
                    }
                }

                --levelsCount;
                ++level;
            }
        }
    }
}
