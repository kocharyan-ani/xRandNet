using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

using Core.Enumerations;
using RandomNumberGeneration;

namespace Core.Model.Engines
{
    enum AlgorithmType { Algorithm1, Algorithm2, Final }

    class AnalyzerActivationEngine
    {
        private readonly AbstractNetwork network;
        private readonly AbstractNetworkContainer container;

        private Int32 stepCount;
        private Double mu;
        private Double lambda;
        private Int32 tracingStepIncrement;
        private RNGCrypto Rand = new RNGCrypto();
        private delegate void Process(AbstractNetworkContainer containerToChange);
        private Process processToRun;

        public SortedDictionary<Double, Double> Trajectory { get; private set; }

        public AnalyzerActivationEngine(AbstractNetwork n, AbstractNetworkContainer c, Int32 s, Double m, Double l, Int32 t)
        {
            network = n;
            container = c;
            stepCount = s;
            mu = m;
            lambda = l;
            tracingStepIncrement = t;
            Trajectory = new SortedDictionary<Double, Double>();
        }

        public void Calculate(AlgorithmType t)
        {
            InitializeProcessToRun(t);
            Debug.Assert(processToRun != null);
            AbstractNetworkContainer containerToChange = (AbstractNetworkContainer)container.Clone();

            Double timeStep = 0;
            Double currentTracingStep = tracingStepIncrement, currentActiveNodesCount = containerToChange.GetActiveNodesCount();
            Trajectory.Add(timeStep, currentActiveNodesCount / containerToChange.Size);

            while (timeStep <= stepCount && currentActiveNodesCount != 0)
            {
                processToRun(containerToChange);
                currentActiveNodesCount = containerToChange.GetActiveNodesCount();
                Trajectory.Add(++timeStep, currentActiveNodesCount / containerToChange.Size);

                network.UpdateStatus(NetworkStatus.StepCompleted);

                // TODO closed Trace
                /*if (TraceCurrentStep(timeStep, currentTracingStep, "ActivationAlgorithm_1"))
                {
                    currentTracingStep += TracingStepIncrement;
                    network.UpdateStatus(NetworkStatus.StepCompleted);
                }*/
            }
            Debug.Assert(timeStep > stepCount || !containerToChange.DoesActiveNodeExist());
        }

        private void InitializeProcessToRun(AlgorithmType t)
        {
            switch (t)
            {
                case AlgorithmType.Algorithm1:
                    processToRun = Algorithm1;
                    break;
                case AlgorithmType.Algorithm2:
                    processToRun = Algorithm2;
                    break;
                case AlgorithmType.Final:
                    processToRun = AlgorithmFinal;
                    break;
            }
        }

        private void Algorithm1(AbstractNetworkContainer containerToChange)
        {            
            Int32 RandomActiveNode = Rand.Next(0, containerToChange.Size);
            Debug.Assert(RandomActiveNode < containerToChange.Size);

            if (containerToChange.GetActiveStatus(RandomActiveNode))
            {
                if (Rand.NextDouble() < lambda)
                {
                    ActivateVertex(containerToChange, true, RandomActiveNode);
                }
                if (Rand.NextDouble() < mu)
                {
                    containerToChange.SetActiveStatus(RandomActiveNode, false);
                }
            }
        }

        private void Algorithm2(AbstractNetworkContainer containerToChange)
        {            
            Int32 RandomActiveNode = GetRandomActiveNodeIndex(containerToChange);
            Debug.Assert(RandomActiveNode < containerToChange.Size);

            Debug.Assert(containerToChange.GetActiveStatus(RandomActiveNode));
            if (Rand.NextDouble() < lambda)
            {
                ActivateVertex(containerToChange, true, RandomActiveNode);
            }
            if (Rand.NextDouble() < mu)
            {
                containerToChange.SetActiveStatus(RandomActiveNode, false);
            }
        }

        private void AlgorithmFinal(AbstractNetworkContainer containerToChange)
        {            
            Int32[] final = new Int32[containerToChange.Size];
            for (int i = 0; i < final.Length; ++i)
            {
                final[i] = containerToChange.GetActiveStatus(i) ? 1 : 0;
            }

            for (int i = 0; i < containerToChange.GetActiveStatuses().Count; ++i)
            {
                if (!containerToChange.GetActiveStatus(i))
                {
                    continue;
                }
                if (Rand.NextDouble() < lambda)
                {
                    Int32 index = RandomNeighbourToActivate(containerToChange, true, i);
                    if (index != -1)
                    {
                        ++final[index];
                    }
                }
                if (Rand.NextDouble() < mu)
                {
                    --final[i];
                }
            }

            for (int i = 0; i < final.Count(); ++i)
            {
                // TODO get k as parameter
                containerToChange.SetActiveStatus(i, final[i] > 0);
            }
        }

        private void ActivateVertex(AbstractNetworkContainer containerToChange, bool allPas, int n)
        {
            Int32 RandomNode = RandomNeighbourToActivate(containerToChange, allPas, n);
            if (RandomNode == -1)
            {
                return;
            }
            containerToChange.SetActiveStatus(RandomNode, true);
        }

        private Int32 GetRandomActiveNodeIndex(AbstractNetworkContainer containerToChange)
        {
            List<Int32> activeIndexes = new List<Int32>();

            for (int i = 0; i < container.Size; ++i)
            {
                if (containerToChange.GetActiveStatus(i))
                {
                    activeIndexes.Add(i);
                }
            }

            if (activeIndexes.Count == 0)
            {
                return -1;
            }

            return GetRandomIndex(activeIndexes);
        }

        private List<Int32> GetVertexPassiveNeighbours(AbstractNetworkContainer containerToChange, Int32 Vertex)
        {
            List<Int32> PassiveNeighbours = new List<Int32>();
            for (int i = 0; i < container.Size; ++i)
            {
                if (i == Vertex)
                {
                    continue;
                }
                if (containerToChange.AreConnected(Vertex, i) && !containerToChange.GetActiveStatus(i))
                {
                    PassiveNeighbours.Add(i);
                }
            }

            return PassiveNeighbours;
        }

        private Int32 GetRandomIndex(List<Int32> list) => list.OrderBy(x => Rand.Next()).FirstOrDefault();

        private Int32 RandomNeighbourToActivate(AbstractNetworkContainer containerToChange, bool allPas, int n)
        {
            if (allPas ? containerToChange.Degrees[n] == 0 : GetVertexPassiveNeighbours(containerToChange, n).Count == 0)
            {
                return -1;
            }
            return allPas ? GetRandomIndex(containerToChange.GetAdjacentVertices(n))
                          : GetRandomIndex(GetVertexPassiveNeighbours(containerToChange, n));
        }
    }
}
