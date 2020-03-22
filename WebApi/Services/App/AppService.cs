using System.Collections.Generic;
using System.Linq;
using WebApi.Database.Repositories;
using WebApi.Models.Factories;
using AppModel = WebApi.Models.App;

namespace WebApi.Services {
    public sealed class AppService : IAppService {
        private readonly IAppRepository _appRepository;

        public AppService(IAppRepository appRepository) {
            _appRepository = appRepository;
        }

        public void Add(AppModel model) {
            var entity = AppFactory.Create(model);

            _appRepository.Add(entity);

            model.Id = entity.Id;
        }

        public void Delete(int id) {
            _appRepository.Delete(id);
        }

        public AppModel Get(int id) {
            var entity = _appRepository.Get(id);

            return AppFactory.Create(entity);
        }

        public IEnumerable<AppModel> List() {
            return _appRepository.List().Select(AppFactory.Create);
        }

        public void Update(AppModel model) {
            var entity = AppFactory.Create(model);

            _appRepository.Update(entity);
        }
    }
}