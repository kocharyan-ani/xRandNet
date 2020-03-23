using NewsDbEntity = WebApi.Database.Models.News;

namespace WebApi.Database.Repositories {
    public interface INewsRepository : IRepository<NewsDbEntity> {
        NewsDbEntity Get();
    }
}