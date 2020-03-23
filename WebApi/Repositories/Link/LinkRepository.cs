using System.Linq;
using WebApi.Database.Context;
using LinkDbEntity = WebApi.Database.Models.Link;

namespace WebApi.Database.Repositories {
    public sealed class LinkRepository : Repository<LinkDbEntity>, ILinkRepository {
        public LinkRepository(DatabaseContext context) : base(context) {
        }

        public LinkDbEntity Get() {
            return Context.Link.FirstOrDefault();
        }
    }
}