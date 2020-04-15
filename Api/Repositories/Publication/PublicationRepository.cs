using Api.Database.Context;
using PublicationDbEntity = Api.Database.Models.Publication;

namespace Api.Database.Repositories {
    public sealed class PublicationRepository : Repository<PublicationDbEntity>, IPublicationRepository {
        public PublicationRepository(DatabaseContext context) : base(context) {
        }
    }
}