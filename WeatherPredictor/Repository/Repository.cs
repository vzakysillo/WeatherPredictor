using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using WeatherPredictor.Context;

namespace WeatherPredictor.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly MainContext _context;

        public Repository(MainContext context)
        {
            _context = context;
        }

        protected DbSet<T> Entities => _context.Set<T>();

        public virtual IEnumerable<T> GetAll() => Entities.ToList();

        public virtual T? Get(int id) => Entities.Find(id);

        public virtual void Create(T item)
        {
            if (!Exists(item))
            {
                Entities.Add(item);
                _context.SaveChanges();
            }
        }

        public virtual void Update(T item)
        {
            Entities.Update(item);
            _context.SaveChanges();
        }

        public virtual void Delete(int id)
        {
            var item = Entities.Find(id);
            if (item != null)
            {
                Entities.Remove(item);
                _context.SaveChanges();
            }
        }

        public virtual void Delete(string hash)
        {
            var entity = Entities.Find(hash);
            if (entity != null)
            {
                Entities.Remove(entity);
                _context.SaveChanges();
            }
        }

        protected virtual bool Exists(T item) => false;

        public virtual IEnumerable<T> Find(Expression<Func<T, bool>> predicate)
        {
            return Entities.Where(predicate);
        }

    }
}

