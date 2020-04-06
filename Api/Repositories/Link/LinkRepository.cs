using System.Collections.Generic;
using System.Linq;
using Api.Database.Context;
using Api.Database.Models;
using LinkDbEntity = Api.Database.Models.Link;

namespace Api.Database.Repositories {
    public sealed class LinkRepository : Repository<Link>, ILinkRepository {
        public LinkRepository(DatabaseContext context) : base(context) {
        }
    }
}