using System;
using System.Diagnostics;
using System.Globalization;

using Core;
using Core.Attributes;
using Core.Enumerations;
using Core.Exceptions;
using Core.Utility;

namespace Research
{
    /// <summary>
    /// Activation research implementation.
    /// </summary>
    [AvailableModelType(ModelType.ER)]
    [AvailableModelType(ModelType.BA)]
    [AvailableModelType(ModelType.WS)]
    [AvailableModelType(ModelType.HMN)]
    [RequiredResearchParameter(ResearchParameter.InitialActivationProbability)]
    [RequiredResearchParameter(ResearchParameter.DeactivationSpeed)]
    [RequiredResearchParameter(ResearchParameter.ActivationSpeed)]
    [RequiredResearchParameter(ResearchParameter.ActivationStepCount)]
    [RequiredResearchParameter(ResearchParameter.TracingStepIncrement)]
    [AvailableGenerationType(GenerationType.Random)]
    [AvailableGenerationType(GenerationType.Static)]
    [AvailableAnalyzeOption(
        // Trajectories //
          AnalyzeOption.Algorithm_1_By_All_Nodes
        | AnalyzeOption.Algorithm_2_By_Active_Nodes_List
        | AnalyzeOption.Algorithm_Final
        )]
    public class ActivationResearch : AbstractResearch
    {
        public override ResearchType GetResearchType() => ResearchType.Activation;

        public override int GetProcessStepsCount()
        {
            Debug.Assert(ResearchParameterValues.ContainsKey(ResearchParameter.ActivationStepCount));
            Debug.Assert(ResearchParameterValues.ContainsKey(ResearchParameter.TracingStepIncrement));
            if (processStepCount == -1)
            {
                int ev = Convert.ToInt32(ResearchParameterValues[ResearchParameter.ActivationStepCount]);
                int tev = Convert.ToInt32(ResearchParameterValues[ResearchParameter.TracingStepIncrement]);
                int cc = tev != 0 ? ev / tev : 0;
                int t = (TracingPath != "") ? 2 : 1;
                processStepCount = realizationCount * (t + (ev + 1) * GetAnalyzeOptionsCount() + cc) + 1;
            }
            return processStepCount;
        }

        protected override void ValidateResearchParameters()
        {
            if (!ResearchParameterValues.ContainsKey(ResearchParameter.ActivationStepCount) ||
                !ResearchParameterValues.ContainsKey(ResearchParameter.ActivationSpeed) || 
                !ResearchParameterValues.ContainsKey(ResearchParameter.DeactivationSpeed) ||
                !ResearchParameterValues.ContainsKey(ResearchParameter.TracingStepIncrement) ||
                !ResearchParameterValues.ContainsKey(ResearchParameter.InitialActivationProbability))
            {
                CustomLogger.Write("Research - " + ResearchName + ". Invalid research parameters.");
                throw new InvalidResearchParameters();
            }

            Int32 Time = Convert.ToInt32(ResearchParameterValues[ResearchParameter.ActivationStepCount]);
            Double Lambda = Double.Parse(ResearchParameterValues[ResearchParameter.ActivationSpeed].ToString(), CultureInfo.InvariantCulture);
            Double Mu = Double.Parse(ResearchParameterValues[ResearchParameter.DeactivationSpeed].ToString(), CultureInfo.InvariantCulture);
            Double IP = Double.Parse(ResearchParameterValues[ResearchParameter.InitialActivationProbability].ToString(), CultureInfo.InvariantCulture);

            if (Time <= 0 || Lambda < 0 || Mu < 0|| IP < 0 || IP > 1)
            {
                CustomLogger.Write("Research - " + ResearchName + ". Invalid research parameters.");
                throw new InvalidResearchParameters();
            }

            base.ValidateResearchParameters();
        }

        protected override void FillParameters(AbstractEnsembleManager m)
        {
            m.ResearchParamaterValues = ResearchParameterValues;
            m.GenerationParameterValues = GenerationParameterValues;
        }
    }
}
