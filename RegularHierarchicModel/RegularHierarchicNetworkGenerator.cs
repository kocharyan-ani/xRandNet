using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Diagnostics;

using Core;
using Core.Enumerations;
using Core.Model;
using Core.Exceptions;
using RandomNumberGeneration;

namespace RegularHierarchicModel
{
    /// <summary>
    /// Implementation of regularly branching block-hierarchic network's generator.
    /// </summary>
    class RegularHierarchicNetworkGenerator : AbstractNetworkGenerator
    {
        /// <summary>
        /// Container with network of specified model (regular block-hierarchic).
        /// </summary>
        private RegularHierarchicNetworkContainer container = new RegularHierarchicNetworkContainer();

        public override List<List<EdgesAddedOrRemoved>> GenerationSteps { get; protected set; }

        public override INetworkContainer Container
        {
            get { return container; }
            set { container = (RegularHierarchicNetworkContainer)value; }
        }

        public override void RandomGeneration(Dictionary<GenerationParameter, Object> genParam, bool visualMode)
        {
            Debug.Assert(genParam.ContainsKey(GenerationParameter.BranchingIndex));
            Debug.Assert(genParam.ContainsKey(GenerationParameter.Level));
            Debug.Assert(genParam.ContainsKey(GenerationParameter.Mu));

            Int32 branchingIndex = Convert.ToInt32(genParam[GenerationParameter.BranchingIndex]);
            Int32 level = Convert.ToInt32(genParam[GenerationParameter.Level]);
            Double mu = Double.Parse(genParam[GenerationParameter.Mu].ToString(), CultureInfo.InvariantCulture);

            if (mu < 0)
                throw new InvalidGenerationParameters();

            container.BranchingIndex = branchingIndex;
            container.Level = level;
            // TODO add logic
            if (visualMode)
                GenerationSteps = new List<List<EdgesAddedOrRemoved>>();
            container.HierarchicTree = GenerateTree(branchingIndex, level, mu);
        }

        private RNGCrypto rand = new RNGCrypto();
        private const int ARRAY_MAX_SIZE = 2000000000;        

        /// <summary>
        /// Recursively creates a block-hierarchic tree.
        /// </summary>
        /// <note>Data is initializing and generating started from root.</note>
        private BitArray[][] GenerateTree(Int32 branchingIndex, 
            Int32 level, 
            Double mu)
        {
            BitArray[][] treeMatrix = new BitArray[level][];

            int nodeDataLength = (branchingIndex - 1) * branchingIndex / 2;
            long levelDataLength;
            int arrayCountForLevel;
            for (Int32 i = level; i > 0; --i)
            {                
                levelDataLength = Convert.ToInt64(Math.Pow(branchingIndex, level - i) * nodeDataLength);
                arrayCountForLevel = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(levelDataLength) / ARRAY_MAX_SIZE));

                treeMatrix[level - i] = new BitArray[arrayCountForLevel];
                int j;
                for (j = 0; j < arrayCountForLevel - 1; j++)
                {
                    treeMatrix[level - i][j] = new BitArray(ARRAY_MAX_SIZE);
                }
                treeMatrix[level - i][j] = new BitArray(Convert.ToInt32(levelDataLength -
                    (arrayCountForLevel - 1) * ARRAY_MAX_SIZE));

                GenerateData(treeMatrix, i, branchingIndex, level, mu);
            }

            return treeMatrix;
        }
        
        /// <summary>
        /// Generates random data for current level of block-hierarchic tree.
        /// </summary>
        /// <node>Current level must be initialized.</node>
        private void GenerateData(BitArray[][] treeMatrix, 
            Int32 currentLevel, 
            Int32 branchingIndex, 
            Int32 maxLevel, 
            Double mu)
        {
            for (int i = 0; i < treeMatrix[maxLevel - currentLevel].Length; i++)
            {
                for (int j = 0; j < treeMatrix[maxLevel - currentLevel][i].Length; j++)
                {
                    if (rand.NextDouble() <= (1 / Math.Pow(branchingIndex, currentLevel * mu)))
                    {
                        treeMatrix[maxLevel - currentLevel][i][j] = true;

                    }
                    else
                    {
                        treeMatrix[maxLevel - currentLevel][i][j] = false;
                    }
                }
            }
        }
    }
}
