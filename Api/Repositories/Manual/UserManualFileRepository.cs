using Api.Database.Context;
using Api.Database.Models;
using UserManualFileDbEntity = Api.Database.Models.ManualFile;

namespace Api.Database.Repositories {
    public sealed class UserManualFileRepository : Repository<ManualFile>, IUserManualFileRepository {
        public UserManualFileRepository(DatabaseContext context) : base(context) {
        }
    }
}