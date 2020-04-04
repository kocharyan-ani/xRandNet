using AppDbEntity = Api.Database.Models.App;

namespace Api.Database.Repositories {
    public interface IAppRepository : IRepository<AppDbEntity> {
        AppDbEntity Get(string version);

    }
}