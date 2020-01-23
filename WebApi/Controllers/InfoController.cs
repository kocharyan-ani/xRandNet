using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;
using WebApi.Database;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Route("api/data")]
    [ApiController]
    public class InfoController : Controller
    {
        public DbManager DbManager { get; }

        public InfoController(DbManager dbManager)
        {
            DbManager = dbManager;
        }

        [HttpGet]
        [Route("aboutUs")]
        public ActionResult<string> GetInfoAboutUs()
        {
            var info = DbManager.GetAboutUsInfo();
            if (info == null)
            {
                return NotFound();
            }

            return Ok(new
            {
                content = info
            });
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Route("aboutUs")]
        public ActionResult<string> EditInfoAboutUs([FromBody] InfoAboutUs infoAboutUs)
        {
            DbManager.UpdateAboutUsInfo(infoAboutUs);
            return Ok();
        }

        [HttpGet]
        [Route("links")]
        public ActionResult<string> GetLinksByType([FromQuery] int type)
        {
            var links = DbManager.GetLinks(type);
            if (links == null)
            {
                return NotFound();
            }

            return Ok(links);
        }

        [HttpDelete]
        [Route("links")]
        [Authorize(Roles = "Admin")]
        public ActionResult<string> DeleteLinks([FromBody] Link link)
        {
            DbManager.DeleteLink(link);
            return Ok();
        }

        [HttpPut]
        [Route("links")]
        [Authorize(Roles = "Admin")]
        public ActionResult<string> AddLink([FromBody] Link link)
        {
            var linkAdded = DbManager.AddLink(link);
            if (linkAdded == null)
            {
                return BadRequest("Something gone wrong. Link was not added");
            }

            return Ok(link);
        }
    }
}