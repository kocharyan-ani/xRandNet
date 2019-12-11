using System;
using System.Collections.Generic;
using Core;
using Core.Attributes;
using Core.Enumerations;
using Core.Settings;
using Core.Utility;

namespace Session
{
    public class WebSessionManager
    {
        AbstractResearch research;

        public WebSessionManager()
        {
            CustomLogger.webMode = true;
        }

        public Guid CreateResearch(String type)
        {
            research = AbstractResearch.CreateResearchByType(GetType<ResearchType>(type));
            research.ModelType = GetAvailableModelTypes(research.GetType())[0];
            research.ResearchName = "Default";
            research.TracingPath = "";
            research.CheckConnected = false;

            return research.ResearchID;
        }

        private T GetType<T>(string name)
        {
            return (T)Enum.Parse(typeof(T), name);
        }

        public List<ModelType> GetAvailableModelTypes(Type type)
        {
            return AvailableModelTypes(type);
        }

        private List<ModelType> AvailableModelTypes(Type t)
        {
            List<ModelType> r = new List<ModelType>();
            AvailableModelType[] rAvailableModelTypes = (AvailableModelType[])t.GetCustomAttributes(typeof(AvailableModelType), true);
            for (int i = 0; i < rAvailableModelTypes.Length; ++i)
                r.Add(rAvailableModelTypes[i].ModelType);

            return r;
        }

        public void SetResearchName(string researchName)
        {
            research.ResearchName = researchName;
        }

        public void SetResearchModelType(string modelType)
        {
            research.ModelType = GetType<ModelType>(modelType);
        }

        public void SetResearchGenerationType(string generationType)
        {
            research.GenerationType = GetType<GenerationType>(generationType);
        }

        public void SetResearchCheckConnected(bool checkConnected)
        {
            research.CheckConnected = checkConnected;
        }

        public void SetResearchRealizationCount(int realizationCount)
        {
            research.RealizationCount = realizationCount;
        }

        public AnalyzeOption GetAnalyzeOptions()
        {
            return research.AnalyzeOption;
        }

        public void SetAnalyzeOptions(AnalyzeOption o)
        {
            research.AnalyzeOption = o;
        }

        public void SetResearchStorage(StorageType storageType)
        {
            research.Storage = AbstractResultStorage.CreateStorage(storageType, RandNetSettings.StorageDirectory);
        }

        public void SetResearchParameterValue(ResearchParameter p, Object value)
        {
            research.ResearchParameterValues[p] = value;
        }

        public void SetGenerationParameterValue(GenerationParameter p, Object value)
        {
            research.GenerationParameterValues[p] = value;
        }

        public string GetFilePath()
        {
            return research.ResearchFileName;
        }

        public bool IsCompleted()
        {
            return research.StatusInfo.Status == ResearchStatus.Completed;
        }

        public void StartResearch()
        {
            research.StartResearch();
        }
    }
}