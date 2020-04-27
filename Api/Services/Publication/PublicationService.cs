using System.Collections.Generic;
using System.Linq;
using Api.Database.Repositories;
using Api.Models.Factories;
using PublicationModel = Api.Models.Publication;

namespace Api.Services {
    public sealed class PublicationService : IPublicationService {
        private readonly IPublicationRepository _publicationRepository;
        private readonly IPublicationFileRepository _publicationFileRepository;

        public PublicationService(IPublicationRepository publicationRepository,IPublicationFileRepository publicationFileRepository) {
            _publicationRepository = publicationRepository;
            _publicationFileRepository = publicationFileRepository;
        }

        public void Add(PublicationModel model) {
            var entity = PublicationFactory.Create(model);

            _publicationRepository.Add(entity);

            model.Id = entity.Id;
        }

        public void Delete(int id) {
            _publicationRepository.Delete(id);
        }

        public PublicationModel Get(int id) {
            var entity = _publicationRepository.Get(id);
            entity.File = _publicationFileRepository.Get(entity.FileId);
            return PublicationFactory.Create(entity);
        }

        public IEnumerable<PublicationModel> List() {
            return _publicationRepository.List().Select(PublicationFactory.Create);
        }

        public void Update(PublicationModel model) {
            var entity = _publicationRepository.Get(model.Id);
            entity.Authors = model.Authors;
            entity.Journal = model.Journal;
            entity.Title = model.Title;
            entity.File = _publicationFileRepository.Get(entity.FileId);
            _publicationRepository.Update(entity);
        }
    }
}