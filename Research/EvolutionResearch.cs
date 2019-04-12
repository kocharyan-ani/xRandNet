using System;
using System.Diagnostics;

using Core;
using Core.Enumerations;
using Core.Attributes;

namespace Research
{
    /// <summary>
    /// Evolution research implementation.
    /// </summary>
    [AvailableModelType(ModelType.ER)]
    [AvailableGenerationType(GenerationType.Random)]
    [AvailableGenerationType(GenerationType.Static)]
    [RequiredResearchParameter(ResearchParameter.EvolutionStepCount)]
    [RequiredResearchParameter(ResearchParameter.Omega)]
    [RequiredResearchParameter(ResearchParameter.PermanentDistribution)]
    [RequiredResearchParameter(ResearchParameter.TracingStepIncrement)]
    [AvailableAnalyzeOption(
        // Trajectories //
        AnalyzeOption.Cycles3Evolution
        )]
    public class EvolutionResearch : AbstractResearch
    {
        public override ResearchType GetResearchType() => ResearchType.Evolution;

        public override int GetProcessStepsCount()
        {
            Debug.Assert(ResearchParameterValues.ContainsKey(ResearchParameter.EvolutionStepCount));
            Debug.Assert(ResearchParameterValues.ContainsKey(ResearchParameter.TracingStepIncrement));
            if (processStepCount == -1)
            {
                int ev = Convert.ToInt32(ResearchParameterValues[ResearchParameter.EvolutionStepCount]);
                int tev = Convert.ToInt32(ResearchParameterValues[ResearchParameter.TracingStepIncrement]);
                int cc = tev != 0 ? ev / tev : 0;
                int t = (TracingPath != "") ? 2 : 1;
                processStepCount = realizationCount * (t + (ev + 1) * GetAnalyzeOptionsCount() + cc) + 1;
            }
            return processStepCount;
        }

        protected override void FillParameters(AbstractEnsembleManager m)
        {
            m.ResearchParamaterValues = ResearchParameterValues;
            m.GenerationParameterValues = GenerationParameterValues;
        }
    }
}
