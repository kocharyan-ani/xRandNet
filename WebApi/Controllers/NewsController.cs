using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApi.Database;

namespace WebApi.Controllers
{
    [Route("api/news")]
    [ApiController]
    public class NewsController : Controller
    {
        public DbManager DbManager { get; }

        public NewsController(DbManager dbManager)
        {
            DbManager = dbManager;
        }

        [HttpGet]
        public ActionResult<string> Get()
        {
            var news = DbManager.GetAnnouncements();
            if (news == null)
            {
                return NotFound();
            }

            return Ok(news);
        }

        [HttpPost]
        public void Post([FromBody]string value)
        {

        }

        // DELETE api/news/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {

        }
    }
}
