using UserDbEntity = Api.Database.Models.User;
using UserModel = Api.Models.User;

namespace Api.Models.Factories {
    public static class UserFactory {
        public static Database.Models.User Create(User model) {
            if (model == default) return default;
            return new Database.Models.User(model.Id, model.FirstName, model.LastName, model.Password, model.Username,
                model.IsAdmin);
        }

        public static User Create(Database.Models.User entity) {
            if (entity == default) return default;
            return new User(entity.FirstName, entity.LastName, entity.Username, entity.Password, entity.IsAdmin);
        }
    }
}