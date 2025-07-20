using bnbClone_API.Data;
using bnbClone_API.Models;
using bnbClone_API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace bnbClone_API.Repositories.Impelementations
{
    public class GenericRepo<T> : IGenericRepo<T> where T : class
    {
        private readonly ApplicationDbContext dbContext;

        public  GenericRepo(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<T> AddAsync(T entity)
        {
            await dbContext.Set<T>().AddAsync(entity);
            return entity;
        }

        public virtual async Task<bool> DeleteAsync(int id)
        {
            var entity = await dbContext.Set<T>().FindAsync(id);
            if (entity == null)
                return false;

            // Check if entity has a status enum and perform soft delete
            var entityType = typeof(T);

            if (entityType.GetProperty("AccountStatus") != null)
            {
                entityType.GetProperty("AccountStatus")?.SetValue(entity, AccountStatus.Deleted);
            }
            else if (entityType.GetProperty("PropertyStatus") != null)
            {
                entityType.GetProperty("PropertyStatus")?.SetValue(entity, PropertyStatus.Suspended);
            }
            else if (entityType.GetProperty("BookingStatus") != null)
            {
                entityType.GetProperty("BookingStatus")?.SetValue(entity, BookingStatus.Cancelled);
            }
            else
            {
                // Optional: fallback if entity has no supported status
                throw new InvalidOperationException("Entity does not support soft delete via status enum.");
            }

            dbContext.Set<T>().Update(entity);
            return true;

        }

        public async Task<bool> ExistsAsync(int id)
        {
            var entity = await dbContext.Set<T>().FindAsync(id);
            if (entity != null)
                return true;
            return false;

        }

        public async Task<IEnumerable<T>> FindInDataAsync(Expression<Func<T, bool>> predicate)
        {
             return await dbContext.Set<T>().Where(predicate).ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
           return await dbContext.Set<T>().ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await dbContext.Set<T>().FindAsync(id);
        }

        public async Task<IEnumerable<T>> GetPagedAsync(Expression<Func<T, bool>>? filter, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy, int pageNumber, int pageSize)
        {
            IQueryable<T> query = dbContext.Set<T>();

            // Optional filtering
            if (filter != null)
                query = query.Where(filter);

            // Optional sorting
            if (orderBy != null)
                query = orderBy(query);

            // Paging
            query = query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize);

            return await query.ToListAsync();
        }

        public async Task<T> UpdateAsync(T entity)
        {
            dbContext.Set<T>().Update(entity);
            return entity;
        }
    }
}
