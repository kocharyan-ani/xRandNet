using System.Collections.Generic;
using BugModel = WebApi.Models.Bug;

namespace WebApi.Services.Bug {
    public interface IBugService {
        void Add(BugModel model);

        void Delete(int id);

        BugModel Get(int id);

        IEnumerable<BugModel> List();

        void Update(BugModel model);
    }
}