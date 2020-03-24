using InfoDbEntity = Api.Database.Models.Info;
using InfoModel = Api.Models.Info;

namespace Api.Models.Factories {
    public static class InfoFactory {
        public static Database.Models.Info Create(Info model) {
            if (model == default) return default;
            return new Database.Models.Info(model.Id, model.Content);
        }

        public static Info Create(Database.Models.Info entity) {
            if (entity == default) return default;
            return new Info(entity.Id,entity.Content);
        }
    }
}