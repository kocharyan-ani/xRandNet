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
        public DbManager DbManager { get; }

        public FileController(DbManager dbManager)
        {
            DbManager = dbManager;
        }

        [HttpGet]
        [Route("userManual")]
        public ActionResult<byte[]> DownloadUserManual()
        {
            var userManualFile = DbManager.GetUserManual();
            if (userManualFile == null) return NotFound();

            return File(userManualFile.Data, userManualFile.MimeType, userManualFile.Name);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
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
            var file = new File {Name = formFile.FileName, MimeType = formFile.ContentType, Data = data};
            DbManager.SetUserManual(file);
            return Ok();
        }
    }
}