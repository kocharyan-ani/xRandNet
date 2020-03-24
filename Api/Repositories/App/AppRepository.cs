using Api.Database.Context;
using Api.Database.Models;
using AppDbEntity = Api.Database.Models.App;

namespace Api.Database.Repositories {
    public sealed class AppRepository : Repository<App>, IAppRepository {
        public AppRepository(DatabaseContext context) : base(context) {
        }
    }
}