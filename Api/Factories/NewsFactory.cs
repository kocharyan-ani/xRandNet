using NewsDbEntity = Api.Database.Models.News;
using NewsModel = Api.Models.News;

namespace Api.Models.Factories {
    public static class NewsFactory {
        public static Database.Models.News Create(News model) {
            if (model == default) return default;
            return new Database.Models.News(model.Id, model.Title, model.Content, model.DatePosted);
        }

        public static News Create(Database.Models.News entity) {
            if (entity == default) return default;
            return new News(entity.Id, entity.Title, entity.Content, entity.DatePosted);
        }
    }
}