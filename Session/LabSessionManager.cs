using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

using Core;
using Core.Utility;
using Core.Enumerations;
using Core.Attributes;
using Core.Exceptions;
using System.Collections;

namespace Session
{
    public static class LabSessionManager
    {
        private static AbstractResearch existingResearch;
        //private static List<List<EdgesAddedOrRemoved>> steps;

        public static void CreateResearch(ResearchType researchType)
        {
            CustomLogger.VisualMode = true;
            existingResearch = AbstractResearch.CreateResearchByType(researchType);
            existingResearch.ModelType = GetAvailableModelTypes()[0];
            existingResearch.ResearchName = "Default";
            existingResearch.VisualMode = true;
            existingResearch.GenerationType = GenerationType.Random;
        }

        public static void StartResearch()
        {
            // TODO change Wait()
            existingResearch.StartResearch().Wait();
        }

        public static void DestroyResearch()
        {
            existingResearch = null;
        }

        public static bool IsResearchCreated()
        {
            return existingResearch != null ? true : false;
        }

        public static ResearchType GetResearchType()
        {
            return existingResearch.GetResearchType();
        }

        public static string GetResearchName()
        {
            return existingResearch.ResearchName;
        }

        public static void SetResearchName(String name)
        {
            existingResearch.ResearchName = name;
        }

        //Watch again
        public static List<ModelType> GetAvailableModelTypes()
        {
            return AvailableModelTypes(existingResearch.GetType());
        }

        public static List<ModelType> GetAvailableModelTypes(ResearchType rt)
        {
            ResearchTypeInfo[] info = (ResearchTypeInfo[])rt.GetType().GetField(rt.ToString()).GetCustomAttributes(typeof(ResearchTypeInfo), false);
            Type t = Type.GetType(info[0].Implementation, true);

            return AvailableModelTypes(t);
        }

        private static List<ModelType> AvailableModelTypes(Type t)
        {
            List<ModelType> r = new List<ModelType>();
            AvailableModelType[] rAvailableModelTypes = (AvailableModelType[])t.GetCustomAttributes(typeof(AvailableModelType), true);
            for (int i = 0; i < rAvailableModelTypes.Length; ++i)
                r.Add(rAvailableModelTypes[i].ModelType);

            return r;
        }

        public static ModelType GetResearchModelType()
        {
            return existingResearch.ModelType;
        }

        public static void SetResearchModelType(ModelType model)
        {
            existingResearch.ModelType = model;
        }

        public static ResearchStatus GetResearchStatus()
        {
            //To Do
            return existingResearch.StatusInfo.Status;
        }

        public static int GetProcessStepsCount()
        {
            return existingResearch.GetProcessStepsCount();
        }

        public static List<ResearchParameter> GetRequiredResearchParameters(ResearchType rt)
        {
            //To Do
            ResearchTypeInfo[] info = (ResearchTypeInfo[])rt.GetType().GetField(rt.ToString()).GetCustomAttributes(typeof(ResearchTypeInfo), false);
            Type t = Type.GetType(info[0].Implementation, true);

            List<ResearchParameter> rp = new List<ResearchParameter>();
            RequiredResearchParameter[] rRequiredResearchParameters = (RequiredResearchParameter[])t.GetCustomAttributes(typeof(RequiredResearchParameter), true);
            for (int i = 0; i < rRequiredResearchParameters.Length; ++i)
                rp.Add(rRequiredResearchParameters[i].Parameter);

            return rp;
        }

        public static Dictionary<ResearchParameter, object> GetResearchParameterValues()
        {
            return existingResearch.ResearchParameterValues;
        }

        public static void SetResearchParameterValue(ResearchParameter p, object value)
        {
            //ToDo

            existingResearch.ResearchParameterValues[p] = value;

        }

        public static List<GenerationParameter> GetRequiredGenerationParameters(ResearchType rt,
           ModelType mt)
        {
            List<GenerationParameter> gp = new List<GenerationParameter>();

            if (rt == ResearchType.Collection ||
                rt == ResearchType.Structural)
                return gp;

            ModelTypeInfo[] info = (ModelTypeInfo[])mt.GetType().GetField(mt.ToString()).GetCustomAttributes(typeof(ModelTypeInfo), false);
            Type t = Type.GetType(info[0].Implementation, true);

            RequiredGenerationParameter[] rRequiredGenerationParameters = (RequiredGenerationParameter[])t.GetCustomAttributes(typeof(RequiredGenerationParameter), true);
            for (int i = 0; i < rRequiredGenerationParameters.Length; ++i)
            {
                GenerationParameter g = rRequiredGenerationParameters[i].Parameter;
                if (g != GenerationParameter.AdjacencyMatrix)
                    gp.Add(g);
            }

            return gp;
        }

        public static Dictionary<GenerationParameter, object> GetGenerationParameterValues()
        {
            return existingResearch.GenerationParameterValues;
        }

        public static void SetGenerationParameterValue(GenerationParameter p, object value)
        {
            existingResearch.GenerationParameterValues[p] = value;
        }

