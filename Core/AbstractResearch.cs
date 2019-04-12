﻿using System;
using System.Collections.Generic;
using System.Diagnostics;

using Microsoft.Practices.EnterpriseLibrary.Logging;

using Core.Enumerations;
using Core.Attributes;
using Core.Exceptions;
using Core.Result;
using Core.Events;
using Core.Settings;

namespace Core
{
    /// <summary>
    /// Abstract class presenting Double research. 
    /// </summary>
    public abstract class AbstractResearch
    {
        protected ModelType modelType = ModelType.BA;
        protected GenerationType generationType = GenerationType.Static;
        protected TracingType tracingType = TracingType.Matrix;
        protected int realizationCount = 1;
        protected int processStepCount = -1;

        private ManagerType managerType = ManagerType.Local;
        protected AbstractEnsembleManager currentManager;
        protected delegate void ManagerRunner();

        private ResearchStatusInfo status;
        protected ResearchResult result = new ResearchResult();

        public event ResearchStatusUpdateHandler OnUpdateResearchStatus;

        /// <summary>
        /// Creates a research of specified type using metadata information of enumeration value.
        /// </summary>
        /// <param name="rt">Type of research to create.</param>
        /// <returns>Newly created research.</returns>
        public static AbstractResearch CreateResearchByType(ResearchType rt)
        {
            ResearchTypeInfo[] info = (ResearchTypeInfo[])rt.GetType().GetField(rt.ToString()).GetCustomAttributes(typeof(ResearchTypeInfo), false);
            Type t = Type.GetType(info[0].Implementation, true);
            return (AbstractResearch)t.GetConstructor(Type.EmptyTypes).Invoke(new Object[0]);
        }

        public AbstractResearch()
        {
            ResearchID = Guid.NewGuid();

            ResearchParameterValues = new Dictionary<ResearchParameter, Object>();
            GenerationParameterValues = new Dictionary<GenerationParameter, Object>();
            AnalyzeOption = AnalyzeOption.None;

            InitializeResearchParameters();
            InitializeGenerationParameters();
        }

        public Guid ResearchID { get; private set; }

        public String ResearchName { get; set; }

        public ModelType ModelType
        {
            get { return modelType; }
            set
            {
                List<AvailableModelType> l = new List<AvailableModelType>((AvailableModelType[])this.GetType().GetCustomAttributes(typeof(AvailableModelType), true));
                if (l.Exists(x => x.ModelType == value))
                {
                    modelType = value;
                    InitializeGenerationParameters();
                }
                else
                    throw new CoreException("Research does not support specified model type.");
            }
        }

        public GenerationType GenerationType
        {
            get { return generationType; }
            set
            {
                List<AvailableGenerationType> l = new List<AvailableGenerationType>((AvailableGenerationType[])this.GetType().GetCustomAttributes(typeof(AvailableGenerationType), true));
                if (l.Exists(x => x.GenerationType == value))
                {
                    generationType = value;
                    InitializeGenerationParameters();
                }
                else
                    throw new CoreException("Research does not support specified generation type.");
            }
        }

        public TracingType TracingType
        {
            get { return tracingType; }
            set { tracingType = value; }
        }

        public AbstractResultStorage Storage { get; set; }

        public String TracingPath { get; set; }

        public bool CheckConnected { get; set; }

        public ResearchStatusInfo StatusInfo 
        {
            get { return status; }
            protected set
            {
                status = value;

                // Make sure someone is listening to event
                if (OnUpdateResearchStatus == null)
                    return;

                // Invoke event for GUI
                OnUpdateResearchStatus(this, new ResearchEventArgs(ResearchID));
            }
        }

        public int RealizationCount
        {
            get { return realizationCount; }
            set 
            {
                if (value > 0)
                    realizationCount = value;
                else
                    throw new CoreException("Realization count cannot be less then 1.");
            }
        }

        public Dictionary<ResearchParameter, Object> ResearchParameterValues { get; set; }

