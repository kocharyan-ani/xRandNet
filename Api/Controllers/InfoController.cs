using System;
using System.Linq;
using Api.Models;
using Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
                return NotFound();
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
        [Route("people")]
        [Authorize(Roles = "Admin")]
        public ActionResult DeletePerson([FromBody] Person person) {
            _personService.Delete(person.Id);
            return Ok();
        }
    }
}