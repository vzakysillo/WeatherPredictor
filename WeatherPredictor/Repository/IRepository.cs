using System.Linq.Expressions;

namespace WeatherPredictor.Repository
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
        T? Get(int id);
        void Create(T item);
        void Update(T item);
        void Delete(int id);
        void Delete(string hash);

        IEnumerable<T> Find(Expression<Func<T, bool>> predicate);
    }

}