        public Dictionary<GenerationParameter, Object> GenerationParameterValues { get; set; }

        public AnalyzeOption AnalyzeOption { get; set; }

        public ResearchResult Result
        {
            get { return result; }
        }

        /// <summary>
        /// Creates single EnsembleManager, runs in background thread.
        /// </summary>
        public virtual void StartResearch()
        {
            ValidateResearchParameters();

            CreateEnsembleManager();
            StatusInfo = new ResearchStatusInfo(ResearchStatus.Running, 0);
            Logger.Write("Research ID - " + ResearchID.ToString() +
                ". Research - " + ResearchName + ". STARTED " + GetResearchType() + " RESEARCH.");

            ManagerRunner r = new ManagerRunner(currentManager.Run);
            r.BeginInvoke(new AsyncCallback(RunCompleted), null);
        }

        /// <summary>
        /// Force stops the research.
        /// </summary>
        public virtual void StopResearch()
        {
            currentManager.Cancel();
            StatusInfo = new ResearchStatusInfo(ResearchStatus.Stopped, StatusInfo.CompletedStepsCount);
            Logger.Write("Research ID - " + ResearchID.ToString() +
                ". Research - " + ResearchName + ". STOPPED " + GetResearchType() + " RESEARCH.");
        }

        /// <summary>
        /// Returns research type.
        /// </summary>
        /// <returns>Research type.</returns>
        public abstract ResearchType GetResearchType();

        /// <summary>
        /// Returns count of steps in research working process.
        /// </summary>
        /// <returns>Count of steps.</returns>
        /// <note>Steps are {generation, tracing, each analyze option calculating, save}</note>
        public abstract int GetProcessStepsCount();

        /// <summary>
        /// Validates research parameters.
        /// </summary>
        /// <exception>InvalidResearchParameters.</exception>
        protected virtual void ValidateResearchParameters()
        {
            Logger.Write("Research - " + ResearchName + ". Validated research parameters.");
        }

        /// <summary>
        /// Callback method whn research running completes.
        /// </summary>
        protected virtual void RunCompleted(IAsyncResult res)
        {
            realizationCount = currentManager.RealizationsDone;
            result.EnsembleResults.Add(currentManager.Result);
            SaveResearch();
        }

        /// <summary>
        /// Creates ensemble manager of corresponding type and 
        /// initializes from current research.
        /// </summary>
        protected void CreateEnsembleManager()
        {
            ManagerTypeInfo[] info = (ManagerTypeInfo[])managerType.GetType().GetField(managerType.ToString()).GetCustomAttributes(typeof(ManagerTypeInfo), false);
            Type t = Type.GetType(info[0].Implementation);
            currentManager = (AbstractEnsembleManager)t.GetConstructor(Type.EmptyTypes).Invoke(new Object[0]);

            currentManager.ModelType = modelType;
            currentManager.TracingDirectory = TracingPath == "" ? "" : TracingPath;
            currentManager.TracingPath = (TracingPath == "" ? "" : TracingPath + "\\" + ResearchName);
            currentManager.CheckConnected = CheckConnected;
            currentManager.RealizationCount = realizationCount;
            currentManager.ResearchName = ResearchName;
            currentManager.ResearchType = GetResearchType();
            currentManager.GenerationType = GenerationType;
            currentManager.TracingType = TracingType;
            currentManager.AnalyzeOptions = AnalyzeOption;
            currentManager.NetworkStatusUpdateHandlerMethod = AbstractResearch_OnUpdateNetworkStatus;
            FillParameters(currentManager);
        }

        /// <summary>
        /// Initializes generation parameters for Double ensemble manager.
        /// </summary>
        /// <param name="m">Ensemble manager to initialize.</param>
        protected abstract void FillParameters(AbstractEnsembleManager m);

