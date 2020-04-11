using Api.Database.Context;
using Api.Database.Repositories.Person;
using PersonDbEntity = Api.Database.Models.Person;

namespace Api.Database.Repositories {
    public sealed class PersonRepository : Repository<PersonDbEntity>, IPersonRepository {
        public PersonRepository(DatabaseContext context) : base(context) {
        }
    }
}