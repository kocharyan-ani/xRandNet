using WebApi.Database.Context;
using AppDbEntity = WebApi.Database.Models.App;

namespace WebApi.Database.Repositories {
    public sealed class AppRepository : Repository<AppDbEntity>, IAppRepository {
        public AppRepository(xrandnetContext context) : base(context) {
        }
    }
}