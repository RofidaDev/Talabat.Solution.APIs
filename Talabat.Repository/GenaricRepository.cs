using Microsoft.EntityFrameworkCore;
using Talabat.Core.Entities;
using Talabat.Core.Repositories.Contract;
using Talabat.Core.Specificatios;
using Talabat.Repository.Data;

namespace Talabat.Repository 
{
    public class GenaricRepository<T> : IGenaricRepository<T> where T : BaseEntity
    {
        private readonly StoreContext _dbContext;

        public GenaricRepository(StoreContext dbContext)  
        {
            _dbContext = dbContext;
        }
        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            //if (typeof(T) == typeof(Product))
            //    return (IReadOnlyList<T>)await _dbContext.Set<Product>().OrderBy(p=>p.Name).Include(p => p.Brand).Include(p => p.Category).ToListAsync();
               
            return await _dbContext.Set<T>().ToListAsync();
        }

        public async Task<T?> GetAsync(int id)
        {
           //if (typeof (T) == typeof(Product))
           //     return await _dbContext.Set<Product>().OrderBy(p=>p.Name).Include(P=>P.Brand).Include(P=>P.Category).FirstOrDefaultAsync() as T;
           return await _dbContext.Set<T>().FindAsync(id);
        }
        public async Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecification<T> spec)
        {
            return await ApplySpecifications(spec).ToListAsync();
        }
        public async Task<T?> GetWithSpecAsync(ISpecification<T> spec)
        {
            return await ApplySpecifications(spec).FirstOrDefaultAsync();
        }
        public async Task<int> GetCountSpecAsync(ISpecification<T> spec)
        {
            return await ApplySpecifications(spec).CountAsync();
        }
        private IQueryable<T> ApplySpecifications(ISpecification<T> spec)
        {
            return  SpecificationElevator<T>.GetQuery(_dbContext.Set<T>(), spec);
        }

        async Task IGenaricRepository<T>.AddAsync(T entity)
        => await _dbContext.AddRangeAsync(entity);
        void IGenaricRepository<T>.Update(T entity)
        => _dbContext.Update(entity);

        void IGenaricRepository<T>.Delete(T entity)
        => _dbContext.Remove(entity);
    }
}
