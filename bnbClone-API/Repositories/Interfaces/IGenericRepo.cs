using bnbClone_API.Models;
using System.Linq.Expressions;

namespace bnbClone_API.Repositories.Interfaces
{
    public interface IGenericRepo<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);
        Task<T> AddAsync(T entity);
        Task<T> UpdateAsync(T entity);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        // Optional Additions
        Task<IEnumerable<T>> FindInDataAsync(Expression<Func<T, bool>> predicate);
        Task<IEnumerable<T>> GetPagedAsync(
            Expression<Func<T, bool>>? filter,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy,
            int pageNumber,
            int pageSize);
        Task<bool> FindAnyConAsync(Expression<Func<Booking, bool>> predicate);

        //This method is designed to find a single Entity record from the database
        //that matches a given condition (predicate). It returns the first match, or null if nothing matches.
        Task<T?> FindFirstAsync(Expression<Func<T, bool>> predicate);

    }
}
