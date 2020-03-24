using Api.Database.Context;
using Api.Database.Models;
using BugDbEntity = Api.Database.Models.Bug;

namespace Api.Database.Repositories {
    public sealed class BugRepository : Repository<Bug>, IBugRepository {
        public BugRepository(DatabaseContext context) : base(context) {
        }
    }
}