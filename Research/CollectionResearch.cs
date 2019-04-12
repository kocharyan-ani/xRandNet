using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Diagnostics;
using System.Threading;

using Microsoft.Practices.EnterpriseLibrary.Logging;

using Core;
using Core.Attributes;
using Core.Enumerations;
using Core.Model;
using Core.Utility;
using Core.Settings;
using Core.Events;
using Core.Exceptions;

using Core.Result;

namespace Research
{
    /// <summary>
    /// Collection research implementation.
    /// </summary>
    [AvailableModelType(ModelType.ER)]
    [AvailableModelType(ModelType.BA)]
    [AvailableModelType(ModelType.WS)]
    [AvailableModelType(ModelType.RegularHierarchic)]
    [AvailableModelType(ModelType.NonRegularHierarchic)]
    [AvailableModelType(ModelType.HMN)]
    [AvailableGenerationType(GenerationType.Static)]
    // TODO think about
    [RequiredResearchParameter(ResearchParameter.InitialActivationProbability)]
    [RequiredResearchParameter(ResearchParameter.DeactivationSpeed)]
    [RequiredResearchParameter(ResearchParameter.ActivationSpeed)]
    [RequiredResearchParameter(ResearchParameter.ActivationStepCount)]
    [RequiredResearchParameter(ResearchParameter.TracingStepIncrement)]
    // TODO
    //[RequiredResearchParameter(ResearchParameter.ResearchItemType)]
    [RequiredResearchParameter(ResearchParameter.InputPath)]
    // TODO think about
    [AvailableAnalyzeOption(AnalyzeOption.Algorithm_1_By_All_Nodes
        | AnalyzeOption.Algorithm_2_By_Active_Nodes_List
        | AnalyzeOption.Algorithm_Final
        )]
    /*[AvailableAnalyzeOption(AnalyzeOption.AvgClusteringCoefficient
        | AnalyzeOption.AvgDegree
        | AnalyzeOption.AvgPathLength
        | AnalyzeOption.ClusteringCoefficientDistribution
        | AnalyzeOption.ClusteringCoefficientPerVertex
        | AnalyzeOption.ConnectedComponentDistribution
        | AnalyzeOption.Cycles3
        | AnalyzeOption.Cycles4
        | AnalyzeOption.DegreeDistribution
        | AnalyzeOption.Diameter
        | AnalyzeOption.DistanceDistribution
        | AnalyzeOption.EigenDistanceDistribution
        | AnalyzeOption.EigenValues
        | AnalyzeOption.LaplacianEigenValues
        | AnalyzeOption.TriangleByVertexDistribution
        | AnalyzeOption.Dr
        )]*/
    public class CollectionResearch : AbstractResearch
    {
        private List<AbstractResearch> subResearches = null;
        private int currentResearchIndex = -1;

        public override void StartResearch()
        {
            ValidateResearchParameters();

            StatusInfo = new ResearchStatusInfo(ResearchStatus.Running, 0);
            Logger.Write("Research ID - " + ResearchID.ToString() +
                ". Research - " + ResearchName + ". STARTED COLLECTION RESEARCH.");

            MatrixPath mp = ((MatrixPath)ResearchParameterValues[ResearchParameter.InputPath]);
            List<MatrixPath> matrixes = new List<MatrixPath>();

            Debug.Assert((File.GetAttributes(mp.Path) & FileAttributes.Directory) == FileAttributes.Directory);
            foreach (string fn in Directory.GetFiles(mp.Path, "*.txt"))
            {
                MatrixPath m = new MatrixPath();
                m.Path = fn;
                m.Size = mp.Size;
                matrixes.Add(m);
            }

            ResearchType rt = ResearchType.Activation;   // TODO think about subresearch type is not supported and is always Basic

            subResearches = new List<AbstractResearch>();
            foreach (MatrixPath m in matrixes)
            {
                AbstractResearch r = AbstractResearch.CreateResearchByType(rt);
                r.ResearchName = ResearchName + "_" + Path.GetFileNameWithoutExtension(m.Path);
                r.GenerationType = GenerationType.Static;
                r.RealizationCount = RealizationCount;
                r.ModelType = ModelType;
                r.TracingPath = "";

                r.ResearchParameterValues = ResearchParameterValues;
                Debug.Assert(r.GenerationParameterValues.ContainsKey(GenerationParameter.AdjacencyMatrix));
                r.GenerationParameterValues[GenerationParameter.AdjacencyMatrix] = m;

                r.AnalyzeOption = AnalyzeOption;

                string storageString = Storage.StorageString;
                // depracate sql storage
                /*if (Storage.GetStorageType() != StorageType.SQLStorage)
                {*/
                    storageString += ResearchName;
                    if (!Directory.Exists(storageString))
                        Directory.CreateDirectory(storageString);
                //}
                r.Storage = AbstractResultStorage.CreateStorage(Storage.GetStorageType(), storageString);
                r.OnUpdateResearchStatus += method;

                subResearches.Add(r);
            }

            if (subResearches.Count() != 0)
            {
                ++currentResearchIndex;
                subResearches[currentResearchIndex].StartResearch();
            }
        }

