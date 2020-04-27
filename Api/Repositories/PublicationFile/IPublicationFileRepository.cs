using PublicationFileDbEntity = Api.Database.Models.PublicationFile;

namespace Api.Database.Repositories {
    public interface IPublicationFileRepository : IRepository<PublicationFileDbEntity> {
    }
}