using System.Collections.Generic;
using Api.Models;
using UserModel = Api.Models.User;

namespace Api.Services {
    public interface IUserService {
        void Add(User model);

        void Delete(int id);

        User Get(int id);

        IEnumerable<User> List();

        void Update(User model);
    }
}