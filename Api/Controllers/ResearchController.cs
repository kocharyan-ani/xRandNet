using Api.Models;
using Core.Enumerations;
using Session;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Web.Http;
using AnalyzeOption = Core.Enumerations.AnalyzeOption;

namespace Api.Controllers
{
    public class ResearchController : ApiController
    {
        [HttpPost]
        public string Index([FromBody] Research research)
        {
            var manager = new WebSessionManager();
            manager.CreateResearch(research.research);
            manager.SetResearchName(research.name);
            manager.SetResearchModelType(research.model);
            manager.SetResearchGenerationType(research.generation);
            manager.SetResearchCheckConnected(research.connected);
            manager.SetResearchRealizationCount(research.count);
            manager.SetResearchStorage(GetType<StorageType>(research.storage));
            var opts = manager.GetAnalyzeOptions();
            foreach (var option in research.analyzeOptions)
            {
                var current = GetType<AnalyzeOption>(option.key);
                if (option.value)
                    opts |= current;
                else
                    opts &= ~current;
            }
            manager.SetAnalyzeOptions(opts);

            foreach (var parameter in research.parameters)
            {
                if (Enum.TryParse(parameter.key, out ResearchParameter rp))
                    manager.SetResearchParameterValue(rp, parameter.value);
                else if (Enum.TryParse(parameter.key, out GenerationParameter gp))
                    manager.SetGenerationParameterValue(gp, parameter.value);
            }
            
            manager.StartResearch();

            while(!manager.IsCompleted())
            {
                Thread.Sleep(300);
            }

            return manager.GetFilePath() + ".xml";
        }

        [HttpGet]
        public HttpResponseMessage Download([FromUri] string path)
        {
            var stream = new MemoryStream();

            using (var file = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                var bytes = new byte[file.Length];
                file.Read(bytes, 0, (int)file.Length);
                stream.Write(bytes, 0, (int)file.Length);
            }
            
            stream.Seek(0, SeekOrigin.Begin);
            var response = new HttpResponseMessage(HttpStatusCode.OK) { Content = new StreamContent(stream) };
            response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment") { FileName = path };

            return response;
        }

        private static T GetType<T>(string name)
        {
            return (T)Enum.Parse(typeof(T), name);
        }
    }
}