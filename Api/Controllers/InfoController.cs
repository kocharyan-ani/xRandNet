using System;
using System.IO;
using System.Linq;
using Api.Models;
using Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace Api.Controllers {
    [Route("api/data")]
    [ApiController]
    public class InfoController : Controller {
        private readonly InfoService _infoService;
        private readonly LinkService _linkService;
        private readonly NewsService _newsService;
        private readonly ProjectService _projectService;
        private readonly PublicationService _publicationService;
        private readonly PersonService _personService;

        public InfoController(InfoService infoService, LinkService linkService, NewsService newsService,
            ProjectService projectService, PublicationService publicationService, PersonService personService) {
            _infoService = infoService;
            _linkService = linkService;
            _newsService = newsService;
            _publicationService = publicationService;
            _personService = personService;
            _projectService = projectService;
        }

        [HttpGet]
        [Route("info")]
        public ActionResult<string> GetInfoAboutUs() {
            var info = _infoService.Get();
            if (info == null) {
                return  Ok("");
            }

            return Ok(info);
        }

        [HttpPost]
        // [Authorize(Roles = "Admin")]
        [Route("info")]
        public ActionResult<string> EditInfoAboutUs([FromBody] Info info) {
            _infoService.Update(info);
            return Ok(info);
        }

        [HttpGet]
        [Route("links")]
        public ActionResult<string> GetLinksByType([FromQuery] int type) {
            var links = _linkService.List().Where(link => link.Type == type);
            return Ok(links);
        }

        [HttpDelete]
        [Route("links")]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteLinks([FromQuery] int id) {
            if (id == 0) {
                return BadRequest("Link id should be specified");
            }

            _linkService.Delete(id);
            return Ok();
        }

        [HttpPut]
        [Route("links")]
        [Authorize(Roles = "Admin")]
        public ActionResult<string> AddLink([FromBody] Link link) {
            _linkService.Add(link);
            return Ok(link);
        }

        [HttpPost]
        [Route("links")]
        [Authorize(Roles = "Admin")]
        public ActionResult<string> EditLink([FromBody] Link link) {
            _linkService.Update(link);
            return Ok(link);
        }

        [HttpGet]
        [Route("news")]
        public ActionResult<string> GetNews() {
            var news = _newsService.List();
            return Ok(news);
        }

        [HttpPut]
        [Route("news")]
        [Authorize(Roles = "Admin")]
        public ActionResult<News> AddNews([FromBody] News news) {
            news.DatePosted = DateTime.Now;
            _newsService.Add(news);
            return Ok(news);
        }

        [HttpPost]
        [Route("news")]
        [Authorize(Roles = "Admin")]
        public ActionResult<News> UpdateNews([FromBody] News news) {
            news.DatePosted = DateTime.Now;
            _newsService.Update(news);
            return Ok(news);
        }

        [HttpDelete]
        [Route("news")]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteNews([FromBody] News news) {
            _newsService.Delete(news.Id);
            return Ok();
        }

        [HttpGet]
        [Route("people")]
        public ActionResult<string> GetPeople() {
            var news = _personService.List();
            return Ok(news);
        }

        [HttpPut]
        [Route("people")]
        [Authorize(Roles = "Admin")]
        public ActionResult<News> AddPerson([FromBody] Person person) {
            _personService.Add(person);
            return Ok(person);
        }

        [HttpPost]
        [Route("people")]
        [Authorize(Roles = "Admin")]
        public ActionResult<News> UpdatePerson([FromBody] Person person) {
            _personService.Update(person);
            return Ok(person);
        }

        [HttpDelete]
        [Route("projects")]
        [Authorize(Roles = "Admin")]
        public ActionResult DeletePerson([FromBody] Person person) {
            _personService.Delete(person.Id);
            return Ok();
        }

        [HttpGet]
        [Route("projects")]
        public ActionResult<string> GetProjects() {
            var projects = _projectService.List();
            return Ok(projects);
        }

        [HttpPut]
        [Route("projects")]
        [Authorize(Roles = "Admin")]
        public ActionResult<News> AddProject([FromBody] Project project) {
            _projectService.Add(project);
            return Ok(project);
        }

        [HttpPost]
        [Route("projects")]
        [Authorize(Roles = "Admin")]
        public ActionResult<News> UpdateProject([FromBody] Project project) {
            _projectService.Update(project);
            return Ok(project);
        }

        [HttpDelete]
        [Route("projects")]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteProject([FromBody] Project project) {
            _projectService.Delete(project.Id);
            return Ok();
        }

        
        [HttpGet]
        [Route("publications")]
        public ActionResult<byte[]> DownloadPublication(string publicationId) {
            if (publicationId==null || Int32.Parse(publicationId) == 0) {
                var publications = _publicationService.List();
                return Ok(publications);
            }
            var publication = _publicationService.Get(Int32.Parse(publicationId));
            if (publication?.File == null) return NotFound();
        
            return File(publication.File.Data, publication.File.MimeType, publication.File.Name);
        }

        [HttpPut]
        [Route("publications")]
        [Authorize(Roles = "Admin")]
        [RequestFormLimits(ValueLengthLimit = int.MaxValue, MultipartBodyLengthLimit = int.MaxValue)]
        [DisableRequestSizeLimit]
        [Consumes("multipart/form-data")]
        public ActionResult UploadApp() {
            var formFile = (FormFile) Request.Form.Files[0];
            var softwareInfo = Request.Form["publication"];
            var jObject = JObject.Parse(softwareInfo);
            var stream = formFile.OpenReadStream();
            byte[] data;
            using (var memoryStream = new MemoryStream()) {
                stream.CopyTo(memoryStream);
                data = memoryStream.ToArray();
            }

            var file = new Models.PublicationFile(formFile.FileName, formFile.ContentType, data);
            var publication =
                new Publication(jObject["title"].ToString(), jObject["authors"].ToString(),
                    jObject["journal"].ToString(), file);
            _publicationService.Add(publication);
            return Ok(publication);
        }

        [HttpPost]
        [Route("publications")]
        [Authorize(Roles = "Admin")]
        public ActionResult<News> UpdatePublication([FromBody] Publication publication) {
            _publicationService.Update(publication);
            return Ok(publication);
        }

        [HttpDelete]
        [Route("publications")]
        [Authorize(Roles = "Admin")]
        public ActionResult DeletePublication([FromBody] Publication publication) {
            _publicationService.Delete(publication.Id);
            return Ok();
        }
    }
}