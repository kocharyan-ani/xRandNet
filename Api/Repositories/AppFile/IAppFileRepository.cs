using AppFileDbEntity = Api.Database.Models.AppFile;

namespace Api.Database.Repositories {
    public interface IAppFileRepository : IRepository<AppFileDbEntity> {
    }
}