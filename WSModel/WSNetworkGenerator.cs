using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Core;
using Core.Enumerations;
using Core.Model;
using Core.Utility;
using Core.Exceptions;
using NetworkModel;
using RandomNumberGeneration;

namespace WSModel
{
    /// <summary>
    /// Implementation of generator of random network of Watts-Strogatz's model.
    /// </summary>
    class WSNetworkGenerator : AbstractNetworkGenerator
    {
        private NonHierarchicContainer container;

        public override List<List<EdgesAddedOrRemoved>> GenerationSteps { get; protected set; }

        public WSNetworkGenerator()
        {
            container = new NonHierarchicContainer();
        }

        public override INetworkContainer Container
        {
            get { return container; }
            set { container = (NonHierarchicContainer)value; }
        }

        public override void RandomGeneration(Dictionary<GenerationParameter, Object> genParam, bool visualMode)
        {
            Int32 numberOfVertices = Convert.ToInt32(genParam[GenerationParameter.Vertices]);
            Int32 numberOfEdges = Convert.ToInt32(genParam[GenerationParameter.Edges]);
            Double probability = Convert.ToDouble(genParam[GenerationParameter.Probability]);
            Int32 stepCount = Convert.ToInt32(genParam[GenerationParameter.StepCount]);

            // TODO add validation

            //container.SetParameters(numberOfVertices, numberOfEdges / 2);
            container.Size = numberOfVertices;
            Randomize();
            if (visualMode)
                GenerationSteps = new List<List<EdgesAddedOrRemoved>>();
            GenerateInitialNetwork(numberOfVertices, numberOfEdges);
            FillValuesByProbability(probability, stepCount);
        }

        private int currentId = 0;
        private List<int> collectRandoms = new List<int>();

        private void Randomize()
        {
            Random rand = new Random();
            collectRandoms.Clear();

            for (int i = 0; i < container.Size; ++i)
            {
                double rand_number = rand.Next(0, container.Size);
                collectRandoms.Add((int)rand_number);
            }
        }

        private void GenerateInitialNetwork(int vertexCount, int numberOfEdges)
        {
            List<EdgesAddedOrRemoved> initialStep = (GenerationSteps != null) ? new List<EdgesAddedOrRemoved>() : null;
            for (int i = 0; i < vertexCount; ++i)
            {
                for (int k = 1; k <= numberOfEdges / 2; ++k)
                {
                    int j = (i + k) < vertexCount ? (i + k) : (i + k) % vertexCount;
                    container.AddConnection(i, j);
                    initialStep.Add(new EdgesAddedOrRemoved(i, j, true));
                }
            }
            GenerationSteps.Add(initialStep);
        }

        private void FillValuesByProbability(double probability, int stepCount)
        {
            while (stepCount > 0)
            {
                List<EdgesAddedOrRemoved> step = (GenerationSteps != null) ? new List<EdgesAddedOrRemoved>() : null;
                for (int i = 1; i < container.Size; ++i)
                {
                    List<int> neighbours = new List<int>();
                    List<int> nonNeighbours = new List<int>();
                    for (int k = 0; k < container.Size && k < i; ++k)
                    {
                        if (container.AreConnected(i, k))
                        {
                            neighbours.Add(k);
                            //if (step != null)
                            //    step.Add(new EdgesAddedOrRemoved(i, k, true));
                        }
                        else
                        {
                            nonNeighbours.Add(k);
                            //if (step != null)
                            //    step.Add(new EdgesAddedOrRemoved(i, k, false));
                        }
                    }

                    if (nonNeighbours.Count > 0)
                    {
                        int size_neighbours = neighbours.Count;
                        for (int j = 0; j < size_neighbours; ++j)
                        {
                            int r = WSStep(probability, nonNeighbours, neighbours[j]);
                            if (r != neighbours[j])
                            {
                                //container.Disconnect(i, neighbours[j]);
                                //container.AreConnected(i, r);
                                container.AddConnection(i, r);
                                container.RemoveConnection(i, neighbours[j]);
                                if (step != null)
                                {
                                    step.Add(new EdgesAddedOrRemoved(i, r, true));
                                    step.Add(new EdgesAddedOrRemoved(i, neighbours[j], false));
                                }
                            }
                        }
                    }
                }
                GenerationSteps.Add(step);
                --stepCount;
            }
        }

        public int WSStep(double probability, List<int> indexes, int index)
        {
            // select a number from indices with m_prob probability 
            // or return index with 1 - m_prob probability

            if (probability * container.Size > collectRandoms[currentId])
            {
                int cycleCount = 0;
                while (collectRandoms[currentId] > indexes.Count - 1)
                {
                    cycleCount++;
                    if (currentId == collectRandoms.Count - 1)
                        currentId = 0;
                    else
                        ++currentId;
                    if (cycleCount > container.Size)
                        return index;
                }

                int id = indexes[collectRandoms[currentId]];
                if (currentId == collectRandoms.Count - 1)
                    currentId = 0;
                else
                    ++currentId;
                return id;
            }
            if (currentId == collectRandoms.Count - 1)
                currentId = 0;
            else
                ++currentId;
            return index;
        }
    }
}
