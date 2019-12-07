using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;

using Core.Attributes;
using Core.Enumerations;
using Core.Exceptions;
using Core.Events;
using Core.Model;
using Core.Result;
using Core.Utility;

namespace Core
{
    /// <summary>
    /// Abstract class presenting random network.
    /// </summary>
    public abstract class AbstractNetwork
    {
        public String ResearchName { get; private set; }
        public ResearchType ResearchType { get; private set; }
        public GenerationType GenerationType { get; private set; }
        public TracingType TracingType { get; private set; }
        public Dictionary<ResearchParameter, Object> ResearchParameterValues { get; private set; }
        public Dictionary<GenerationParameter, Object> GenerationParameterValues { get; private set; }
        public AnalyzeOption AnalyzeOptions { get; private set; }

        protected INetworkGenerator networkGenerator;
        protected INetworkAnalyzer networkAnalyzer;

        public int NetworkID { get; set; }
        public bool SuccessfullyCompleted { get; private set; }
        public RealizationResult NetworkResult { get; protected set; }

        public event NetworkStatusUpdateHandler OnUpdateStatus;

        public static AbstractNetwork CreateNetworkByType(ModelType mt, String rName,
            ResearchType rType, GenerationType gType, TracingType tType,
            Dictionary<ResearchParameter, Object> rParams,
            Dictionary<GenerationParameter, Object> genParams,
            AnalyzeOption AnalyzeOptions)
        {
            ModelTypeInfo[] info = (ModelTypeInfo[])mt.GetType().GetField(mt.ToString()).GetCustomAttributes(typeof(ModelTypeInfo), false);
            Type t = Type.GetType(info[0].Implementation);
            Type[] constructTypes = new Type[] {
                    typeof(String),
                    typeof(ResearchType),
                    typeof(GenerationType),
                    typeof(TracingType),
                    typeof(Dictionary<ResearchParameter, Object>),
                    typeof(Dictionary<GenerationParameter, Object>),
                    typeof(AnalyzeOption) };
            Object[] invokeParams = new Object[] {
                    rName,
                    rType,
                    gType,
                    tType,
                    rParams,
                    genParams,
                    AnalyzeOptions };
            return (AbstractNetwork)t.GetConstructor(constructTypes).Invoke(invokeParams);
        }

        public AbstractNetwork(String rName, ResearchType rType,
            GenerationType gType, TracingType tType,
            Dictionary<ResearchParameter, Object> rParams,
            Dictionary<GenerationParameter, Object> genParams,
            AnalyzeOption AnalyzeOptions)
        {
            ResearchName = rName;
            ResearchType = rType;
            GenerationType = gType;
            TracingType = tType;
            ResearchParameterValues = rParams;
            GenerationParameterValues = genParams;
            this.AnalyzeOptions = AnalyzeOptions;

            NetworkResult = new RealizationResult();
        }

        /// <summary>
        /// Generates random network from generation parameters.
        /// </summary>
        public bool Generate()
        {
            try
            {
                CustomLogger.Write("Research - " + ResearchName +
                    ". GENERATION STARTED for network - " + NetworkID.ToString());

                if (GenerationType == GenerationType.Static)
                {
                    Debug.Assert(GenerationParameterValues.ContainsKey(GenerationParameter.AdjacencyMatrix));
                    MatrixPath fp = (MatrixPath)GenerationParameterValues[GenerationParameter.AdjacencyMatrix];
                    Debug.Assert(fp.Path != null && fp.Path != "");
                    NetworkInfoToRead ni = FileManager.Read(fp.Path, fp.Size);
                    networkGenerator.StaticGeneration(ni);

                    CustomLogger.Write("Research - " + ResearchName +
                        ". Static GENERATION FINISHED for network - " + NetworkID.ToString());
                }
                else
                {
                    Debug.Assert(!GenerationParameterValues.ContainsKey(GenerationParameter.AdjacencyMatrix));
                    networkGenerator.RandomGeneration(GenerationParameterValues);
                    if (ResearchType == ResearchType.Activation)
                    {
                        Debug.Assert(ResearchParameterValues.ContainsKey(ResearchParameter.InitialActivationProbability));
                        Double IP = Double.Parse(ResearchParameterValues[ResearchParameter.InitialActivationProbability].ToString(),
                            CultureInfo.InvariantCulture);
                        (networkGenerator.Container as AbstractNetworkContainer).RandomActivating(IP);
                    }

                    CustomLogger.Write("Research - " + ResearchName +
                        ". Random GENERATION FINISHED for network - " + NetworkID.ToString());
                }

                UpdateStatus(NetworkStatus.StepCompleted);
            }
            catch(CoreException ex)
            {
                UpdateStatus(NetworkStatus.Failed);

                CustomLogger.Write("Research - " + ResearchName +
                    "GENERATION FAILED for network - " + NetworkID.ToString() +
                    ". Exception message: " + ex.Message);
                return false;
            }

            return true;
        }

