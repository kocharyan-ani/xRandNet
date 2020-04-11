using PersonDbEntity = Api.Database.Models.Person;

namespace Api.Database.Repositories.Person {
    public interface IPersonRepository  : IRepository<PersonDbEntity> {
        
    }
}