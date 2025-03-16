﻿using System.Linq.Expressions;

namespace Repository.Contracts
{
    public interface ICollegeRepository<T>
    {
        Task<List<T>> GetAllAsync();
        Task<T> GetByIdAsync(Expression<Func<T, bool>> filter, bool asNoTracking = false);
        Task<T> GetByNameAsync(Expression<Func<T, bool>> filter);
        Task<T> CreateAsync(T record);
        Task<T> UpdateAsync(T record);
        Task<bool> DeleteAsync(T record);
    }
}
