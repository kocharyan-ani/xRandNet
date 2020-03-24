using System.Collections.Generic;
using System.Linq;
using Api.Database.Repositories;
using Api.Models.Factories;
using BugModel = Api.Models.Bug;

namespace Api.Services.Bug {
    public sealed class BugService : IBugService {
        private readonly IBugRepository _bugRepository;

        public BugService(IBugRepository bugRepository) {
            _bugRepository = bugRepository;
        }

        public void Add(Models.Bug model) {
            var entity = BugFactory.Create(model);

            _bugRepository.Add(entity);

            model.Id = entity.Id;
        }

        public void Delete(int id) {
            _bugRepository.Delete(id);
        }

        public Models.Bug Get(int id) {
            var entity = _bugRepository.Get(id);

            return BugFactory.Create(entity);
        }

        public IEnumerable<Models.Bug> List() {
            return _bugRepository.List().Select(BugFactory.Create);
        }

        public void Update(Models.Bug model) {
            var entity = BugFactory.Create(model);

            _bugRepository.Update(entity);
        }
    }
}