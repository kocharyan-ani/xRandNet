using LinkDbEntity = Api.Database.Models.Link;
using LinkModel = Api.Models.Link;

namespace Api.Models.Factories {
    public static class LinkFactory {
        public static Database.Models.Link Create(Link model) {
            if (model == default) return default;
            return new Database.Models.Link(model.Id, model.Name, model.Url, model.Type);
        }

        public static Link Create(Database.Models.Link entity) {
            if (entity == default) return default;
            return new Link(entity.Id, entity.Name, entity.Url, entity.Type);
        }
    }
}