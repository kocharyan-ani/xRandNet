using System.Linq;
using Api.Database.Context;
using Api.Database.Models;
using UserDbEntity = Api.Database.Models.User;

namespace Api.Database.Repositories {
    public sealed class UserRepository : Repository<User>, IUserRepository {
        public UserRepository(DatabaseContext context) : base(context) {
        }

        public User Get(string username) {
            return Context.Users.FirstOrDefault(u => u.Username == username);
        }

        public User Get(string username, string password) {
            return Context.Users.FirstOrDefault(u => u.Username == username && u.Password == password);
        }
    }
}