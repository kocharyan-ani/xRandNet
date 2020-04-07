using Microsoft.AspNetCore.Mvc;
using Core.Enumerations;
using Session;
using System;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using Core.Settings;
using Microsoft.AspNetCore.Authorization;

namespace Api.Controllers {
    [Route("api/research")]
    [ApiController]
    public class ResearchController {
        private const string DefaultDirectory = "xRandNet";

        [HttpPost]
        [Authorize]
        [Route("start")]
        public ActionResult<string> Index([FromBody] Models.Research.Research research) {
            // @todo should be replaced
            RandNetSettings.ChangeDefaultDirectory(DefaultDirectory);

            var manager = new WebSessionManager();
            manager.CreateResearch(research.research);
            manager.SetResearchName(research.name);
            manager.SetResearchModelType(research.model);
            manager.SetResearchGenerationType(research.generation);
            manager.SetResearchCheckConnected(research.connected);
            manager.SetResearchRealizationCount(research.count);
            manager.SetResearchStorage(GetType<StorageType>(research.storage));
            var opts = manager.GetAnalyzeOptions();
            foreach (var option in research.analyzeOptions) {
                var current = GetType<AnalyzeOption>(option.key);
                if (option.value)
                    opts |= current;
                else
                    opts &= ~current;
            }

            manager.SetAnalyzeOptions(opts);

            foreach (var parameter in research.parameters) {
                if (Enum.TryParse(parameter.key, out ResearchParameter rp))
                    manager.SetResearchParameterValue(rp, parameter.value);
                else if (Enum.TryParse(parameter.key, out GenerationParameter gp))
                    manager.SetGenerationParameterValue(gp, parameter.value);
            }

            manager.StartResearch().Wait();

            return manager.GetFilePath() + GetFileExtension(research.storage);
        }

        [HttpGet]
        [Authorize]
        [Route("download")]
        public HttpResponseMessage Download([FromQuery] string path) {
            return GetFileResponse(GetFullDirectory(path), path);
        }

        [HttpGet]
        [Authorize]
        [Route("downloadFolder")]
        public HttpResponseMessage DownloadFolder([FromQuery] string path) {
            const string zipExtension = ".zip";
            var fileName = path + zipExtension;
            var folderDirectory = GetFullDirectory(path);
            var fileDirectory = folderDirectory + zipExtension;
            ZipFile.CreateFromDirectory(folderDirectory, fileDirectory);

            var response = GetFileResponse(fileDirectory, fileName);
            File.Delete(fileDirectory);

            return response;
        }

        private static string GetFullDirectory(string path) {
            return DefaultDirectory + "/Results/" + path;
        }

        private static HttpResponseMessage GetFileResponse(string directory, string name) {
            var stream = new MemoryStream();

            using (var file = new FileStream(directory, FileMode.Open, FileAccess.Read)) {
                var bytes = new byte[file.Length];
                file.Read(bytes, 0, (int) file.Length);
                stream.Write(bytes, 0, (int) file.Length);
            }

            stream.Seek(0, SeekOrigin.Begin);
            var response = new HttpResponseMessage(HttpStatusCode.OK) {Content = new StreamContent(stream)};
            response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                {FileName = name};

            return response;
        }

        private static string GetFileExtension(string storage) {
            switch (GetType<StorageType>(storage)) {
                case StorageType.ExcelStorage:
                    return ".xls";
                case StorageType.XMLStorage:
                    return ".xml";
                case StorageType.TXTStorage:
                    return "";
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private static T GetType<T>(string name) {
            return (T) Enum.Parse(typeof(T), name);
        }
    }
}