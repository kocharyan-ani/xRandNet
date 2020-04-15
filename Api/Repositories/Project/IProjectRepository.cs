using ProjectDbEntity = Api.Database.Models.Project;

namespace Api.Database.Repositories {
    public interface IProjectRepository  : IRepository<ProjectDbEntity> {
        
    }
}