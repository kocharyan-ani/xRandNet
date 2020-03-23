using System.Collections.Generic;
using LinkModel = WebApi.Models.Link;

namespace WebApi.Services {
    public interface ILinkService {
        void Add(LinkModel model);

        void Delete(int id);

        LinkModel Get(int id);

        IEnumerable<LinkModel> List();

        void Update(LinkModel model);
    }
}