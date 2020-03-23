using UserDbEntity = WebApi.Database.Models.User;
using UserModel = WebApi.Models.User;

namespace WebApi.Models.Factories {
    public static class UserFactory {
        public static UserDbEntity Create(UserModel model) {
            if (model == default) return default;
            return new UserDbEntity(model.Id, model.FirstName, model.LastName, model.Password, model.Username,
                model.IsAdmin);
        }

        public static UserModel Create(UserDbEntity entity) {
            if (entity == default) return default;
            return new UserModel(entity.FirstName, entity.LastName, entity.Username, entity.Password, entity.IsAdmin);
        }
    }
}