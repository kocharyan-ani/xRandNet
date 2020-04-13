using System.Collections.Generic;
using System.Linq;
using Api.Database.Context;
using Api.Database.Models;
using BugDbEntity = Api.Database.Models.Bug;

namespace Api.Database.Repositories {
    public sealed class BugRepository : Repository<Bug>, IBugRepository {
        public BugRepository(DatabaseContext context) : base(context) {
        }

        public IEnumerable<Bug> List(int appId) {
            return Context.Bugs.Where(bug => bug.AppId == appId);
        }
    }
}