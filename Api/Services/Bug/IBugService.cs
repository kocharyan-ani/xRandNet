using System.Collections.Generic;
using BugModel = Api.Models.Bug;

namespace Api.Services.Bug {
    public interface IBugService {
        void Add(Models.Bug model);

        void Delete(int id);

        Models.Bug Get(int id);

        IEnumerable<Models.Bug> List();

        void Update(Models.Bug model);
    }
}