using System.Collections.Generic;
using System.Linq;
using Api.Database.Repositories;
using Api.Models;
using Api.Models.Factories;
using NewsModel = Api.Models.News;

namespace Api.Services {
    public sealed class NewsService : INewsService {
        private readonly INewsRepository _newsRepository;

        public NewsService(INewsRepository newsRepository) {
            _newsRepository = newsRepository;
        }

        public void Add(News model) {
            var entity = NewsFactory.Create(model);

            _newsRepository.Add(entity);

            model.Id = entity.Id;
        }

        public void Delete(int id) {
            _newsRepository.Delete(id);
        }

        public News Get(int id) {
            var entity = _newsRepository.Get(id);

            return NewsFactory.Create(entity);
        }

        public News Get() {
            var entity = _newsRepository.Get();
            return entity == null ? null : NewsFactory.Create(entity);
        }

        public IEnumerable<News> List() {
            return _newsRepository.List().Select(NewsFactory.Create);
        }

        public void Update(News model) {
            var entity = NewsFactory.Create(model);

            _newsRepository.Update(entity);
        }
    }
}