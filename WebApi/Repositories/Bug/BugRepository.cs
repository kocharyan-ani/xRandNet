using WebApi.Database.Context;
using BugDbEntity = WebApi.Database.Models.Bug;

namespace WebApi.Database.Repositories {
    public sealed class BugRepository : Repository<BugDbEntity>, IBugRepository {
        public BugRepository(xrandnetContext context) : base(context) {
        }
    }
}