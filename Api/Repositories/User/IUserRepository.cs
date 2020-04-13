using UserDbEntity = Api.Database.Models.User;

namespace Api.Database.Repositories {
    public interface IUserRepository : IRepository<UserDbEntity> {
        UserDbEntity Get(string username, string password);
        UserDbEntity Get(string username);
    }
}