        private void method(Object sender, ResearchEventArgs e)
        {
            ResearchStatus rs = GetSubresearchStatus(e.ResearchID);
            if (rs == ResearchStatus.Completed)
            {
                Interlocked.Increment(ref currentResearchIndex);
                if (currentResearchIndex >= subResearches.Count())
                {
                    AverageResults();
                    SaveResearch();
                    StatusInfo = new ResearchStatusInfo(ResearchStatus.Completed, (uint)currentResearchIndex);
                }
                else
                {
                    StatusInfo = new ResearchStatusInfo(ResearchStatus.Running, StatusInfo.CompletedStepsCount + 1);
                    subResearches[currentResearchIndex].StartResearch();
                }
            }
        }

        // TODO think about
        private void AverageResults()
        {
            Debug.Assert(subResearches.Count != 0);

            EnsembleResult r = new EnsembleResult(subResearches[0].Result.Size);
            double rCount = subResearches.Count;

            foreach (AnalyzeOption option in subResearches[0].Result.EnsembleResults[0].Result.Keys)
            {
                SortedDictionary<Double, Double> temp = new SortedDictionary<Double, Double>();
                foreach (AbstractResearch res in subResearches)
                {
                    SortedDictionary<Double, Double> d = res.Result.EnsembleResults[0].Result[option] as SortedDictionary<Double, Double>;
                    foreach (KeyValuePair<double, double> k in d)
                    {
                        if (temp.ContainsKey(k.Key))
                            temp[k.Key] += k.Value / rCount;
                        else
                            temp.Add(k.Key, k.Value / rCount);
                    }
                }
                foreach (var item in temp.Where(kvp => kvp.Value.ToString() == "1").ToList())
                {
                    temp.Remove(item.Key);
                }
                temp.Add(temp.Count, 1);
                r.Result.Add(option, temp);
            }            

            result.EnsembleResults.Add(r);
        }

        private ResearchStatus GetSubresearchStatus(Guid id)
        {
            foreach (AbstractResearch r in subResearches)
            {
                if (r.ResearchID == id)
                    return r.StatusInfo.Status;
            }
            Debug.Assert(false);
            return ResearchStatus.NotStarted;
        }

        public override void StopResearch()
        {
            if (subResearches != null)
            {
                foreach (AbstractResearch r in subResearches)
                    r.StopResearch();

                StatusInfo = new ResearchStatusInfo(ResearchStatus.Stopped, StatusInfo.CompletedStepsCount);

                Logger.Write("Research ID - " + ResearchID.ToString() +
                    ". Research - " + ResearchName + ". STOPPED COLLECTION RESEARCH.");
            }
        }

        public override ResearchType GetResearchType()
        {
            return ResearchType.Collection;
        }

        public override int GetProcessStepsCount()
        {
            Debug.Assert(realizationCount == 1);
            if (processStepCount == -1)
            {
                MatrixPath mp = ((MatrixPath)ResearchParameterValues[ResearchParameter.InputPath]);
                processStepCount = Directory.GetFiles(mp.Path, "*.txt").Count();
            }
            return processStepCount;
        }

        protected override void ValidateResearchParameters()
        {
            if (!ResearchParameterValues.ContainsKey(ResearchParameter.InputPath) ||
                ResearchParameterValues[ResearchParameter.InputPath] == null ||
                ((MatrixPath)ResearchParameterValues[ResearchParameter.InputPath]).Path == "")
            {
                Logger.Write("Research - " + ResearchName + ". Invalid research parameters.");
                throw new InvalidResearchParameters();
            }

            MatrixPath mp = ((MatrixPath)ResearchParameterValues[ResearchParameter.InputPath]);
            if ((File.GetAttributes(mp.Path) & FileAttributes.Directory) != FileAttributes.Directory)
            {
                Logger.Write("Research - " + ResearchName + ". Invalid research parameters." +
                    " Directory should be specified.");
                throw new InvalidResearchParameters();
            }

            Logger.Write("Research - " + ResearchName + ". Validated research parameters.");
        }

        protected override void FillParameters(AbstractEnsembleManager m) {}
    }
}