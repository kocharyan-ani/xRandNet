using Api.Models;
using Core.Enumerations;
using Core.Utility;
using Session;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Web.Http;

namespace Api.Controllers
{
    public class ResearchController : ApiController
    {
        [HttpPost]
        public string Index([FromBody] Research research)
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

            string filePath = manager.GetFilePath();
            manager.StartResearch();

            while(!manager.IsCompleted())
            {
                Thread.Sleep(300);
            }

            return filePath;
        }

        [HttpGet]
        public HttpResponseMessage Donwload([FromUri] string path)
        {
            var stream = new MemoryStream();

            using (FileStream file = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                byte[] bytes = new byte[file.Length];
                file.Read(bytes, 0, (int)file.Length);
                stream.Write(bytes, 0, (int)file.Length);
            }
            
            stream.Seek(0, SeekOrigin.Begin);
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK) { Content = new StreamContent(stream) };
            response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment") { FileName = path };

            return response;
        }

        private T GetType<T>(string name)
        {
            return (T)Enum.Parse(typeof(T), name);
        }
    }
}