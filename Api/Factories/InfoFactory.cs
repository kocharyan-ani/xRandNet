using InfoDbEntity = Api.Database.Models.Info;
using InfoModel = Api.Models.Info;

namespace Api.Models.Factories {
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