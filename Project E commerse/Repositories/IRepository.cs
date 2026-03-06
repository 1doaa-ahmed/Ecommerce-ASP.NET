using System.Linq.Expressions;

namespace Project_E_commerse.Repositories.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetByIdAsync(int id);
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
        //Task AddAsync(T entity);
        Task<T> AddAsync(T entity);
        Task<int>  Update(T entity);
        Task<int>  Delete(T entity);
        Task SaveAsync();
        Task<(bool success, string msg)> DeleteAsync(Expression<Func<T, bool>> predicate);
    }
}