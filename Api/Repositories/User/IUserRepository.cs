using Api.Database.Models;
using UserDbEntity = Api.Database.Models.User;

namespace Api.Database.Repositories {
    public interface IUserRepository : IRepository<User> {
        User Get(string username, string password);
        User Get(string username);
    }
}