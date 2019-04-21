using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using Core;
using Core.Attributes;
using Core.Enumerations;
using Core.Events;
using Core.Exceptions;
using Core.Utility;

namespace Research
{
    /// <summary>
    /// Threshold research implementation.
    /// </summary>
    [AvailableModelType(ModelType.ER)]
    [AvailableModelType(ModelType.RegularHierarchic)]
    [AvailableGenerationType(GenerationType.Random)]
    [AvailableGenerationType(GenerationType.Static)]
    [RequiredResearchParameter(ResearchParameter.ProbabilityMax)]
    [RequiredResearchParameter(ResearchParameter.ProbabilityDelta)]
    [AvailableAnalyzeOption(
        // Globals //
          AnalyzeOption.AvgPathLength
        | AnalyzeOption.Diameter
        | AnalyzeOption.AvgDegree
        | AnalyzeOption.AvgClusteringCoefficient
        | AnalyzeOption.Cycles3
        | AnalyzeOption.Cycles4
        // Eigens //
        | AnalyzeOption.EigenDistanceDistribution
        | AnalyzeOption.EigenValues
        | AnalyzeOption.LaplacianEigenValues
        // Distributions //
        | AnalyzeOption.DegreeDistribution
        | AnalyzeOption.ClusteringCoefficientDistribution
        | AnalyzeOption.ConnectedComponentDistribution
        | AnalyzeOption.ClusteringCoefficientPerVertex
        | AnalyzeOption.DistanceDistribution
        | AnalyzeOption.TriangleByVertexDistribution
        // Centralities //
        | AnalyzeOption.DegreeCentrality
        | AnalyzeOption.ClosenessCentrality
        | AnalyzeOption.BetweennessCentrality
        )]
    public class ThresholdResearch : AbstractResearch
    {
        private bool isCanceled = false;

        private GenerationParameter probabilityParameter;
        private Double minProbability;
        private Double currentProbability;
        private Double maxProbability;
        private Double delta;

        public override void StartResearch()
        {
            ValidateResearchParameters();

            Debug.Assert(GenerationParameterValues.ContainsKey(GenerationParameter.Probability) ||
                GenerationParameterValues.ContainsKey(GenerationParameter.Mu));
            probabilityParameter = GenerationParameterValues.ContainsKey(GenerationParameter.Probability) ?
                GenerationParameter.Probability : GenerationParameter.Mu;

            minProbability = Double.Parse(GenerationParameterValues[probabilityParameter].ToString(), CultureInfo.InvariantCulture);
            currentProbability = minProbability;
            maxProbability = Double.Parse(ResearchParameterValues[ResearchParameter.ProbabilityMax].ToString(), CultureInfo.InvariantCulture);
            delta = Double.Parse(ResearchParameterValues[ResearchParameter.ProbabilityDelta].ToString(), CultureInfo.InvariantCulture);

            StatusInfo = new ResearchStatusInfo(ResearchStatus.Running, 0);
            CustomLogger.Write("Research ID - " + ResearchID.ToString() +
                ". Research - " + ResearchName + ". STARTED Threshold RESEARCH.");

            StartCurrentEnsemble();
        }

        public override void StopResearch()
        {
            isCanceled = true;
            base.StopResearch();
        }

        public override ResearchType GetResearchType() => ResearchType.Threshold;

        public override int GetProcessStepsCount()
        {
            /*if (processStepCount == -1)
            {
                // TODO
            }
            return processStepCount;*/
            return 10000000;
        }

        protected override void ValidateResearchParameters()
        {
            if (!ResearchParameterValues.ContainsKey(ResearchParameter.ProbabilityDelta) ||
                !ResearchParameterValues.ContainsKey(ResearchParameter.ProbabilityMax))
            {
                CustomLogger.Write("Research - " + ResearchName + ". Invalid research parameters.");
                throw new InvalidResearchParameters();
            }

            base.ValidateResearchParameters();
        }

        protected override void RunCompleted(IAsyncResult res)
        {
            if (isCanceled)
                ResearchParameterValues[ResearchParameter.ProbabilityMax] = currentProbability;
            else
                result.EnsembleResults.Add(currentManager.Result);

            currentProbability += delta;
            StartCurrentEnsemble();
        }

        private void StartCurrentEnsemble()
        {
            if (currentProbability <= maxProbability && !isCanceled)
            {
                base.CreateEnsembleManager();
                ManagerRunner r = new ManagerRunner(currentManager.Run);
                r.BeginInvoke(new AsyncCallback(RunCompleted), null);
            }
            else
            {
                base.SaveResearch();
            }
        }

        protected override void FillParameters(AbstractEnsembleManager m)
        {
            Dictionary<GenerationParameter, Object> g = new Dictionary<GenerationParameter, Object>();
            foreach (GenerationParameter p in base.GenerationParameterValues.Keys)
            {
                if (p == probabilityParameter)
                    g.Add(p, currentProbability);
                else
                    g.Add(p, base.GenerationParameterValues[p]);
            }

            m.GenerationParameterValues = g;
        }
    }
}
