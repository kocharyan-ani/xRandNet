using System.Collections.Generic;
using Api.Models;
using LinkModel = Api.Models.Link;

namespace Api.Services {
    public interface ILinkService {
        void Add(Link model);

        void Delete(int id);

        Link Get(int id);

        IEnumerable<Link> List();

        void Update(Link model);
    }
}