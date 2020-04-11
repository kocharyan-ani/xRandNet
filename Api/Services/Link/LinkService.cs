using System.Collections.Generic;
using System.Linq;
using Api.Database.Repositories;
using Api.Models;
using Api.Models.Factories;
using LinkModel = Api.Models.Link;

namespace Api.Services {
    public sealed class LinkService : ILinkService {
        private readonly ILinkRepository _linkRepository;

        public LinkService(ILinkRepository linkRepository) {
            _linkRepository = linkRepository;
        }

        public void Add(Link model) {
            var entity = LinkFactory.Create(model);

            _linkRepository.Add(entity);

            model.Id = entity.Id;
        }

        public void Delete(int id) {
            _linkRepository.Delete(id);
        }

        public Link Get(int id) {
            var entity = _linkRepository.Get(id);

            return LinkFactory.Create(entity);
        }

        public IEnumerable<Link> List() {
            return _linkRepository.List().Select(LinkFactory.Create);
        }

        public void Update(Link model) {
            var entity = LinkFactory.Create(model);

            _linkRepository.Update(entity);
        }
    }
}