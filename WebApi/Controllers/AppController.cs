using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using WebApi.Database;
using WebApi.Models;
using File = WebApi.Models.File;

namespace WebApi.Controllers
{
    [Route("api/app")]
    [ApiController]
    public class AppController : Controller
    {
        public DbManager DbManager { get; }

        public AppController(DbManager dbManager)
        {
            DbManager = dbManager;
        }

        [HttpGet]
        [Route("versions")]
        public ActionResult<App[]> GetVersions()
        {
            var apps = DbManager.GetAppVersions();
            return Ok(apps);
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        [Route("bugs")]
        public ActionResult DeleteBug(Bug bug)
        {
            DbManager.DeleteBug(bug);
            return Ok();
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Route("bugs")]
        public ActionResult<List<Bug>> GetBugs([FromQuery] string version)
        {
            var bugs = DbManager.GetBugs(version);
            if (bugs == null)
            {
                throw new Exception("Could not get bugs for version " + version);
            }

            return Ok(bugs);
        }

        [HttpPut]
        [Authorize(Roles = "User,Admin")]
        [Route("bugs")]
        public ActionResult<Bug> ReportBug(Bug bug)
        {
            var bugWithId = DbManager.SaveBug(bug);
            if (bugWithId == null)
            {
                throw new Exception("Could not save bug");
            }

            return Ok(bugWithId);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Route("bugs")]
        public ActionResult<Bug> EditBug(Bug bug)
        {
            DbManager.UpdateBug(bug);
            return Ok(bug);
        }

        [HttpGet]
        public ActionResult<byte[]> DownloadApp(string version)
        {
            var app = DbManager.GetApp(version);
            if (app?.File == null) return NotFound();

            return File(app.File.Data, app.File.MimeType, app.File.Name);
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public ActionResult UploadApp()
        {
            var formFile = (FormFile) Request.Form.Files[0];
            var softwareInfo = Request.Form["software"];
            var jObject = JObject.Parse(softwareInfo);
            var stream = formFile.OpenReadStream();
            byte[] data;
            using (var memoryStream = new MemoryStream())
            {
                stream.CopyTo(memoryStream);
                data = memoryStream.ToArray();
            }

            if (data == null) return BadRequest();
            var file = new File {Name = formFile.FileName, MimeType = formFile.ContentType, Data = data};
            var app = new App(jObject["version"].ToString(), file, jObject["releaseNotes"].ToString(),DateTime.Now);
            DbManager.SaveApp(app);
            return Ok();
        }
    }
}