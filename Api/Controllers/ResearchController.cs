using Api.Models;
using Core.Enumerations;
using Session;
using System;
using System.Web.Http;
using System.Web.Http.Cors;

namespace Api.Controllers
{
    public class ResearchController : ApiController
    {
        [HttpPost]
        public String Index([System.Web.Http.FromBody] Research research)
        {
            WebSessionManager manager = new WebSessionManager();
            manager.CreateResearch(research.research);
            manager.SetResearchName(research.name);
            manager.SetResearchModelType(research.model);
            manager.SetResearchGenerationType(research.generation);
            manager.SetResearchCheckConnected(research.connected);
            manager.SetResearchRealizationCount(research.count);
            manager.SetResearchStorage(GetType<StorageType>(research.storage));

            Core.Enumerations.AnalyzeOption opts = manager.GetAnalyzeOptions();
            foreach (Models.AnalyzeOption option in research.analyzeOptions)
            {
                Core.Enumerations.AnalyzeOption current = GetType<Core.Enumerations.AnalyzeOption>(option.key);
                if (option.value)
                    opts |= current;
                else
                    opts &= ~current;
            }
            manager.SetAnalyzeOptions(opts);

            foreach (Parameter parameter in research.parameters)
            {
                ResearchParameter rp;
                GenerationParameter gp;
                if (Enum.TryParse(parameter.key, out rp))
                    manager.SetResearchParameterValue(rp, parameter.value);
                else if (Enum.TryParse(parameter.key, out gp))
                    manager.SetGenerationParameterValue(gp, parameter.value);
            }

            manager.StartResearch();

            return research.analyzeOptions[0].key;
        }


        private T GetType<T>(string name)
        {
            return (T)Enum.Parse(typeof(T), name);
        }
    }
}