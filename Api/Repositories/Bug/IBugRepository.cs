using System.Collections.Generic;
using BugDbEntity = Api.Database.Models.Bug;

namespace Api.Database.Repositories {
    public interface IBugRepository : IRepository<BugDbEntity> {
        public IEnumerable<BugDbEntity> List(int appId);
    }
}