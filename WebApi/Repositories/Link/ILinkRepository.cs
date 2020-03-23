using LinkDbEntity = WebApi.Database.Models.Link;

namespace WebApi.Database.Repositories {
    public interface ILinkRepository : IRepository<LinkDbEntity> {
        LinkDbEntity Get();
    }
}