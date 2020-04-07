using System.Collections.Generic;
using Api.Database.Models;
using LinkDbEntity = Api.Database.Models.Link;

namespace Api.Database.Repositories {
    public interface ILinkRepository : IRepository<Link> {
    }
}