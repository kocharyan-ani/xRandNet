using System.Collections.Generic;
using System.Linq;
using Api.Database.Repositories;
using Api.Models.Factories;
using BugModel = Api.Models.Bug;

namespace Api.Services.Bug {
    public sealed class BugService : IBugService {
        private readonly IBugRepository _bugRepository;
        private readonly IAppRepository _appRepository;

        public BugService(IBugRepository bugRepository, IAppRepository appRepository) {
            _bugRepository = bugRepository;
            _appRepository = appRepository;
        }

        public void Add(BugModel model) {
            var appDbEntity = _appRepository.Get(model.Version);
            model.App = AppFactory.Create(appDbEntity);

            var entity = BugFactory.Create(model);
            entity.App = appDbEntity;

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

        public IEnumerable<BugModel> List() {
            var bugs = new List<BugModel>();
            foreach (var bugDbEntity in _bugRepository.List()) {
                var appDbEntity = _appRepository.Get(bugDbEntity.AppId);
                bugDbEntity.App = appDbEntity;
                bugs.Add(BugFactory.Create(bugDbEntity));
            }

            return bugs;
        }

        public IEnumerable<BugModel> List(string version) {
            var bugs = new List<BugModel>();
            var appDbEntity = _appRepository.Get(version);
            foreach (var bugDbEntity in _bugRepository.List(appDbEntity.Id)) {
                bugDbEntity.App = appDbEntity;
                bugs.Add(BugFactory.Create(bugDbEntity));
            }

            return bugs;
        }

        public void Update(Models.Bug model) {
            var entity = BugFactory.Create(model);

            _bugRepository.Update(entity);
        }
    }
}