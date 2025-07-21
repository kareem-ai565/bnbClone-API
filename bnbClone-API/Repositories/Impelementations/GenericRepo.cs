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
        virtual public async Task<T> AddAsync(T entity)
        {
            await dbContext.Set<T>().AddAsync(entity);
            return entity;
        }


        virtual public async Task<bool> DeleteAsync(int id)

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
            else if (entityType.GetProperty("Status") != null && entityType.GetProperty("Status")!.PropertyType == typeof(string))
            {
                if (entity is Property)
                {
                    entityType.GetProperty("Status")?.SetValue(entity, PropertyStatus.Suspended.ToString());
                }
                else if (entity is Booking)
                {
                    entityType.GetProperty("Status")?.SetValue(entity, BookingStatus.Cancelled.ToString());
                }
                else
                {
                    // Optional: fallback if entity has no supported status
                    throw new InvalidOperationException("Entity does not support soft delete via status enum.");
                }
            }
                dbContext.Set<T>().Update(entity);
                return true;
            
        }

        virtual public async Task<bool> ExistsAsync(int id)
        {
            var entity = await dbContext.Set<T>().FindAsync(id);
            if (entity != null)
                return true;
            return false;

        }

        virtual public async Task<IEnumerable<T>> FindInDataAsync(Expression<Func<T, bool>> predicate)
        {
             return await dbContext.Set<T>().Where(predicate).ToListAsync();
        }

        virtual public async Task<IEnumerable<T>> GetAllAsync()
        {
           return await dbContext.Set<T>().ToListAsync();
        }

        virtual public async Task<T> GetByIdAsync(int id)
        {
            return await dbContext.Set<T>().FindAsync(id);
        }

        virtual public async Task<IEnumerable<T>> GetPagedAsync(Expression<Func<T, bool>>? filter, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy, int pageNumber, int pageSize)
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

        virtual public async Task<T> UpdateAsync(T entity)
        {
            dbContext.Set<T>().Update(entity);
            return entity;
        }

       virtual public async Task<bool> FindAnyConAsync(Expression<Func<Booking, bool>> predicate)
        {
            return await dbContext.Set<Booking>().AnyAsync(predicate);
        }

        public virtual async Task<T?> FindFirstAsync(Expression<Func<T, bool>> predicate)
        {
            return await dbContext.Set<T>().FirstOrDefaultAsync(predicate);
        }

    }
}
