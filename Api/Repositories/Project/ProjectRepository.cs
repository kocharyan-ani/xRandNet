using Api.Database.Context;
using ProjectDbEntity = Api.Database.Models.Project;

namespace Api.Database.Repositories {
    public sealed class ProjectRepository : Repository<ProjectDbEntity>, IProjectRepository {
        public ProjectRepository(DatabaseContext context) : base(context) {
        }
    }
}