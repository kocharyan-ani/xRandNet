using ProjectDbEntity = Api.Database.Models.Project;
using ProjectModel = Api.Models.Project;

namespace Api.Models.Factories {
    public static class ProjectFactory {
        public static ProjectDbEntity Create(ProjectModel model) {
            if (model == default) return default;
            return new ProjectDbEntity(model.Id, model.Name, model.Description);
        }

        public static ProjectModel Create(ProjectDbEntity entity) {
            if (entity == default) return default;
            return new ProjectModel(entity.Id, entity.Name, entity.Description);
        }
    }
}