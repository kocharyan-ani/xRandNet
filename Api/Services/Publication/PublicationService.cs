using System.Collections.Generic;
using System.Linq;
using Api.Database.Repositories;
using Api.Models.Factories;
using PublicationModel = Api.Models.Publication;

namespace Api.Services {
    public sealed class PublicationService : IPublicationService {
        private readonly IPublicationRepository _personRepository;

        public PublicationService(IPublicationRepository personRepository) {
            _personRepository = personRepository;
        }

        public void Add(PublicationModel model) {
            var entity = PublicationFactory.Create(model);

            _personRepository.Add(entity);

            model.Id = entity.Id;
        }

        public void Delete(int id) {
            _personRepository.Delete(id);
        }

        public PublicationModel Get(int id) {
            var entity = _personRepository.Get(id);

            return PublicationFactory.Create(entity);
        }

        public IEnumerable<PublicationModel> List() {
            return _personRepository.List().Select(PublicationFactory.Create);
        }

        public void Update(PublicationModel model) {
            var entity = PublicationFactory.Create(model);

            _personRepository.Update(entity);
        }
    }
}