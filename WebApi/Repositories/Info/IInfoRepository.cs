using InfoDbEntity = WebApi.Database.Models.Info;

namespace WebApi.Database.Repositories {
    public interface IInfoRepository : IRepository<InfoDbEntity> {
        InfoDbEntity Get();
    }
}