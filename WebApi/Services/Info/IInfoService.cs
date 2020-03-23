using System.Collections.Generic;
using InfoModel = WebApi.Models.Info;

namespace WebApi.Services {
    public interface IInfoService {
        void Add(InfoModel model);

        void Delete(int id);

        InfoModel Get(int id);

        IEnumerable<InfoModel> List();

        void Update(InfoModel model);
    }
}