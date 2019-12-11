using System.IO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi.Database;
using File = WebApi.Models.File;

namespace WebApi.Controllers
{
    [Route("api/files")]
    [ApiController]
    public class FileController : Controller
    {
        public DbManager dbManager { get; }

        public FileController(DbManager dbManager)
        {
            this.dbManager = dbManager;
        }

        [HttpGet]
        [Route("downloadApp")]
        public ActionResult<byte[]> DownloadApp(string version)
        {
            var app = dbManager.GetApp(version);
            if (app.File == null) return NotFound();

            return File(app.File.Data, app.File.MimeType, app.File.Name);
        }

        [HttpPost]
        [Route("uploadApp")]
        public ActionResult UploadApp()
        {
            var formFile = (FormFile) Request.Form.Files[0];
            var stream = formFile.OpenReadStream();
            byte[] data = null;
            using (var memoryStream = new MemoryStream())
            {
                stream.CopyTo(memoryStream);
                data = memoryStream.ToArray();
            }

            if (data == null) return BadRequest();
            var file = new File();
            file.Name = formFile.FileName;
            file.MimeType = formFile.ContentType;
            file.Data = data;
            dbManager.SetUserManual(file);
            return Ok();
        }

        [HttpGet]
        [Route("userManual")]
        [Authorize]
        public ActionResult<byte[]> DownloadUserManual()
        {
            var usermanualFile = dbManager.GetUserManual();
            if (usermanualFile == null) return NotFound();

            return File(usermanualFile.Data, usermanualFile.MimeType, usermanualFile.Name);
        }

        [HttpGet]
        [Route("test")]
        public ActionResult<string> test()
        {
            return "";
        }

        [HttpPost]
        [Route("userManual")]
        public ActionResult UploadUserManual()
        {
            var formFile = (FormFile) Request.Form.Files[0];
            var stream = formFile.OpenReadStream();
            byte[] data = null;
            using (var memoryStream = new MemoryStream())
            {
                stream.CopyTo(memoryStream);
                data = memoryStream.ToArray();
            }

            if (data == null) return BadRequest();
            var file = new File();
            file.Name = formFile.FileName;
            file.MimeType = formFile.ContentType;
            file.Data = data;
            dbManager.SetUserManual(file);
            return Ok();
        }
    }
}