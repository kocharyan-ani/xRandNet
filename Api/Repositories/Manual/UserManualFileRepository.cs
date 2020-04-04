using Api.Database.Context;
using UserManualFileDbEntity = Api.Database.Models.ManualFile;

namespace Api.Database.Repositories {
    public sealed class UserManualFileRepository : Repository<UserManualFileDbEntity>, IUserManualFileRepository {
        public UserManualFileRepository(DatabaseContext context) : base(context) {
        }
    }
}