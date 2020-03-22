using System.Collections.Generic;
using System.Linq;
using WebApi.Database.Repositories;
using WebApi.Models.Factories;
using BugModel = WebApi.Models.Bug;

namespace WebApi.Services.Bug {
    public sealed class BugService : IBugService {
        private readonly IBugRepository _bugRepository;

        public BugService(IBugRepository bugRepository) {
            _bugRepository = bugRepository;
        }

        public void Add(BugModel model) {
            var entity = BugFactory.Create(model);

            _bugRepository.Add(entity);

            model.Id = entity.Id;
        }

        public void Delete(int id) {
            _bugRepository.Delete(id);
        }

        public BugModel Get(int id) {
            var entity = _bugRepository.Get(id);

            return BugFactory.Create(entity);
        }

        public IEnumerable<BugModel> List() {
            return _bugRepository.List().Select(BugFactory.Create);
        }

        public void Update(BugModel model) {
            var entity = BugFactory.Create(model);

            _bugRepository.Update(entity);
        }
    }
}