using System.Linq.Expressions;

namespace Practice_Quiz_Generator.Infrastructure.Repositories.Interfaces
{
    public interface IRepositoryBase<T>
    {
        IQueryable<T> FindAll(bool trackChanges);
        IQueryable<T> FindByCondition(Expression<Func<T, bool>> condition, bool trackChanges);
        void Update(T entity);
        void Delete(T entity);
        void DeleteRange(IEnumerable<T> entities);
        Task CreateAsync(T entity);
        Task CreateManyAsync(IEnumerable<T> entity);
        Task SaveChangesAync();
    }
}
