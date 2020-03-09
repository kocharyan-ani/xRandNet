using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Core.Enumerations;
using Core.Attributes;
using Core.Exceptions;
using Core.Result;
using Core.Events;
using Core.Utility;

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
        // TODO remove
        //private List<int> degrees;

        protected AbstractEnsembleManager currentManager;
        protected delegate void ManagerRunner();

        private ResearchStatusInfo status;
        protected ResearchResult result = new ResearchResult();

        public List<List<EdgesAddedOrRemoved>> GenerationSteps { get; private set; }

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

        public String ResearchFileName
        {
            get
            {
                if(Storage != null)
                {
                    return Storage.FileName;
                }
                return null;
            }
        }

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

        public bool VisualMode { get; set; }

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
        public virtual Task StartResearch()
        {
            ValidateResearchParameters();

            CreateEnsembleManager();
            StatusInfo = new ResearchStatusInfo(ResearchStatus.Running, 0);
            CustomLogger.Write("Research ID - " + ResearchID.ToString() +
                ". Research - " + ResearchName + ". STARTED " + GetResearchType() + " RESEARCH.");

            return Task.Run(() =>
                {
                    currentManager.Run();
                    RunCompleted();
                });
        }

        /// <summary>
        /// Force stops the research.
        /// </summary>
        public virtual void StopResearch()
        {
            currentManager.Cancel();
            StatusInfo = new ResearchStatusInfo(ResearchStatus.Stopped, StatusInfo.CompletedStepsCount);
            CustomLogger.Write("Research ID - " + ResearchID.ToString() +
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
            CustomLogger.Write("Research - " + ResearchName + ". Validated research parameters.");
        }

        /// <summary>
        /// Callback method whn research running completes.
        /// </summary>
        protected virtual void RunCompleted()
        {
            realizationCount = currentManager.RealizationsDone;
            result.EnsembleResults.Add(currentManager.Result);
            GenerationSteps = currentManager.GenerationSteps;
            if(!VisualMode)
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
            currentManager.VisualMode = VisualMode;
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

            CustomLogger.Write("Research - " + ResearchName + ". Result is SAVED");

            result.Clear();
            CustomLogger.Write("Research - " + ResearchName + ". Result is CLEARED.");
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

        /*public List<List<EdgesAddedOrRemoved>> Generate(int numberOfVertices, double probability, int stepCount = 0, int edges = 0)
        {
            switch (modelType)
            {
                case ModelType.ER:
                    return GenerateER(numberOfVertices, probability, false);
                case ModelType.BA:
                    return GenerateBA(numberOfVertices, probability, stepCount, edges);
                case ModelType.WS:
                    return GenerateWS(numberOfVertices, probability, stepCount, edges);
                default:
                    return null;
            }
        }

        private List<List<EdgesAddedOrRemoved>> GenerateBA(int initialVertices, double probability, int stepCount, int edges)
        {
            List<List<EdgesAddedOrRemoved>> network = new List<List<EdgesAddedOrRemoved>>();
            List<EdgesAddedOrRemoved> initialNetwork = GenerateER(initialVertices, probability, true)[0];

            int n = initialVertices;
            int countEdges = edges;
            int stepNumber = 1;
            while (stepCount > 0)
            {
                double[] probabilityArray = CountProbabilities(n);
                List<EdgesAddedOrRemoved> stepEdges = new List<EdgesAddedOrRemoved>();
                countEdges = edges;
                while (countEdges > 0)
                {
                    for (int i = 0; i < n; i++)
                    {
                        Random rand = new Random(DateTime.Now.Millisecond);
                        double number = rand.Next(0, 100);
                        if (number <= probabilityArray[i] * 100)
                        {
                            EdgesAddedOrRemoved edge = new EdgesAddedOrRemoved(i, initialVertices + stepNumber, true);

                            stepEdges.Add(edge);
                            if (degrees.Count == n)
                            {
                                degrees.Add(0);
                            }
                            degrees[n]++;
                            countEdges--;
                            if (countEdges == 0)
                            {
                                break;
                            }
                        }
                    }

                }
                network.Add(stepEdges);
                stepCount--;
                stepNumber++;
                n++;
            }
            network.Add(initialNetwork);
            return network;
        }

        private List<List<EdgesAddedOrRemoved>> GenerateER(int numberOfVertices, double probability, bool fromBA)
        {
            List<List<EdgesAddedOrRemoved>> edges = new List<List<EdgesAddedOrRemoved>>();
            int edgesCount = 0;

            if (fromBA)
            {
                degrees = new List<int>();
                for (int i = 0; i < numberOfVertices; i++)
                {
                    degrees.Add(0);
                }
            }
            else 
            {
                edges.Add(null);
            }
            List<EdgesAddedOrRemoved> stepList = new List<EdgesAddedOrRemoved>();
            Random rand = new Random(DateTime.Now.Millisecond);
            for (int i = 0; i < numberOfVertices - 1; i++)
            {
                for (int j = i + 1; j < numberOfVertices; j++)
                {
                    double n = rand.Next(0, 100);
                    if (n <= probability * 100)
                    {
                        if (!fromBA)
                        {
                            if (edgesCount >= ((numberOfVertices - 1) / probability))
                            {
                                edges.Add(stepList);
                                stepList = new List<EdgesAddedOrRemoved>();
                                edgesCount = 0;
                            }
                        }
                        else
                        {
                            degrees[i]++;
                            degrees[j]++;
                        }
                        edgesCount++;
                        EdgesAddedOrRemoved edge = new EdgesAddedOrRemoved(i,j,true);
                        stepList.Add(edge);
                    }
                }
            }
            edges.Add(stepList);

            return edges;
        }


        private List<List<EdgesAddedOrRemoved>> GenerateWS(int numberOfVertices, double probability, int stepCount, int edges)
        {
            List<List<EdgesAddedOrRemoved>> network = new List<List<EdgesAddedOrRemoved>>();
            List<EdgesAddedOrRemoved> initialNetwork = new List<EdgesAddedOrRemoved>();
            List<EdgesAddedOrRemoved> previousStep = new List<EdgesAddedOrRemoved>();

            for (int i = 0; i < numberOfVertices; i++)
            {
                EdgesAddedOrRemoved edge;
                for (int j = 1; j <= edges / 2; j++)
                {
                    edge = new EdgesAddedOrRemoved(i, (i + j) % numberOfVertices,true);

                    initialNetwork.Add(edge);
                    previousStep.Add(edge);
                }
            }

            Random rand = new Random(DateTime.Now.Millisecond);
            Random rand2 = new Random(DateTime.Now.Millisecond);

            List<EdgesAddedOrRemoved> step;

            int index;
            int previousStepCount = previousStep.Count;
            EdgesAddedOrRemoved edge1;
            EdgesAddedOrRemoved edge2;
            for (int i = 0; i < stepCount; i++)
            {
                step = new List<EdgesAddedOrRemoved>();
                for (int j = 0; j < previousStepCount; j++)
                {
                    if (previousStep[j].Added)
                    {
                        List<int> availableVertices = new List<int>();

                        for (int k = 0; k < numberOfVertices; ++k)
                        {
                            availableVertices.Add(k);
                        }
                        double p = rand.Next(0, 100);
                        if (p <= probability * 100)
                        {
                            for (int k = 0; k < previousStep.Count; k++)
                            {
                                if (previousStep[k].Added)
                                {
                                    if (previousStep[j].Vertex1 == previousStep[k].Vertex1)
                                    {
                                        availableVertices.Remove(previousStep[k].Vertex2);
                                    }
                                    else if (previousStep[j].Vertex1 == previousStep[k].Vertex2)
                                    {
                                        availableVertices.Remove(previousStep[k].Vertex1);
                                    }
                                }
                            }
                            availableVertices.Remove(previousStep[j].Vertex1);
                           
                            index = rand2.Next(0, availableVertices.Count);

                            edge1 = new EdgesAddedOrRemoved(previousStep[j].Vertex1, availableVertices[index],true);

                            step.Add(edge1);
                            previousStep.Add(edge1);
                           
                            edge2 = previousStep[j];
                            edge2.Added = false;
                            previousStep[j] = edge2;
                            step.Add(edge2);
                        }
                    }
                }
                network.Add(new List<EdgesAddedOrRemoved>(step));
                previousStep = step;
                previousStepCount = previousStep.Count;
            }
            network.Add(initialNetwork);
            return network;
        }


        private double[] CountProbabilities(int numberOfVertices)
        {
            double[] probabilities = new double[numberOfVertices];
            int sum = 0;
            for (int i = 0; i < degrees.Count; i++)
            {
                sum += degrees[i];
            }

            for (int i = 0; i < degrees.Count; i++)
            {
                probabilities[i] = (double)degrees[i] / sum;
            }

            return probabilities;
        }*/
    }
}
