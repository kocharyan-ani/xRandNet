using Api.Database.Models;
using InfoDbEntity = Api.Database.Models.Info;

namespace Api.Database.Repositories {
    public interface IInfoRepository : IRepository<Info> {
        Info Get();
    }
}