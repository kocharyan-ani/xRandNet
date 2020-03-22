using WebApi.Database.Context;
using UserManualFileDbEntity = WebApi.Database.Models.ManualFile;

namespace WebApi.Database.Repositories {
    public sealed class UserManualFileRepository : Repository<UserManualFileDbEntity>, IUserManualFileRepository {
        public UserManualFileRepository(DatabaseContext context) : base(context) {
        }
    }
}