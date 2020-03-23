using System.Collections.Generic;
using System.Linq;
using WebApi.Database.Repositories;
using WebApi.Models.Factories;
using WebApi.Services.User;
using UserModel = WebApi.Models.User;

namespace WebApi.Services.Bug {
    public sealed class UserService : IUserService {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository) {
            _userRepository = userRepository;
        }

        public void Add(UserModel model) {
            var entity = UserFactory.Create(model);

            _userRepository.Add(entity);

            model.Id = entity.Id;
        }

        public void Delete(int id) {
            _userRepository.Delete(id);
        }

        public UserModel Get(int id) {
            var entity = _userRepository.Get(id);

            return UserFactory.Create(entity);
        }

        public IEnumerable<UserModel> List() {
            return _userRepository.List().Select(UserFactory.Create);
        }

        public void Update(UserModel model) {
            var entity = UserFactory.Create(model);

            _userRepository.Update(entity);
        }

        public bool Exists(string username) {
            return _userRepository.Get(username) != null;
        }

        public UserModel Get(string username, string password) {
            var entity = _userRepository.Get(username, password);
            if (entity == null) {
                return null;
            }

            return UserFactory.Create(entity);
        }
    }
}