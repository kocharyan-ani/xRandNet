using System;
using System.Collections.Generic;
using System.IO;
using Api.Models;
using Api.Services;
using Api.Services.Bug;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace Api.Controllers {
    [Route("api/app")]
    [ApiController]
    public class AppController : Controller {
        private readonly AppService _appService;
        private readonly BugService _bugService;

        public AppController(AppService appService, BugService bugService) {
            _appService = appService;
            _bugService = bugService;
        }

        [HttpGet]
        [Route("versions")]
        public ActionResult<IEnumerable<App>> GetVersions() {
            var apps = _appService.List();
            return Ok(apps);
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        [Route("bugs")]
        public ActionResult DeleteBug(Bug bug) {
            _bugService.Delete(bug.Id);
            return Ok();
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Route("bugs")]
        public ActionResult<List<Bug>> GetBugs([FromQuery] string version) {
            var bugs = _bugService.List(version);
            return Ok(bugs);
        }

        [HttpPut]
        [Authorize(Roles = "User,Admin")]
        [Route("bugs")]
        public ActionResult<Bug> ReportBug(Bug bug) {
            bug.ReportDate = DateTime.Now;
            _bugService.Add(bug);
            return Ok(bug);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Route("bugs")]
        public ActionResult<Bug> EditBug(Bug bug) {
            _bugService.Update(bug);
            return Ok();
        }

        [HttpGet]
        public ActionResult<byte[]> DownloadApp(string version) {
            var app = _appService.Get(version);
            if (app?.File == null) return NotFound();

            return File(app.File.Data, app.File.MimeType, app.File.Name);
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        [RequestFormLimits(ValueLengthLimit = int.MaxValue, MultipartBodyLengthLimit = int.MaxValue)]
        [DisableRequestSizeLimit]
        [Consumes("multipart/form-data")]
        public ActionResult UploadApp() {
            var formFile = (FormFile) Request.Form.Files[0];
            var softwareInfo = Request.Form["software"];
            var jObject = JObject.Parse(softwareInfo);
            var stream = formFile.OpenReadStream();
            byte[] data;
            using (var memoryStream = new MemoryStream()) {
                stream.CopyTo(memoryStream);
                data = memoryStream.ToArray();
            }

            var file = new AppFile() {Name = formFile.FileName, MimeType = formFile.ContentType, Data = data};
            var app = new App(jObject["version"].ToString(), file, jObject["releaseNotes"].ToString(), DateTime.Now);
            _appService.Add(app);
            return Ok();
        }
    }
}