using Api.Model;
using Session;
using System;
using System.Web.Http;
using System.Web.Http.Cors;

namespace Api.Controllers
{
    public class ResearchController : ApiController
    {
        [HttpPost]
        public String Index([System.Web.Http.FromBody] Research research)
        {
            WebSessionManager manager = new WebSessionManager();
            manager.CreateResearch(research.research);
            manager.SetResearchName(research.name);
            manager.SetResearchModelType(research.model);
            manager.SetResearchGenerationType(research.generation);
            manager.SetResearchCheckConnected(research.connected);
            manager.SetResearchRealizationCount(research.count);

            return research.name;
        }
    }
}