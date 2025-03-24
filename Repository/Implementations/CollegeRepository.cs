using Microsoft.EntityFrameworkCore;
using Repository.Contracts;
using Repository.DBContext;
using System.Linq.Expressions;

namespace Repository.Implementations
{
    class CollegeRepository<T> : ICollegeRepository<T> where T : class
    {
        public readonly CollegeDBContext _dbContext;
        private readonly DbSet<T> _dbSet;
        public CollegeRepository(CollegeDBContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<T>();
        }

        public async Task<T> CreateAsync(T record)
        {
            await _dbSet.AddAsync(record);
            await _dbContext.SaveChangesAsync();
            return record;
        }

        public async Task<bool> DeleteAsync(T record)
        {
            _dbSet.Remove(record);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<T> GetAsync(Expression<Func<T, bool>> filter, bool asNoTracking = false)
        {
            if (asNoTracking)
            {
                return await _dbSet.AsNoTracking().Where(filter).FirstOrDefaultAsync();
            }
            return await _dbSet.Where(filter).FirstOrDefaultAsync();
        }

        //public async Task<T> GetByNameAsync(Expression<Func<T, bool>> filter)
        //{
        //    return await _dbSet.Where(filter).FirstOrDefaultAsync();
        //}

        public async Task<List<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<T> UpdateAsync(T record)
        {
            _dbSet.Update(record);
            await _dbContext.SaveChangesAsync();
            return record;
        }
    }
}
