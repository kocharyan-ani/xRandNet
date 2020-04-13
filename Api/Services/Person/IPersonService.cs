using System.Collections.Generic;
using Api.Models;
using PersonModel = Api.Models.Person;

namespace Api.Services {
    public interface IPersonService {
        void Add(Person model);

        void Delete(int id);

        Person Get(int id);

        IEnumerable<Person> List();

        void Update(Person model);
    }
}