        public static AnalyzeOption GetAvailableAnalyzeOptions()
        {
            return AnalyzeOption.Algorithm_1_By_All_Nodes;
        }

        public static AnalyzeOption GetAvailableAnalyzeOptions(ResearchType rt, ModelType mt)
        {
            ResearchTypeInfo[] rInfo = (ResearchTypeInfo[])rt.GetType().GetField(rt.ToString()).GetCustomAttributes(typeof(ResearchTypeInfo), false);
            Type researchType = Type.GetType(rInfo[0].Implementation, true);

            AvailableAnalyzeOption rAvailableOptions = ((AvailableAnalyzeOption[])researchType.GetCustomAttributes(typeof(AvailableAnalyzeOption), true))[0];

            ModelTypeInfo[] mInfo = (ModelTypeInfo[])mt.GetType().GetField(mt.ToString()).GetCustomAttributes(typeof(ModelTypeInfo), false);
            Type modelType = Type.GetType(mInfo[0].Implementation, true);

            AvailableAnalyzeOption mAvailableOptions = ((AvailableAnalyzeOption[])modelType.GetCustomAttributes(typeof(AvailableAnalyzeOption), true))[0];

            return rAvailableOptions.Options & mAvailableOptions.Options;
        }

        public static AnalyzeOption GetAnalyzeOptions()
        {
            return existingResearch.AnalyzeOption;
        }

        public static void SetAnalyzeOptions(AnalyzeOption o)
        {
            //To Do
            existingResearch.AnalyzeOption = o;
        }

        /*public static void Generate(int numberOfVertices, double probability, int stepCount = 0,int edges = 0)
        {
            if (steps != null)
            {
                steps.Clear();
            }
            steps = existingResearch.Generate(numberOfVertices,probability,stepCount,edges);
        }*/

        public static List<EdgesAddedOrRemoved> GetStep(int stepNumber)
        {
            Debug.Assert(existingResearch.GenerationSteps != null);
            return existingResearch.GenerationSteps[stepNumber];
        }

        public static int GetStepCount()
        {
            Debug.Assert(existingResearch.GenerationSteps != null);
            return existingResearch.GenerationSteps.Count;
        }

        private static List<BitArray> GetActivationSteps()
        {
            List<BitArray> act = new List<BitArray>(){
            new BitArray(BitConverter.GetBytes(0)),
            new BitArray(BitConverter.GetBytes(1)),
            new BitArray(BitConverter.GetBytes(2)),
            new BitArray(BitConverter.GetBytes(3)),
            new BitArray(BitConverter.GetBytes(4)),
            new BitArray(BitConverter.GetBytes(5)),
            new BitArray(BitConverter.GetBytes(6)),
            new BitArray(BitConverter.GetBytes(7)),
            new BitArray(BitConverter.GetBytes(8)),
            new BitArray(BitConverter.GetBytes(9)),
            new BitArray(BitConverter.GetBytes(10)),
            new BitArray(BitConverter.GetBytes(11)),
            new BitArray(BitConverter.GetBytes(12)),
            new BitArray(BitConverter.GetBytes(13)),
            new BitArray(BitConverter.GetBytes(14)) };
            return act;
        }

        public static BitArray GetActivationStep(int stepNumber)
        {
            //Debug.Assert(existingResearch.ActivesInformation != null);
            //return existingResearch.ActivesInformation[stepNumber];
            return GetActivationSteps()[stepNumber];
        }

        public static int GetActivationStepCount()
        {
            //Debug.Assert(existingResearch.ActivesInformation != null);
            //return existingResearch.ActivesInformation.Count;
            return GetActivationSteps().Count();
        }
        public static int GetActiveNodesCountbyStep(int stepNumber)
        {
            BitArray step = GetActivationStep(stepNumber);
            int activesCount = 0;
            foreach (var e in step)
            {
                if ((bool)e) activesCount++;
            }
            return activesCount;
        }

        public static int GetMaximumActiveNodesCount()
        {
            int maxCount = 0;
            for (int i = 0; i < GetActivationStepCount(); ++i)
            {
                int activesCount = GetActiveNodesCountbyStep(i);
                if (activesCount > maxCount)
                {
                    maxCount = activesCount;
                }
            }
            return maxCount;
        }

        public static int GetMaximumTrianglesCount()
        {
            int maxCount = 0;
            List<int> trianglesCount = GetTrianglesCount();
            for (int i = 0; i < trianglesCount.Count; ++i)
            {
                if (trianglesCount[i] > maxCount)
                {
                    maxCount = trianglesCount[i];
                }
            }
            return maxCount;
        }

        public static List<List<int>> GetBranches()
        {
            Debug.Assert(existingResearch.Branches != null);
            return existingResearch.Branches;
        }

        public static List<List<EdgesAddedOrRemoved>> GetEvolutionInformation()
        {
            return existingResearch.EvolutionInformation;
        }
        public static List<int> GetTrianglesCount()
        {
            return new List<int>() { 4, 5, 5, 7, 8 };
        }



    }
}
