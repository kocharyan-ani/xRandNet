using System.Collections.Generic;
using Api.Models;
using AppModel = Api.Models.App;

namespace Api.Services {
    public interface IAppService {
        void Add(App model);

        void Delete(int id);

        App Get(int id);

        IEnumerable<App> List();

        void Update(App model);
    }
}