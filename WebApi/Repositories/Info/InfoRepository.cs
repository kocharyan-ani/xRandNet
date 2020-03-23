using System.Linq;
using WebApi.Database.Context;
using InfoDbEntity = WebApi.Database.Models.Info;

namespace WebApi.Database.Repositories {
    public sealed class InfoRepository : Repository<InfoDbEntity>, IInfoRepository {
        public InfoRepository(DatabaseContext context) : base(context) {
        }

        public InfoDbEntity Get() {
            return Context.Info.FirstOrDefault();
        }
    }
}