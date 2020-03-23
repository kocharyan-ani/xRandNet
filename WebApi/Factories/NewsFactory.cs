using NewsDbEntity = WebApi.Database.Models.News;
using NewsModel = WebApi.Models.News;

namespace WebApi.Models.Factories {
    public static class NewsFactory {
        public static NewsDbEntity Create(NewsModel model) {
            if (model == default) return default;
            return new NewsDbEntity(model.Id, model.Title, model.Content, model.DatePosted);
        }

        public static NewsModel Create(NewsDbEntity entity) {
            if (entity == default) return default;
            return new NewsModel(entity.Id, entity.Title, entity.Content, entity.DatePosted);
        }
    }
}