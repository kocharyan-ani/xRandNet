using System.Collections.Generic;
using System.Linq;
using Api.Database.Repositories;
using Api.Models;
using Api.Models.Factories;
using AppModel = Api.Models.App;

namespace Api.Services {
    public sealed class AppService : IAppService {
        private readonly IAppRepository _appRepository;
        private readonly IAppFileRepository _appFileRepository;

        public AppService(IAppRepository appRepository, IAppFileRepository appFileRepository) {
            _appRepository = appRepository;
            _appFileRepository = appFileRepository;
        }

        public void Add(App model) {
            var entity = AppFactory.Create(model);

            _appFileRepository.Add(entity.File);
            model.File.Id = entity.Id;

            _appRepository.Add(entity);
            model.Id = entity.Id;
        }

        public void Delete(int id) {
            _appRepository.Delete(id);
        }

        public App Get(int id) {
            var entity = _appRepository.Get(id);

            return AppFactory.Create(entity);
        }

        public App Get(string version) {
            var entity = _appRepository.Get(version);
            entity.File = _appFileRepository.Get(entity.FileId);
            return AppFactory.Create(entity);
        }

        public IEnumerable<App> List() {
            return _appRepository.List().Select(AppFactory.Create);
        }

        public void Update(App model) {
            var entity = AppFactory.Create(model);

            _appRepository.Update(entity);
        }
    }
}