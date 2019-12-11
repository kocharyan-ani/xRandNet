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
    class ConnectedNonRegularHierarchicGenerator : AbstractNetworkGenerator
    {
        private ConnectedNonRegularHierarchicContainer container = new ConnectedNonRegularHierarchicContainer();

        public override INetworkContainer Container
        {
            get { return container; }
            set { container = (ConnectedNonRegularHierarchicContainer)value; }
        }

        public override void RandomGeneration(Dictionary<GenerationParameter, Object> genParam)
        {
            Debug.Assert(genParam.ContainsKey(GenerationParameter.BranchingIndex));
            Debug.Assert(genParam.ContainsKey(GenerationParameter.Vertices));
            Debug.Assert(genParam.ContainsKey(GenerationParameter.Mu));

            Int32 branchingIndex = Convert.ToInt32(genParam[GenerationParameter.BranchingIndex]);
            Int32 vertices = Convert.ToInt32(genParam[GenerationParameter.Vertices]);
            Double mu = Double.Parse(genParam[GenerationParameter.Mu].ToString(), CultureInfo.InvariantCulture);

            if (mu < 0)
                throw new InvalidGenerationParameters();

            container.Size = vertices;
            List<List<Tuple<int,int>>> branching = generateBranching(vertices, branchingIndex);
            if (branching.Count > 0)
            {
                int previous = -1;
                for(int i=0; i < branching[0].Count; i++)
                {
                    int connectionCount = 0;
                    for (int firstNode = previous +1; firstNode <= branching[0][i].Item2; firstNode++)
                    {
                        for (int secondNode = firstNode + 1; secondNode <= branching[0][i].Item2; secondNode++)
                        {
                            if (rand.NextDouble() <= (1 / Math.Pow(branchingIndex, 1 * mu)))
                            {
                                container.AddConnection(firstNode, secondNode);
                                connectionCount++;
                            }

                        }
                    }
                    if (connectionCount == 0 && previous +1 < branching[0][i].Item2)
                    {
                        int firstNode = Convert.ToInt32(Math.Round(rand.NextDouble(previous +1, branching[0][i].Item2)));
                        int secondNode = firstNode;
                        while(firstNode == secondNode)
                            secondNode = Convert.ToInt32(Math.Round(rand.NextDouble(previous + 1, branching[0][i].Item2)));
                        container.AddConnection(firstNode, secondNode);
                    }
                    previous = branching[0][i].Item2;
                }
                for(int i=1; i< branching.Count; i++)
                {
                    int previousSubtree = -1;
                    for (int j = 0; j< branching[i].Count;j++)
                    {
                        for(int firstSubtree = previousSubtree +1; firstSubtree < branching[i][j].Item1; firstSubtree++)
                        {
                            for(int secondSubtree = firstSubtree +1; secondSubtree <= branching[i][j].Item1; secondSubtree++)
                            {
                                if (rand.NextDouble() <= (1 / Math.Pow(branchingIndex, (i + 1) * mu)))
                                {
                                    int connectionCount = 0;
                                    int firstSubtreePrevious = firstSubtree == 0 ? -1 : branching[i - 1][firstSubtree - 1].Item2;
                                    int secondSubtreePrevious = secondSubtree == 0 ? -1 : branching[i - 1][secondSubtree - 1].Item2;
                                    for(int firstNode = firstSubtreePrevious +1; firstNode <= branching[i-1][firstSubtree].Item2;firstNode++)
                                    {
                                        for(int secondNode = secondSubtreePrevious + 1; secondNode <= branching[i-1][secondSubtree].Item2;secondNode++)
                                        {
                                            if (rand.NextDouble() <= (1 / Math.Pow(branchingIndex, (i + 1) * mu)))
                                            {
                                                container.AddConnection(firstNode, secondNode);
                                                connectionCount++;
                                            }
                                        }
                                    }
                                    if(connectionCount == 0)
                                    {
                                        int firstNode = Convert.ToInt32(Math.Round(rand.NextDouble(
                                            firstSubtreePrevious + 1, branching[i - 1][firstSubtree].Item2)));
                                        int secondNode = Convert.ToInt32(Math.Round(rand.NextDouble(
                                            secondSubtreePrevious + 1, branching[i - 1][secondSubtree].Item2)));
                                        container.AddConnection(firstNode, secondNode);
                                    }
                                }

                            }
                        }
                        previousSubtree = branching[i][j].Item1;
                    }
                }
            }
           
        }

        private List<List<Tuple<int, int>>> generateBranching(int vertices, int branchingIndex)
        {
            List<List<Tuple<int,int>>> branching = new List<List<Tuple<int,int>>>();
            List<List<int>> containerBranching = new List<List<int>>();
            Int32 subtreeCount = vertices;
            List<Tuple<int,int>> firstLevel = new List<Tuple<int, int>>();
            containerBranching.Add(new List<int>());
            int previousNode = -1;
            while (subtreeCount != 0)
            {
                Int32 branchSize = Convert.ToInt32(Math.Floor(rand.NextDouble(2, Math.Min(subtreeCount, branchingIndex))));
                subtreeCount -= branchSize;
                previousNode += branchSize;
                Tuple<int, int> newNode = new Tuple<int, int>(previousNode, previousNode);
                firstLevel.Add(newNode);
                containerBranching[0].Add(branchSize);
            }
            branching.Add(firstLevel);

            
            subtreeCount = branching[0].Count;
            while (subtreeCount > 1)
            {
                List<Tuple<int,int>> newLevel = new List<Tuple<int,int>>();
                List<int> containerNewLevel = new List<int>();
                Tuple<int, int> previous = new Tuple<int, int>(-1, -1);
                while (subtreeCount > 1)
                {
                    Int32 branchSize = Convert.ToInt32(Math.Round(rand.NextDouble(2, Math.Min(subtreeCount, branchingIndex)))); 
                    subtreeCount -= branchSize;
                    Tuple<int, int> newNode = new Tuple<int, int>(previous.Item1 + branchSize, branching[branching.Count - 1][previous.Item1 + branchSize].Item2);
                    newLevel.Add(newNode);
                    containerNewLevel.Add(branchSize);
                    previous = newNode;
                }
                if (subtreeCount == 1)
                {
                    newLevel.Add(new Tuple<int, int>(previous.Item1 + 1, branching[branching.Count - 1][previous.Item1 + 1].Item2));
                    containerNewLevel.Add(1);
                }
                branching.Add(newLevel);
                containerBranching.Add(containerNewLevel);
                subtreeCount = newLevel.Count;
            }
            containerBranching.Reverse();
            container.Branching = containerBranching;
            return branching;
        }
        private RNGCrypto rand = new RNGCrypto();
    }
}
