using System;

using Core;
using Core.Attributes;
using Core.Enumerations;

namespace Research
{
    /// <summary>
    /// Basic research implementation.
    /// </summary>
    [AvailableModelType(ModelType.ER)]
    [AvailableModelType(ModelType.BA)]
    [AvailableModelType(ModelType.BB)]
    [AvailableModelType(ModelType.WS)]
    [AvailableModelType(ModelType.RegularHierarchic)]
    [AvailableModelType(ModelType.NonRegularHierarchic)]
    [AvailableModelType(ModelType.ConnectedRegularHierarchic)]
    [AvailableModelType(ModelType.ConnectedNonRegularHierarchic)]
    [AvailableModelType(ModelType.HMN)]
    [AvailableGenerationType(GenerationType.Random)]
    [AvailableGenerationType(GenerationType.Static)]
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
    public class BasicResearch : AbstractResearch
    {
        public override ResearchType GetResearchType() => ResearchType.Basic;

        public override int GetProcessStepsCount()
        {
            if (processStepCount == -1)
            {
                int t = (TracingPath != "") ? 2 : 1;
                processStepCount = realizationCount * (t + GetAnalyzeOptionsCount()) + 1;
            }
            return processStepCount;
        }
        
        protected override void FillParameters(AbstractEnsembleManager m)
        {
            m.GenerationParameterValues = GenerationParameterValues;
        }        
    }
}
