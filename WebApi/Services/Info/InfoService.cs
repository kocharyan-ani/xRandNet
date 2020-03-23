using System.Collections.Generic;
using System.Linq;
using WebApi.Database.Repositories;
using WebApi.Models.Factories;
using InfoModel = WebApi.Models.Info;

namespace WebApi.Services {
    public sealed class InfoService : IInfoService {
        private readonly IInfoRepository _infoRepository;

        public InfoService(IInfoRepository infoRepository) {
            _infoRepository = infoRepository;
        }

        public void Add(InfoModel model) {
            var entity = InfoFactory.Create(model);

            _infoRepository.Add(entity);

            model.Id = entity.Id;
        }

        public void Delete(int id) {
            _infoRepository.Delete(id);
        }

        public InfoModel Get(int id) {
            var entity = _infoRepository.Get(id);

            return InfoFactory.Create(entity);
        }

        public InfoModel Get() {
            var entity = _infoRepository.Get();
            return entity == null ? null : InfoFactory.Create(entity);
        }

        public IEnumerable<InfoModel> List() {
            return _infoRepository.List().Select(InfoFactory.Create);
        }

        public void Update(InfoModel model) {
            var entity = InfoFactory.Create(model);

            _infoRepository.Update(entity);
        }
    }
}