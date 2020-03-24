using System.Collections.Generic;

namespace Api.Database.Repositories {
    public interface IRepository<T> {
        void Add(T item);

        void Delete(int id);

        T Get(int id);

        IEnumerable<T> List();

        void Update(T item);

        void Update(int id, object item);
    }
}