        public bool CheckConnected()
        {
            try
            {
                CustomLogger.Write("Research - " + ResearchName +
                    ". CHECK CONNECTED STARTED for network - " + NetworkID.ToString());

                bool result = false;
                Debug.Assert(networkGenerator.Container != null);
                networkAnalyzer.Container = networkGenerator.Container;
                Object ccd = networkAnalyzer.CalculateOption(AnalyzeOption.ConnectedComponentDistribution);
                Debug.Assert(ccd is SortedDictionary<Double, Double>);
                SortedDictionary<Double, Double> r = ccd as SortedDictionary<Double, Double>;
                result = r.ContainsKey(networkGenerator.Container.Size);
                if (!result)
                {
                    UpdateStatus(NetworkStatus.StepCompleted);  // for analyze
                }

                CustomLogger.Write("Research - " + ResearchName +
                        ". CHECK COMPLETED FINISHED for network - " + NetworkID.ToString());

                return result;
            }
            catch (CoreException ex)
            {
                UpdateStatus(NetworkStatus.Failed);

                CustomLogger.Write("Research - " + ResearchName +
                    "CHECK COMPLETED FAILED for network - " + NetworkID.ToString() +
                    ". Exception message: " + ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Calculates specified analyze options values.
        /// </summary>
        public bool Analyze()
        {
            if (networkAnalyzer.Container == null)
                networkAnalyzer.Container = networkGenerator.Container;
            
            try
            {
                CustomLogger.Write("Research - " + ResearchName +
                    ". ANALYZE STARTED for network - " + NetworkID.ToString());

                NetworkResult.NetworkSize = networkAnalyzer.Container.Size;
                NetworkResult.EdgesCount = networkAnalyzer.CalculateEdgesCount();

                Array existingOptions = Enum.GetValues(typeof(AnalyzeOption));
                foreach (AnalyzeOption opt in existingOptions)
                {
                    if (opt != AnalyzeOption.None && (AnalyzeOptions & opt) == opt)
                    {
                        NetworkResult.Result.Add(opt, networkAnalyzer.CalculateOption(opt));
                        UpdateStatus(NetworkStatus.StepCompleted);

                        CustomLogger.Write("Research - " + ResearchName +
                            ". CALCULATED analyze option: " +  opt.ToString() +
                            " for network - " + NetworkID.ToString());
                    }
                }

                SuccessfullyCompleted = true;

                CustomLogger.Write("Research - " + ResearchName +
                        ". ANALYZE FINISHED for network - " + NetworkID.ToString());
            }
            catch (SystemException ex)
            {
                UpdateStatus(NetworkStatus.Failed);

                CustomLogger.Write("Research - " + ResearchName +
                    ". ANALYZE FAILED for network - " + NetworkID.ToString() +
                    ". Exception message: " + ex.Message);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Traces the adjacency matrix of generated network to file.
        /// </summary>
        public bool Trace(String tracingDirectory, String tracingPath)
        {
            try
            {
                CustomLogger.Write("Research - " + ResearchName +
                    ". TRACING STARTED for network - " + NetworkID.ToString());

                if (TracingIsNotSupported())
                    return true;

                if (TracingType == TracingType.Matrix)
                {
                    MatrixInfoToWrite matrixInfo = new MatrixInfoToWrite();
                    matrixInfo.Matrix = networkGenerator.Container.GetMatrix();
                    matrixInfo.Branches = networkGenerator.Container.GetBranches();
                    matrixInfo.ActiveStates = (networkGenerator.Container as AbstractNetworkContainer).GetActiveStatuses();

                    FileManager.Write(tracingDirectory, matrixInfo, tracingPath);
                }
                else if (TracingType == TracingType.Neighbourship)
                {
                    NeighbourshipInfoToWrite neighbourshipInfo = new NeighbourshipInfoToWrite();
                    neighbourshipInfo.Neighbourship = networkGenerator.Container.GetNeighbourship();
                    neighbourshipInfo.Branches = networkGenerator.Container.GetBranches();
                    neighbourshipInfo.ActiveStates = (networkGenerator.Container as AbstractNetworkContainer).GetActiveStatuses();

                    FileManager.Write(tracingDirectory, neighbourshipInfo, tracingPath);
                }

                UpdateStatus(NetworkStatus.StepCompleted);

                CustomLogger.Write("Research - " + ResearchName +
                        ". TRACING FINISHED for network - " + NetworkID.ToString());
            }
            catch (CoreException ex)
            {
                UpdateStatus(NetworkStatus.Failed);

                CustomLogger.Write("Research - " + ResearchName +
                    "TRACING FAILED for network - " + NetworkID.ToString() +
                    ". Exception message: " + ex.Message);
                return false;
            }

            return true;
        }

        private bool TracingIsNotSupported()
        {
            bool support = networkGenerator.Container.Size > 30000;
            //if (networkGenerator.Container is AbstractHierarchicContainer)
            //{
            //    networkAnalyzer.Container = networkGenerator.Container;
            //    support = (networkAnalyzer.CalculateEdgesCount() > 10000000);
            //}
            //else support = (networkGenerator.Container.Size > 30000);

            if (support)
            {
                UpdateStatus(NetworkStatus.StepCompleted);

                CustomLogger.Write("Research - " + ResearchName +
                        ". TRACING is SKIPPED for network - " + NetworkID.ToString());
            }

            return support;
        }

        public void UpdateStatus(NetworkStatus status)
        {
            // Make sure someone is listening to event
            if (OnUpdateStatus == null) 
                return;

            // Invoke event for AbstractEnsembleManager
            OnUpdateStatus(this, new NetworkEventArgs(status));
        }
    }
}
