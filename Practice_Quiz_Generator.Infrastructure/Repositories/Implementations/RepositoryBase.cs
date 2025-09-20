using Microsoft.EntityFrameworkCore;
using Practice_Quiz_Generator.Infrastructure.DatabaseContext;
using Practice_Quiz_Generator.Infrastructure.Repositories.Interfaces;
using System.Linq.Expressions;

namespace Practice_Quiz_Generator.Infrastructure.Repositories.Implementations
{
    public class RepositoryBase<T>(ExamPortalContext _context) : IRepositoryBase<T> where T : class
    {

        public async Task CreateAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
        }

        public async Task CreateManyAsync(IEnumerable<T> entity)
        {
            await _context.Set<T>().AddRangeAsync(entity);
        }

        public void Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
        }

        public void DeleteRange(IEnumerable<T> entities)
        {
            _context.Set<T>().RemoveRange(entities);
        }

        public IQueryable<T> FindAll(bool trackChanges)
        {
            return trackChanges ? _context.Set<T>() : _context.Set<T>().AsNoTracking();
        }

        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> condition, bool trackChanges)
        {
            return trackChanges ? _context.Set<T>().Where(condition) : _context.Set<T>().Where(condition).AsNoTracking();
        }
        public void Update(T entity)
        {
            _context.Set<T>().Update(entity);
        }

        public async Task SaveChangesAync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
