using System.Collections.Generic;
using System.Linq;
using Api.Database.Repositories;
using Api.Models;
using Api.Models.Factories;
using InfoModel = Api.Models.Info;
using InfoDBEntity = Api.Database.Models.Info;

namespace Api.Services {
    public sealed class InfoService : IInfoService {
        private readonly IInfoRepository _infoRepository;

        public InfoService(IInfoRepository infoRepository) {
            _infoRepository = infoRepository;
        }

        public void Add(Info model) {
            var entity = InfoFactory.Create(model);

            _infoRepository.Add(entity);

            model.Id = entity.Id;
        }

        public void Delete(int id) {
            _infoRepository.Delete(id);
        }

        public Info Get(int id) {
            var entity = _infoRepository.Get(id);

            return InfoFactory.Create(entity);
        }

        public Info Get() {
            var entity = _infoRepository.Get();
            return entity == null ? null : InfoFactory.Create(entity);
        }

        public IEnumerable<Info> List() {
            return _infoRepository.List().Select(InfoFactory.Create);
        }

        public void Update(Info model) {
            var infos = _infoRepository.List().ToList();
            if (!infos.Any()) {
                Add(model);
            }
            else {
                InfoDBEntity info = infos.FirstOrDefault();
                info.Content = model.Content;
                _infoRepository.Update(info);
            }
        }
    }
}