using System.Collections.Generic;
using NewsModel = WebApi.Models.News;

namespace WebApi.Services {
    public interface INewsService {
        void Add(NewsModel model);

        void Delete(int id);

        NewsModel Get(int id);

        IEnumerable<NewsModel> List();

        void Update(NewsModel model);
    }
}