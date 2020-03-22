using BugDbEntity = WebApi.Database.Models.Bug;

namespace WebApi.Database.Repositories {
    public interface IBugRepository : IRepository<BugDbEntity> {
    }
}