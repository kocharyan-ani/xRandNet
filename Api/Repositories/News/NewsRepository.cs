using System.Linq;
using Api.Database.Context;
using Api.Database.Models;
using NewsDbEntity = Api.Database.Models.News;

namespace Api.Database.Repositories {
    public sealed class NewsRepository : Repository<News>, INewsRepository {
        public NewsRepository(DatabaseContext context) : base(context) {
        }

        public News Get() {
            return Context.News.FirstOrDefault();
        }
    }
}