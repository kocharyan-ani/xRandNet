using System.Collections.Generic;
using System.Linq;
using Api.Database.Repositories;
using Api.Models.Factories;
using PersonModel = Api.Models.Person;

namespace Api.Services {
    public sealed class PersonService : IPersonService {
        private readonly IPersonRepository _personRepository;

        public PersonService(IPersonRepository personRepository) {
            _personRepository = personRepository;
        }

        public void Add(PersonModel model) {
            var entity = PeopleFactory.Create(model);

            _personRepository.Add(entity);

            model.Id = entity.Id;
        }

        public void Delete(int id) {
            _personRepository.Delete(id);
        }

        public PersonModel Get(int id) {
            var entity = _personRepository.Get(id);

            return PeopleFactory.Create(entity);
        }

        public IEnumerable<PersonModel> List() {
            return _personRepository.List().Select(PeopleFactory.Create);
        }

        public void Update(PersonModel model) {
            var entity = PeopleFactory.Create(model);

            _personRepository.Update(entity);
        }
    }
}