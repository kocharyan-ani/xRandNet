using System.Linq;
using WebApi.Database.Context;
using UserDbEntity = WebApi.Database.Models.User;

namespace WebApi.Database.Repositories {
    public sealed class UserRepository : Repository<UserDbEntity>, IUserRepository {
        public UserRepository(DatabaseContext context) : base(context) {
        }

        public UserDbEntity Get(string username) {
            return Context.User.FirstOrDefault(u => u.Username == username);
        }

        public UserDbEntity Get(string username, string password) {
            return Context.User.FirstOrDefault(u => u.Username == username && u.Password == password);
        }
    }
}