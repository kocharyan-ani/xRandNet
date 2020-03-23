using System.Collections.Generic;
using UserModel = WebApi.Models.User;

namespace WebApi.Services.User {
    public interface IUserService {
        void Add(UserModel model);

        void Delete(int id);

        UserModel Get(int id);

        IEnumerable<UserModel> List();

        void Update(UserModel model);
    }
}