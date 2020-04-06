using System.Linq;
using Api.Database.Context;
using Api.Database.Models;
using InfoDbEntity = Api.Database.Models.Info;

namespace Api.Database.Repositories {
    public sealed class InfoRepository : Repository<Info>, IInfoRepository {
        public InfoRepository(DatabaseContext context) : base(context) {
        }

        public Info Get() {
            return Context.Info.FirstOrDefault();
        }
    }
}