
using Talabat.Core.Entities;
using Talabat.Core.Specificatios;

namespace Talabat.Core.Repositories.Contract
{
    public interface IGenaricRepository<T> where T : BaseEntity
    {
        Task<T?> GetAsync(int id);
        Task<IReadOnlyList<T>> GetAllAsync();

        Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecification<T> spec);
        Task<T?> GetWithSpecAsync(ISpecification<T> spec);
        Task<int> GetCountSpecAsync(ISpecification<T> spec);

        Task AddAsync(T entity);
        void Update(T entity);
        void Delete(T entity);

    }
}
