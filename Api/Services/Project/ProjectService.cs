using System.Collections.Generic;
using System.Linq;
using Api.Database.Repositories;
using Api.Models.Factories;
using ProjectModel = Api.Models.Project;

namespace Api.Services {
    public sealed class ProjectService : IProjectService {
        private readonly IProjectRepository _projectRepository;

        public ProjectService(IProjectRepository projectRepository) {
            _projectRepository = projectRepository;
        }

        public void Add(ProjectModel model) {
            var entity = ProjectFactory.Create(model);

            _projectRepository.Add(entity);

            model.Id = entity.Id;
        }

        public void Delete(int id) {
            _projectRepository.Delete(id);
        }

        public ProjectModel Get(int id) {
            var entity = _projectRepository.Get(id);

            return ProjectFactory.Create(entity);
        }

        public IEnumerable<ProjectModel> List() {
            return _projectRepository.List().Select(ProjectFactory.Create);
        }

        public void Update(ProjectModel model) {
            var entity = ProjectFactory.Create(model);

            _projectRepository.Update(entity);
        }
    }
}