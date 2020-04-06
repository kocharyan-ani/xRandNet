using System.Collections.Generic;
using System.Linq;
using Api.Database.Repositories;
using Api.Models;
using Api.Models.Factories;
using UserModel = Api.Models.User;

namespace Api.Services {
    public sealed class UserService : IUserService {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository) {
            _userRepository = userRepository;
        }

        public void Add(User model) {
            var entity = UserFactory.Create(model);

            _userRepository.Add(entity);

            model.Id = entity.Id;
        }

        public void Delete(int id) {
            _userRepository.Delete(id);
        }

        public User Get(int id) {
            var entity = _userRepository.Get(id);

            return UserFactory.Create(entity);
        }

        public IEnumerable<User> List() {
            return _userRepository.List().Select(UserFactory.Create);
        }

        public void Update(User model) {
            var entity = UserFactory.Create(model);

            _userRepository.Update(entity);
        }

        public bool Exists(string username) {
            return _userRepository.Get(username) != null;
        }

        public User Get(string username, string password) {
            var entity = _userRepository.Get(username, password);
            return entity == null ? null : UserFactory.Create(entity);
        }
    }
}