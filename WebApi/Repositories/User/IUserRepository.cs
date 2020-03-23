using UserDbEntity = WebApi.Database.Models.User;

namespace WebApi.Database.Repositories {
    public interface IUserRepository : IRepository<UserDbEntity> {
        UserDbEntity Get(string username, string password);
        UserDbEntity Get(string username);
    }
}