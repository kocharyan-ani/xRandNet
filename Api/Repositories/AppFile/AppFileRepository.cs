using Api.Database.Context;
using AppFileDbEntity = Api.Database.Models.AppFile;

namespace Api.Database.Repositories {
    public sealed class AppFileRepository : Repository<AppFileDbEntity>, IAppFileRepository {
        public AppFileRepository(DatabaseContext context) : base(context) {
        }
    }
}