using System.Collections.Generic;
using Api.Models;
using PublicationModel = Api.Models.Publication;

namespace Api.Services {
    public interface IPublicationService {
        void Add(Publication model);

        void Delete(int id);

        Publication Get(int id);

        IEnumerable<Publication> List();

        void Update(Publication model);
    }
}