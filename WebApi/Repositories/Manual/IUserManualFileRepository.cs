using UserManualFileDbEntity = WebApi.Database.Models.ManualFile;

namespace WebApi.Database.Repositories {
    public interface IUserManualFileRepository : IRepository<UserManualFileDbEntity> {
    }
}