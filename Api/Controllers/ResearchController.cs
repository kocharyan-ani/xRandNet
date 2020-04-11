using Microsoft.AspNetCore.Mvc;
using Core.Enumerations;
using Session;
using System;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Core.Settings;
using Microsoft.AspNetCore.Authorization;

namespace Api.Controllers {
    [Route("api/research")]
    [ApiController]
    public class ResearchController: Controller {
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
        public Task<IActionResult> Download([FromQuery] string path) {
            return GetFileResponse(GetFullDirectory(path), path);
        }

        [HttpGet]
        [Authorize]
        [Route("downloadFolder")]
        public Task<IActionResult> DownloadFolder([FromQuery] string path) {
            const string zipExtension = ".zip";
            var fileName = path + zipExtension;
            var folderDirectory = GetFullDirectory(path);
            var fileDirectory = folderDirectory + zipExtension;
            ZipFile.CreateFromDirectory(folderDirectory, fileDirectory);

            var response = GetFileResponse(fileDirectory, fileName);
            System.IO.File.Delete(fileDirectory);

            return response;
        }

        private static string GetFullDirectory(string path) {
            return DefaultDirectory + "/Results/" + path;
        }

        private async Task<IActionResult> GetFileResponse(string directory, string name) {
            var memory = new MemoryStream();  
            using (var stream = new FileStream(directory, FileMode.Open))  
            {  
                await stream.CopyToAsync(memory);  
            }  
            memory.Position = 0;  
            return File(memory, MimeMapping.MimeUtility.GetMimeMapping(name), name);
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