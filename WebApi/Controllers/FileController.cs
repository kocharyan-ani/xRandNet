using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Route("api/files")]
    [ApiController]
    public class FileController : Controller
    {
        private DB.DBManager dbManager;

        public FileController()
        {
            this.dbManager = new DB.DBManager();
        }

        [HttpGet]
        [Route("downloadApp")]
        public ActionResult<byte[]> DownloadApp(string version)
        {
            Models.App app = this.dbManager.GetApp(version);
            if (app.File == null)
            {
                return NotFound();
            }

            return File(app.File.Data, app.File.MimeType, app.File.Name);
        }

        [HttpPost]
        [Route("uploadApp")]
        public ActionResult UploadApp()
        {
            FormFile formFile = (FormFile)Request.Form.Files[0];
            Stream stream = formFile.OpenReadStream();
            byte[] data = null;
            using (var memoryStream = new MemoryStream())
            {
                stream.CopyTo(memoryStream);
                data = memoryStream.ToArray();
            }
            if (data == null)
            {
                return BadRequest();
            }
            Models.File file = new Models.File();
            file.Name = formFile.FileName;
            file.MimeType = formFile.ContentType;
            file.Data = data;
            this.dbManager.SetUserManual(file);
            return Ok();
        }

        [HttpGet]
        [Route("userManual")]
        public ActionResult<byte[]> DownloadUserManual()
        {
            Models.File usermanualFile = this.dbManager.GetUserManual();
            if (usermanualFile == null)
            {
                return NotFound();
            }

            return File(usermanualFile.Data, usermanualFile.MimeType, usermanualFile.Name);
        }

        [HttpPost]
        [Route("userManual")]
        public ActionResult UploadUserManual()
        {
            FormFile formFile = (FormFile)Request.Form.Files[0];
            Stream stream = formFile.OpenReadStream();
            byte[] data = null;
            using (var memoryStream = new MemoryStream())
            {
                stream.CopyTo(memoryStream);
                data = memoryStream.ToArray();
            }
            if (data == null)
            {
                return BadRequest();
            }
            Models.File file = new Models.File();
            file.Name = formFile.FileName;
            file.MimeType = formFile.ContentType;
            file.Data = data;
            this.dbManager.SetUserManual(file);
            return Ok();
        }
    }
}
