using UserManualFileDbEntity = Api.Database.Models.ManualFile;

namespace Api.Database.Repositories {
    public interface IUserManualFileRepository : IRepository<UserManualFileDbEntity> {
    }
}