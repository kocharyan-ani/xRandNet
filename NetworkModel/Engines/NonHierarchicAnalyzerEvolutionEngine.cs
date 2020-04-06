using System;
using System.Collections.Generic;
using System.Diagnostics;

using Core;
using Core.Enumerations;
using RandomNumberGeneration;

namespace NetworkModel.Engines
{
    /// <summary>
    /// Utility engine class, which implements evolution in non hierarchical network.
    /// <options>Evoltutional options are
    ///     Cycles3    
    /// </options>
    /// </summary>
    class NonHierarchicAnalyzerEvolutionEngine
    {
        private readonly AbstractNetwork network;
        private readonly NonHierarchicContainer container;
        private readonly bool visualMode;

        private Int32 stepCount;
        private Double omega;
        private Boolean permanentDistribution;
        private Int32 tracingStepIncrement;
        private Double currentCycle3Count;
        
        public SortedDictionary<Double, Double> Cycles3Trajectory { get; private set; }
        public List<List<EdgesAddedOrRemoved>> EvolutionInformation { get; private set; }

        public NonHierarchicAnalyzerEvolutionEngine(AbstractNetwork n, NonHierarchicContainer c, bool vm, 
            Int32 s, Double o, Boolean p, Int32 t, Double cycles3)
        {
            network = n;
            container = c;
            visualMode = vm;
            stepCount = s;
            omega = o;
            permanentDistribution = p;
            tracingStepIncrement = t;
            currentCycle3Count = cycles3;
            Cycles3Trajectory = new SortedDictionary<Double, Double>();
            if (visualMode)
                EvolutionInformation = new List<List<EdgesAddedOrRemoved>>();
        }

        public void Calculate()
        {
            NonHierarchicContainer containerToChange = (NonHierarchicContainer)container.Clone();
            containerToChange.InitializeEvolutionInformation();

            RNGCrypto rand = new RNGCrypto();
            int currentStep = 0, currentTracingStep = tracingStepIncrement;
            Cycles3Trajectory.Add(currentStep, currentCycle3Count);                
           
            while (currentStep != stepCount)
            {
                NonHierarchicContainer savedContainer = (NonHierarchicContainer)containerToChange.Clone();
                ++currentStep;

                List<EdgesAddedOrRemoved> edges = visualMode ? new List<EdgesAddedOrRemoved>() : null;
                double deltaCount = permanentDistribution ?
                                    containerToChange.PermanentRandomization(ref edges) :
                                    containerToChange.NonPermanentRandomization(ref edges);
                double newCycle3Count = currentCycle3Count + deltaCount;

                int delta = (int)(newCycle3Count - currentCycle3Count);
                if (delta > 0) // accept case
                {   
                    Cycles3Trajectory.Add(currentStep, newCycle3Count);
                    if (visualMode)
                        EvolutionInformation.Add(edges);
                    currentCycle3Count = newCycle3Count;
                }
                else
                {
                    double probability = Math.Exp((-omega * Math.Abs(delta)));
                    if (rand.NextDouble() < probability) // accept case
                    {   
                        Cycles3Trajectory.Add(currentStep, newCycle3Count);
                        if (visualMode)
                            EvolutionInformation.Add(edges);
                        currentCycle3Count = newCycle3Count;
                    }
                    else // reject case
                    {
                        Cycles3Trajectory.Add(currentStep, currentCycle3Count);
                        containerToChange = savedContainer;
                    }
                }

                network.UpdateStatus(NetworkStatus.StepCompleted);

                // TODO closed Trace
                /*if (currentTracingStep == currentStep)
                {
                    container.Trace(network.ResearchName,
                        "Realization_" + network.NetworkID.ToString(),
                        "Matrix_" + currentTracingStep.ToString());
                    currentTracingStep += tracingStepIncrement;

                    network.UpdateStatus(NetworkStatus.StepCompleted);
                }*/
            }
        }
    }
}
