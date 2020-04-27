using Api.Database.Context;
using PublicationFileDbEntity = Api.Database.Models.PublicationFile;

namespace Api.Database.Repositories {
    public sealed class PublicationFileRepository : Repository<PublicationFileDbEntity>, IPublicationFileRepository {
        public PublicationFileRepository(DatabaseContext context) : base(context) {
        }
    }
}