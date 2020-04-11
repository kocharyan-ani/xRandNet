using System.IO;
using System.Linq;
using Api.Database.Models;
using Api.Database.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using File = Api.Database.Models.File;

namespace Api.Controllers {
    [Route("api/files")]
    [ApiController]
    public class FileController : Controller {
        public IUserManualFileRepository UserManualFileRepository;

        public FileController(IUserManualFileRepository userManualFileRepository) {
            UserManualFileRepository = userManualFileRepository;
        }

        [HttpGet]
        [Route("userManual")]
        public ActionResult<byte[]> DownloadUserManual() {
            var files = UserManualFileRepository.List();
            var manualFiles = files as File[] ?? files.ToArray();
            if (manualFiles.Length == 0) return NotFound();
            return File(manualFiles[0].Data, manualFiles[0].MimeType, manualFiles[0].Name);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Route("userManual")]
        [RequestFormLimits(ValueLengthLimit = int.MaxValue, MultipartBodyLengthLimit = int.MaxValue)] 
        [DisableRequestSizeLimit] 
        [Consumes("multipart/form-data")]
        public ActionResult UploadUserManual() {
            var formFile = (FormFile) Request.Form.Files[0];
            var stream = formFile.OpenReadStream();
            byte[] data = null;
            using (var memoryStream = new MemoryStream()) {
                stream.CopyTo(memoryStream);
                data = memoryStream.ToArray();
            }

            var file = UserManualFileRepository.List().FirstOrDefault();
            if (file == null) {
                file = new ManualFile();
            }

            file.Name = formFile.FileName;
            file.MimeType = formFile.ContentType;
            file.Data = data;
            UserManualFileRepository.Update(file);
            return Ok();
        }
    }
}