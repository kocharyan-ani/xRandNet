using System.Collections.Generic;
using System.Linq;
using WebApi.Database.Repositories;
using WebApi.Models.Factories;
using LinkModel = WebApi.Models.Link;

namespace WebApi.Services {
    public sealed class LinkService : ILinkService {
        private readonly ILinkRepository _linkRepository;

        public LinkService(ILinkRepository linkRepository) {
            _linkRepository = linkRepository;
        }

        public void Add(LinkModel model) {
            var entity = LinkFactory.Create(model);

            _linkRepository.Add(entity);

            model.Id = entity.Id;
        }

        public void Delete(int id) {
            _linkRepository.Delete(id);
        }

        public LinkModel Get(int id) {
            var entity = _linkRepository.Get(id);

            return LinkFactory.Create(entity);
        }

        public LinkModel Get() {
            var entity = _linkRepository.Get();
            return entity == null ? null : LinkFactory.Create(entity);
        }

        public IEnumerable<LinkModel> List() {
            return _linkRepository.List().Select(LinkFactory.Create);
        }

        public void Update(LinkModel model) {
            var entity = LinkFactory.Create(model);

            _linkRepository.Update(entity);
        }
    }
}