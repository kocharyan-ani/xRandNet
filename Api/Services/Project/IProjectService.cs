using System.Collections.Generic;
using Api.Models;
using ProjectModel = Api.Models.Project;

namespace Api.Services {
    public interface IProjectService {
        void Add(Project model);

        void Delete(int id);

        Project Get(int id);

        IEnumerable<Project> List();

        void Update(Project model);
    }
}