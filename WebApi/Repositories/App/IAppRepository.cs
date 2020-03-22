using AppDbEntity = WebApi.Database.Models.App;

namespace WebApi.Database.Repositories {
    public interface IAppRepository : IRepository<AppDbEntity> {
    }
}