using System.Linq;
using WebApi.Database.Context;
using NewsDbEntity = WebApi.Database.Models.News;

namespace WebApi.Database.Repositories {
    public sealed class NewsRepository : Repository<NewsDbEntity>, INewsRepository {
        public NewsRepository(DatabaseContext context) : base(context) {
        }

        public NewsDbEntity Get() {
            return Context.News.FirstOrDefault();
        }
    }
}