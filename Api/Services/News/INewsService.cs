using System.Collections.Generic;
using Api.Models;
using NewsModel = Api.Models.News;

namespace Api.Services {
    public interface INewsService {
        void Add(News model);

        void Delete(int id);

        News Get(int id);

        IEnumerable<News> List();

        void Update(News model);
    }
}