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

        private Int32 stepCount;
        private Double omega;
        private Boolean permanentDistribution;
        private Int32 tracingStepIncrement;
        private Double currentCycle3Count;
        
        public SortedDictionary<Double, Double> Cycles3Trajectory { get; private set; }

        public NonHierarchicAnalyzerEvolutionEngine(AbstractNetwork n, NonHierarchicContainer c, Int32 s, Double o, Boolean p, Int32 t, Double cycles3)
        {
            network = n;
            container = c;
            stepCount = s;
            omega = o;
            permanentDistribution = p;
            tracingStepIncrement = t;
            currentCycle3Count = cycles3;
            Cycles3Trajectory = new SortedDictionary<Double, Double>();
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

                double deltaCount = permanentDistribution ?
                                    containerToChange.PermanentRandomization() :
                                    containerToChange.NonPermanentRandomization();
                double newCycle3Count = currentCycle3Count + deltaCount;

                int delta = (int)(newCycle3Count - currentCycle3Count);
                if (delta > 0) // accept case
                {   
                    Cycles3Trajectory.Add(currentStep, newCycle3Count);
                    currentCycle3Count = newCycle3Count;
                }
                else
                {
                    double probability = Math.Exp((-omega * Math.Abs(delta)));
                    if (rand.NextDouble() < probability) // accept case
                    {   
                        Cycles3Trajectory.Add(currentStep, newCycle3Count);
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
