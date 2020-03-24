using System.Collections.Generic;
using Api.Models;
using InfoModel = Api.Models.Info;

namespace Api.Services {
    public interface IInfoService {
        void Add(Info model);

        void Delete(int id);

        Info Get(int id);

        IEnumerable<Info> List();

        void Update(Info model);
    }
}