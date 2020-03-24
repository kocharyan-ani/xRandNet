using System.Collections.Generic;
using System.Linq;
using Api.Database.Context;
using Api.Database.Models;

namespace Api.Database.Repositories {
    public abstract class Repository<T> : IRepository<T> where T : class {
        protected readonly DatabaseContext Context;

        protected Repository(DatabaseContext context) {
            Context = context;
        }

        public void Add(T item) {
            Context.Set<T>().Add(item);
            Context.SaveChanges();
        }

        public void Delete(int id) {
            Context.Set<T>().Remove(Get(id));
            Context.SaveChanges();
        }

        public T Get(int id) {
            return Context.Set<T>().Find(id);
        }

        public IEnumerable<T> List() {
            return Context.Set<T>().ToList();
        }

        public void Update(T item) {
            Context.Set<T>().Update(item);
            Context.SaveChanges();
        }

        public void Update(int id, object item) {
            Context.Entry(Get(id)).CurrentValues.SetValues(item);
            Context.SaveChanges();
        }
    }
}