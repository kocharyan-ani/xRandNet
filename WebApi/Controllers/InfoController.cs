using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models;
using WebApi.Services;

namespace WebApi.Controllers {
    [Route("api/data")]
    [ApiController]
    public class InfoController : Controller {
        public InfoService InfoService;
        public LinkService LinkService;
        public NewsService NewsService;

        public InfoController(InfoService infoService, LinkService linkService, NewsService newsService) {
            InfoService = infoService;
            LinkService = linkService;
            NewsService = newsService;
        }

        [HttpGet]
        [Route("info")]
        public ActionResult<string> GetInfoAboutUs() {
            var info = InfoService.Get();
            if (info == null) {
                return NotFound();
            }

            return Ok(new {content = info});
        }

        [HttpPost]
        // [Authorize(Roles = "Admin")]
        [Route("info")]
        public ActionResult<string> EditInfoAboutUs([FromBody] Info infoAboutUs) {
            InfoService.Update(infoAboutUs);
            return Ok();
        }

        [HttpGet]
        [Route("links")]
        public ActionResult<string> GetLinksByType([FromQuery] int type) {
            var links = LinkService.List().Where(link => link.Type == type);
            return Ok(links);
        }

        [HttpDelete]
        [Route("links")]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteLinks([FromQuery] int id) {
            if (id == 0) {
                return BadRequest("Link id should be specified");
            }

            LinkService.Delete(id);
            return Ok();
        }

        [HttpPut]
        [Route("links")]
        [Authorize(Roles = "Admin")]
        public ActionResult<string> AddLink([FromBody] Link link) {
            LinkService.Add(link);
            return Ok(link);
        }

        [HttpPost]
        [Route("links")]
        [Authorize(Roles = "Admin")]
        public ActionResult<string> EditLink([FromBody] Link link) {
            LinkService.Update(link);
            return Ok(link);
        }

        [HttpGet]
        [Route("news")]
        public ActionResult<string> Get() {
            var news = NewsService.List();
            return Ok(news);
        }

        [HttpPut]
        [Route("news")]
        // [Authorize(Roles = "Admin")]
        public ActionResult<News> AddNews([FromBody] News news) {
            NewsService.Add(news);
            return Ok(news);
        }

        [HttpPost]
        [Route("news")]
        // [Authorize(Roles = "Admin")]
        public ActionResult<News> UpdateNews([FromBody] News news) {
            NewsService.Update(news);
            return Ok(news);
        }

        [HttpDelete]
        [Route("news")]
        // [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id) {
            NewsService.Delete(id);
            return Ok();
        }
    }
}