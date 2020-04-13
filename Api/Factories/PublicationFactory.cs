using PublicationDbEntity = Api.Database.Models.Publication;
using PublicationModel = Api.Models.Publication;

namespace Api.Models.Factories {
    public static class PublicationFactory {
        public static PublicationDbEntity Create(PublicationModel model) {
            if (model == default) return default;
            return new PublicationDbEntity(model.Id, model.Title, model.Authors, model.Journal,
                FileFactory.Create(model.File));
        }

        public static PublicationModel Create(PublicationDbEntity entity) {
            if (entity == default) return default;
            return new PublicationModel(entity.Id, entity.Title, entity.Authors, entity.Journal,
                FileFactory.Create(entity.File));
        }
    }
}