using System;
using System.Collections.Generic;

using Core.Enumerations;
using Core.Result;
using Core.Events;

namespace Core
{
    /// <summary>
    /// Abstract class representing ensemble manager.
    /// </summary>
    public abstract class AbstractEnsembleManager
    {
        protected AbstractNetwork[] networks;
        protected Int32 realizationsDone;

        public ModelType ModelType { protected get; set; }

        public String TracingDirectory { protected get; set; }

        public String TracingPath { protected get; set; }

        public bool CheckConnected { protected get; set; }

        public bool VisualMode { protected get; set; }

        public Int32 RealizationCount { protected get; set; }

        public String ResearchName { get; set; }

        public ResearchType ResearchType { get; set; }

        public GenerationType GenerationType { get; set; }

        public TracingType TracingType { get; set; }

        public Dictionary<ResearchParameter, Object> ResearchParamaterValues { get; set; }

        public Dictionary<GenerationParameter, Object> GenerationParameterValues { get; set; }

        public AnalyzeOption AnalyzeOptions { get; set; }

        public NetworkStatusUpdateHandler NetworkStatusUpdateHandlerMethod { get; set; }

        public Int32 RealizationsDone 
        {
            get { return realizationsDone; }
            protected set { realizationsDone = value; }
        }

        public EnsembleResult Result { get; protected set; }
        
        public List<List<EdgesAddedOrRemoved>> GenerationSteps { get; protected set; }

        /// <summary>
        /// Runs generation, analyze and save for each realization in Double ensemble.
        /// Blocks current thread until whole work completes.
        /// </summary>
        public abstract void Run();

        /// <summary>
        /// Terminates running operations.
        /// </summary>
        public abstract void Cancel();
    }
}