        /// <summary>
        /// Saves the results of research analyze.
        /// </summary>
        protected void SaveResearch()
        {
            if (result.EnsembleResults.Count == 0 || result.EnsembleResults[0] == null)
            {
                StatusInfo = new ResearchStatusInfo(ResearchStatus.Failed, StatusInfo.CompletedStepsCount + 1);
                return;
            }
            result.ResearchID = ResearchID;
            result.ResearchName = ResearchName;
            result.ResearchType = GetResearchType();
            result.ModelType = modelType;
            result.RealizationCount = realizationCount;
            result.Size = result.EnsembleResults[0].NetworkSize;
            result.Edges = result.EnsembleResults[0].EdgesCount;
            result.Date = DateTime.Now;

            result.ResearchParameterValues = ResearchParameterValues;
            result.GenerationParameterValues = GenerationParameterValues;

            Storage.Save(result);
            StatusInfo = new ResearchStatusInfo(ResearchStatus.Completed, StatusInfo.CompletedStepsCount + 1);

            Logger.Write("Research - " + ResearchName + ". Result is SAVED");

            result.Clear();
            Logger.Write("Research - " + ResearchName + ". Result is CLEARED.");
        }

        protected int GetAnalyzeOptionsCount()
        {
            int counter = 0;
            Array existingOptions = Enum.GetValues(typeof(AnalyzeOption));
            foreach (AnalyzeOption opt in existingOptions)
                if (AnalyzeOption.HasFlag(opt) && opt != AnalyzeOption.None)
                    ++counter;

            return counter;
        }

        private void InitializeResearchParameters()
        {
            ResearchParameterValues.Clear();

            RequiredResearchParameter[] rp = (RequiredResearchParameter[])this.GetType().GetCustomAttributes(typeof(RequiredResearchParameter), true);
            for (int i = 0; i < rp.Length; ++i)
            {
                ResearchParameter p = rp[i].Parameter;
                ResearchParameterInfo[] info = (ResearchParameterInfo[])p.GetType().GetField(p.ToString()).GetCustomAttributes(typeof(ResearchParameterInfo), false);
                ResearchParameterValues.Add(rp[i].Parameter, info[0].DefaultValue);
            }
        }

        private void InitializeGenerationParameters()
        {
            GenerationParameterValues.Clear();

            if (GetResearchType() == ResearchType.Collection ||
                GetResearchType() == ResearchType.Structural)
                return;

            if (generationType == GenerationType.Static)
            {
                GenerationParameterValues.Add(GenerationParameter.AdjacencyMatrix, null);
                return;
            }

            ModelTypeInfo info = ((ModelTypeInfo[])modelType.GetType().GetField(modelType.ToString()).GetCustomAttributes(typeof(ModelTypeInfo), false))[0];
            Type t = Type.GetType(info.Implementation, true);
            RequiredGenerationParameter[] gp = (RequiredGenerationParameter[])t.GetCustomAttributes(typeof(RequiredGenerationParameter), false);
            for (int i = 0; i < gp.Length; ++i)
            {
                GenerationParameter g = gp[i].Parameter;
                if (g != GenerationParameter.AdjacencyMatrix)
                {
                    GenerationParameterInfo[] gInfo = (GenerationParameterInfo[])g.GetType().GetField(g.ToString()).GetCustomAttributes(typeof(GenerationParameterInfo), false);
                    GenerationParameterValues.Add(g, gInfo[0].DefaultValue);
                }
            }
        }

        private void AbstractResearch_OnUpdateNetworkStatus(Object sender, NetworkEventArgs e)
        {
            switch (e.Status)
            {
                case NetworkStatus.StepCompleted:
                    StatusInfo = new ResearchStatusInfo(ResearchStatus.Running, StatusInfo.CompletedStepsCount + 1);
                    break;
                case NetworkStatus.Failed:
                    StatusInfo = new ResearchStatusInfo(ResearchStatus.Failed, StatusInfo.CompletedStepsCount);
                    break;
                default:
                    Debug.Assert(false);
                    break;
            }
        }
    }
}
