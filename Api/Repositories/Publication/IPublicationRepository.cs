using PublicationDbEntity = Api.Database.Models.Publication;

namespace Api.Database.Repositories {
    public interface IPublicationRepository  : IRepository<PublicationDbEntity> {
        
    }
}