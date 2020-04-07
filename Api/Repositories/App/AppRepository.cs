using System.Linq;
using Api.Database.Context;
using Api.Database.Models;
using AppDbEntity = Api.Database.Models.App;

namespace Api.Database.Repositories {
    public sealed class AppRepository : Repository<App>, IAppRepository {
        public AppRepository(DatabaseContext context) : base(context) {
        }

        public App Get(string version) {
            return Context.App.FirstOrDefault(app => app.Version == version);
        }
    }
}