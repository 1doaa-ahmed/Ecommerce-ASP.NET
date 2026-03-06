using Microsoft.EntityFrameworkCore;
using Project_E_commerse.Data;
using Project_E_commerse.Repositories.Interfaces;
using System.Linq.Expressions;

namespace Project_E_commerse.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly ApplicationDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public Repository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
            => await _dbSet.ToListAsync();

        public async Task<T?> GetByIdAsync(int id)
            => await _dbSet.FindAsync(id);

        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
            => await _dbSet.Where(predicate).ToListAsync();

        public async Task<T> AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<(T? entity, string msg)> AddAsync(T entity, Expression<Func<T, bool>> predicate)
        {
            bool exists = await _dbSet.AnyAsync(predicate);
            if (exists)
                return (null, "Already exists!");

            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
            return (entity, "Added successfully!");
        }
        //public void Update(T entity)
        //    => _dbSet.Update(entity);

        public async Task<int> Update(T entity)
        {
            _dbSet.Update(entity);
            return await _context.SaveChangesAsync();
        }
        public async Task<(bool success, string msg)> DeleteAsync(Expression<Func<T, bool>> predicate)
        {
            var entity = await _dbSet.FirstOrDefaultAsync(predicate);
            if (entity is null)
                return (false, "Item not found!");

            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();

            return (true, "Deleted successfully!");
        }
        //public void Delete(T entity)
        //    => _dbSet.Remove(entity);

        public async Task SaveAsync()
            => await _context.SaveChangesAsync();

        public Task<int> Delete(T entity)
        {
            _dbSet.Remove(entity);
            return  _context.SaveChangesAsync();
        }
    }
}