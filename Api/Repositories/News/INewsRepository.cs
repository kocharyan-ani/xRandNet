using Api.Database.Models;
using NewsDbEntity = Api.Database.Models.News;

namespace Api.Database.Repositories {
    public interface INewsRepository : IRepository<News> {
        News Get();
    }
}