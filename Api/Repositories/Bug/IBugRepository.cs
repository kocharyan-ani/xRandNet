using Api.Database.Models;
using BugDbEntity = Api.Database.Models.Bug;

namespace Api.Database.Repositories {
    public interface IBugRepository : IRepository<Bug> {
    }
}