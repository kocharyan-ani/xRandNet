using PersonDbEntity = Api.Database.Models.Person;
using PersonModel = Api.Models.Person;

namespace Api.Models.Factories {
    public static class PeopleFactory {
        public static PersonDbEntity Create(PersonModel model) {
            if (model == default) return default;
            return new PersonDbEntity(model.Id, model.FirstName, model.LastName, model.Description, model.LinkedInUrl,
                model.FacebookUrl);
        }

        public static PersonModel Create(PersonDbEntity entity) {
            if (entity == default) return default;
            return new PersonModel(entity.Id, entity.FirstName, entity.LastName, entity.Description, entity.LinkedInUrl,
                entity.FacebookUrl);
        }
    }
}