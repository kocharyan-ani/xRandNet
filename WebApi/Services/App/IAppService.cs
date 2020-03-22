using System.Collections.Generic;
using AppModel = WebApi.Models.App;

namespace WebApi.Services {
    public interface IAppService {
        void Add(AppModel model);

        void Delete(int id);

        AppModel Get(int id);

        IEnumerable<AppModel> List();

        void Update(AppModel model);
    }
}