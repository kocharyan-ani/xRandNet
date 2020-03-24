using System.IO;
using System.Linq;
using Api.Database.Models;
using Api.Database.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
            var userManualFile = UserManualFileRepository.Get(1);
            if (userManualFile == null) return NotFound();

            return File(userManualFile.Data, userManualFile.MimeType, userManualFile.Name);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Route("userManual")]
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