using PersonDbEntity = Api.Database.Models.Person;

namespace Api.Database.Repositories {
    public interface IPersonRepository  : IRepository<PersonDbEntity> {
        
    }
}