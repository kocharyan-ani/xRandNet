using System.Collections.Generic;
using System.Linq;
using WebApi.Database.Repositories;
using WebApi.Models.Factories;
using NewsModel = WebApi.Models.News;

namespace WebApi.Services {
    public sealed class NewsService : INewsService {
        private readonly INewsRepository _newsRepository;

        public NewsService(INewsRepository newsRepository) {
            _newsRepository = newsRepository;
        }

        public void Add(NewsModel model) {
            var entity = NewsFactory.Create(model);

            _newsRepository.Add(entity);

            model.Id = entity.Id;
        }

        public void Delete(int id) {
            _newsRepository.Delete(id);
        }

        public NewsModel Get(int id) {
            var entity = _newsRepository.Get(id);

            return NewsFactory.Create(entity);
        }

        public NewsModel Get() {
            var entity = _newsRepository.Get();
            return entity == null ? null : NewsFactory.Create(entity);
        }

        public IEnumerable<NewsModel> List() {
            return _newsRepository.List().Select(NewsFactory.Create);
        }

        public void Update(NewsModel model) {
            var entity = NewsFactory.Create(model);

            _newsRepository.Update(entity);
        }
    }
}