using InfoDbEntity = WebApi.Database.Models.Info;
using InfoModel = WebApi.Models.Info;

namespace WebApi.Models.Factories {
    public static class InfoFactory {
        public static InfoDbEntity Create(InfoModel model) {
            if (model == default) return default;
            return new InfoDbEntity(model.Id, model.Content);
        }

        public static InfoModel Create(InfoDbEntity entity) {
            if (entity == default) return default;
            return new InfoModel(entity.Id,entity.Content);
        }
    }
}