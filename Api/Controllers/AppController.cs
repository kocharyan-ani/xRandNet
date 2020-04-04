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
using Api.Database;
using Api.Database.Repositories;
using Api.Models.Factories;
using File = Api.Models.File;

namespace Api.Controllers {
    [Route("api/app")]
    [ApiController]
    public class AppController : Controller {
        private AppService AppService { get; }
        private BugService BugService { get; }

        public AppController(AppService appService, BugService bugService) {
            AppService = appService;
            BugService = bugService;
        }

        [HttpGet]
        [Route("versions")]
        public ActionResult<IEnumerable<App>> GetVersions() {
            var apps = AppService.List();
            return Ok(apps);
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        [Route("bugs")]
        public ActionResult DeleteBug(Bug bug) {
            BugService.Delete(bug.Id);
            return Ok();
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Route("bugs")]
        public ActionResult<List<Bug>> GetBugs([FromQuery] string version) {
            var bugs = new Bug(); // = AppRepo.Get();
            // BugRepo.Get()
            // TODO find bug list by app version
            return Ok(bugs);
        }

        [HttpPut]
        [Authorize(Roles = "User,Admin")]
        [Route("bugs")]
        public ActionResult<Bug> ReportBug(Bug bug) {
            // BugRepo.Add(bug);
            // TODO  return bug with id
            // if (bugWithId == null) {
            //     throw new Exception("Could not save bug");
            // }

            return Ok();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Route("bugs")]
        public ActionResult<Bug> EditBug(Bug bug) {
            // BugRepo.Update(bug);
            return Ok();
        }

        [HttpGet]
        public ActionResult<byte[]> DownloadApp(string version) {
            var app = AppService.Get(version);
            if (app?.File == null) return NotFound();

            return File(app.File.Data, app.File.MimeType, app.File.Name);
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
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
            AppService.Add(app);
            return Ok();
        }
    }
}