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
        [Route("aboutUs")]
        public ActionResult<string> GetInfoAboutUs() {
            var info = InfoService.Get();
            if (info == null) {
                return NotFound();
            }

            return Ok(new {content = info});
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Route("aboutUs")]
        public ActionResult<string> EditInfoAboutUs([FromBody] Info infoAboutUs) {
            InfoService.Update(infoAboutUs);
            return Ok();
        }

        [HttpGet]
        [Route("links")]
        public ActionResult<string> GetLinksByType([FromQuery] int type) {
            var links = DbManager.GetLinks(type);
            if (links == null) {
                return NotFound();
            }

            return Ok(links);
        }

        [HttpDelete]
        [Route("links")]
        [Authorize(Roles = "Admin")]
        public ActionResult<string> DeleteLinks([FromBody] Link link) {
            DbManager.DeleteLink(link);
            return Ok();
        }

        [HttpPut]
        [Route("links")]
        [Authorize(Roles = "Admin")]
        public ActionResult<string> AddLink([FromBody] Link link) {
            var linkAdded = DbManager.AddLink(link);
            if (linkAdded == null) {
                return BadRequest("Something gone wrong. Link was not added");
            }

            return Ok(link);
        }

        [HttpPost]
        [Route("links")]
        [Authorize(Roles = "Admin")]
        public ActionResult<string> EditLink([FromBody] Link link) {
            var linkAdded = DbManager.UpdateLink(link);
            if (linkAdded == null) {
                return BadRequest("Something gone wrong. Link was not updated");
            }

            return Ok(link);
        }

        [HttpGet]
        [Route("news")]
        public ActionResult<string> Get() {
            var news = DbManager.GetAnnouncements();
            if (news == null) {
                return NotFound();
            }

            return Ok(news);
        }

        [HttpPost]
        [Route("news")]
        [Authorize(Roles = "Admin")]
        public void Post([FromBody] string value) {
        }

        // DELETE api/news/5
        [HttpDelete("{id}")]
        [Route("news")]
        [Authorize(Roles = "Admin")]
        public void Delete(int id) {
        }
    }
}