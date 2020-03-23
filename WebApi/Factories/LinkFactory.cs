using LinkDbEntity = WebApi.Database.Models.Link;
using LinkModel = WebApi.Models.Link;

namespace WebApi.Models.Factories {
    public static class LinkFactory {
        public static LinkDbEntity Create(LinkModel model) {
            if (model == default) return default;
            return new LinkDbEntity(model.Id, model.Name, model.Url, model.Type);
        }

        public static LinkModel Create(LinkDbEntity entity) {
            if (entity == default) return default;
            return new LinkModel(entity.Id, entity.Name, entity.Url, entity.Type);
        }
    }
}